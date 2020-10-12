using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.Logging;
using MMALSharp.Common;
using MMALSharp.Common.Utility;
using MMALSharp.Components;
using MMALSharp.Native;
using MMALSharp.Ports;
using MMALSharp.Ports.Clocks;
using MMALSharp.Ports.Controls;
using MMALSharp.Ports.Inputs;
using MMALSharp.Ports.Outputs;
using static MMALSharp.MmalNativeExceptionHelper;

namespace MMALSharp
{
    public abstract unsafe class MMALComponentBase : MmalObject, IComponent
    {
        public IControlPort Control { get; }
        public List<IInputPort> Inputs { get; }
        public List<IOutputPort> Outputs { get; }
        public List<IPort> Clocks { get; }
        public List<IPort> Ports { get; }

        public string Name => Marshal.PtrToStringAnsi((IntPtr)Ptr->Name);
        public bool Enabled => Ptr->IsEnabled == 1;
        public bool ForceStopProcessing { get; set; }

        internal MMAL_COMPONENT_T* Ptr { get; }

        protected MMALComponentBase(string name)
        {
            Ptr = CreateComponent(name);

            Inputs = new List<IInputPort>();
            Outputs = new List<IOutputPort>();
            Clocks = new List<IPort>();
            Ports = new List<IPort>();

            Control = new ControlPort((IntPtr)Ptr->Control, this, Guid.NewGuid());

            for (int i = 0; i < Ptr->ClockNum; i++)
                Clocks.Add(new ClockPort((IntPtr)(&(*Ptr->Clock[i])), this, Guid.NewGuid()));

            for (int i = 0; i < Ptr->PortNum; i++)
                Ports.Add(new GenericPort((IntPtr)(&(*Ptr->Port[i])), this, PortType.Generic, Guid.NewGuid()));
        }

        public override bool CheckState() => Ptr != null && (IntPtr)Ptr != IntPtr.Zero;

        /// <summary>
        /// Enables any connections associated with this component, traversing down the pipeline to enable those connections
        /// also.
        /// </summary>
        /// <exception cref="MmalPortConnectedException">Thrown when port enabled prior to enabling connection.</exception>
        public void EnableConnections()
        {
            foreach (IOutputPort port in Outputs.Where(o => o.ConnectedReference != null))
            {
                // This component has an output port connected to another component.
                port.ConnectedReference.DownstreamComponent.EnableConnections();

                // Enable the connection
                port.ConnectedReference.Enable();
            }
        }

        /// <summary>
        /// Disables any connections associated with this component, traversing down the pipeline to disable those connections
        /// also.
        /// </summary>
        /// <exception cref="MmalPortConnectedException">Thrown when port still enabled prior to disabling connection.</exception>
        public void DisableConnections()
        {
            foreach (IOutputPort port in Outputs.Where(o => o.ConnectedReference != null))
            {
                MmalLog.Logger.LogDebug($"Disabling connection between {Name} and {port.ConnectedReference.DownstreamComponent.Name}");

                // This component has an output port connected to another component.
                port.ConnectedReference.DownstreamComponent.DisableConnections();

                // Disable the connection
                port.ConnectedReference.Disable();
            }
        }

        /// <summary>
        /// Prints a summary of the ports associated with this component to the console.
        /// </summary>
        public virtual void PrintComponent()
        {
            MmalLog.Logger.LogInformation($"Component: {Name}");

            var sb = new StringBuilder();

            for (var i = 0; i < Inputs.Count; i++)
            {
                if (Inputs[i].EncodingType != null)
                {
                    sb.Append($"    INPUT port {i} encoding: {Inputs[i].NativeEncodingType.ParseEncoding().EncodingName}. \n");
                    sb.Append($"        Width: {Inputs[i].Resolution.Width}. Height: {Inputs[i].Resolution.Height} \n");
                    sb.Append($"        Num buffers: {Inputs[i].BufferNum}. Buffer size: {Inputs[i].BufferSize}. \n");
                    sb.Append($"        Rec num buffers: {Inputs[i].BufferNumRecommended}. Rec buffer size: {Inputs[i].BufferSizeRecommended} \n");
                    sb.Append($"        Resolution: {Inputs[i].Resolution.Width} x {Inputs[i].Resolution.Height} \n");
                    sb.Append($"        Crop: {Inputs[i].Crop.Width} x {Inputs[i].Crop.Height} \n \n");
                }
            }

            for (var i = 0; i < Outputs.Count; i++)
            {
                if (Outputs[i].EncodingType != null)
                {
                    sb.Append($"    OUTPUT port {i} encoding: {Outputs[i].NativeEncodingType.ParseEncoding().EncodingName}. \n");
                    sb.Append($"        Width: {Outputs[i].Resolution.Width}. Height: {Outputs[i].Resolution.Height} \n");
                    sb.Append($"        Num buffers: {Outputs[i].BufferNum}. Buffer size: {Outputs[i].BufferSize}. \n");
                    sb.Append($"        Rec num buffers: {Outputs[i].BufferNumRecommended}. Rec buffer size: {Outputs[i].BufferSizeRecommended} \n");
                    sb.Append($"        Resolution: {Outputs[i].Resolution.Width} x {Outputs[i].Resolution.Height} \n");
                    sb.Append($"        Crop: {Outputs[i].Crop.Width} x {Outputs[i].Crop.Height} \n \n");
                }
            }

            MmalLog.Logger.LogInformation(sb.ToString());
        }

        /// <summary>
        /// Disposes of the current component, and frees any native resources still in use by it.
        /// </summary>
        public override void Dispose()
        {
            if (!CheckState())
                return;

            MmalLog.Logger.LogDebug($"Disposing component {Name}.");

            // See if any pools need disposing before destroying component.
            foreach (var port in Inputs.Where(i => i.BufferPool != null))
            {
                MmalLog.Logger.LogDebug("Destroying port pool");

                port.DestroyPortPool();
            }

            foreach (var port in Outputs.Where(i => i.BufferPool != null))
            {
                MmalLog.Logger.LogDebug("Destroying port pool");

                port.DestroyPortPool();
            }

            DisableComponent();
            DestroyComponent();

            MmalLog.Logger.LogDebug("Completed disposal...");

            base.Dispose();
        }

        /// <summary>
        /// Acquire a reference on a component. Acquiring a reference on a component will prevent a component from being destroyed until the 
        /// acquired reference is released (by a call to mmal_component_destroy). References are internally counted so all acquired references 
        /// need a matching call to release them.
        /// </summary>
        public void AcquireComponent() => MMALComponent.mmal_component_acquire(Ptr);

        /// <summary>
        /// Release a reference on a component Release an acquired reference on a component. Triggers the destruction of the component 
        /// when the last reference is being released.
        /// </summary>
        public void ReleaseComponent() => MmalCheck(MMALComponent.mmal_component_release(Ptr), "Unable to release component");

        /// <summary>
        /// Destroy a previously created component Release an acquired reference on a component. 
        /// Only actually destroys the component when the last reference is being released.
        /// </summary>
        public void DestroyComponent() => MmalCheck(MMALComponent.mmal_component_destroy(Ptr), "Unable to destroy component");

        /// <summary>
        /// Enable processing on a component.
        /// </summary>
        public void EnableComponent()
        {
            if (!Enabled)
                MmalCheck(MMALComponent.mmal_component_enable(Ptr), "Unable to enable component");
        }

        /// <summary>
        /// Disable processing on a component.
        /// </summary>
        public void DisableComponent()
        {
            if (Enabled)
                MmalCheck(MMALComponent.mmal_component_disable(Ptr), "Unable to disable component");
        }

        /// <summary>
        /// Helper method to destroy any port pools still in action. Failure to do this will cause MMAL to block indefinitely.
        /// </summary>
        public void CleanPortPools()
        {
            // See if any pools need disposing before destroying component.
            foreach (var port in Inputs.Where(i => i.BufferPool != null))
            {
                MmalLog.Logger.LogDebug("Destroying input port pool.");

                port.DestroyPortPool();
            }

            foreach (var port in Outputs.Where(i => i.BufferPool != null))
            {
                MmalLog.Logger.LogDebug("Destroying output port pool.");

                port.DestroyPortPool();
            }
        }

        /// <summary>
        /// Provides a facility to create a component with a given name.
        /// </summary>
        /// <param name="name">The name of the component to create.</param>
        /// <returns>A pointer to the new component struct.</returns>
        static MMAL_COMPONENT_T* CreateComponent(string name)
        {
            IntPtr ptr = IntPtr.Zero;
            MmalCheck(MMALComponent.mmal_component_create(name, &ptr), "Unable to create component");

            var compPtr = (MMAL_COMPONENT_T*)ptr.ToPointer();

            return compPtr;
        }
    }
}
