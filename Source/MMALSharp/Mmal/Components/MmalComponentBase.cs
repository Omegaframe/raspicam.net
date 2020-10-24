using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.Logging;
using MMALSharp.Mmal.Ports;
using MMALSharp.Mmal.Ports.Clocks;
using MMALSharp.Mmal.Ports.Controls;
using MMALSharp.Mmal.Ports.Inputs;
using MMALSharp.Mmal.Ports.Outputs;
using MMALSharp.Native.Component;
using MMALSharp.Utility;
using static MMALSharp.MmalNativeExceptionHelper;

namespace MMALSharp.Mmal.Components
{
    abstract unsafe class MmalComponentBase : MmalObject, IComponent
    {
        public IControlPort Control { get; }
        public List<IInputPort> Inputs { get; }
        public List<IOutputPort> Outputs { get; }
        public List<IPort> Clocks { get; }
        public List<IPort> Ports { get; }

        public string Name => Marshal.PtrToStringAnsi((IntPtr)Ptr->Name);
        public bool Enabled => Ptr->IsEnabled == 1;
        public bool ForceStopProcessing { get; set; }

        internal MmalComponentType* Ptr { get; }

        protected MmalComponentBase(string name)
        {
            Ptr = CreateComponent(name);

            Inputs = new List<IInputPort>();
            Outputs = new List<IOutputPort>();
            Clocks = new List<IPort>();
            Ports = new List<IPort>();

            Control = new ControlPort((IntPtr)Ptr->Control, this, Guid.NewGuid());

            for (var i = 0; i < Ptr->ClockNum; i++)
                Clocks.Add(new ClockPort((IntPtr)(&(*Ptr->Clock[i])), this, Guid.NewGuid()));

            for (var i = 0; i < Ptr->PortNum; i++)
                Ports.Add(new GenericPort((IntPtr)(&(*Ptr->Port[i])), this, PortType.Generic, Guid.NewGuid()));
        }

        public override bool CheckState() => Ptr != null && (IntPtr)Ptr != IntPtr.Zero;

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

        public void DisableConnections()
        {
            foreach (var port in Outputs.Where(o => o.ConnectedReference != null))
            {
                MmalLog.Logger.LogDebug($"Disabling connection between {Name} and {port.ConnectedReference.DownstreamComponent.Name}");

                // This component has an output port connected to another component.
                port.ConnectedReference.DownstreamComponent.DisableConnections();

                // Disable the connection
                port.ConnectedReference.Disable();
            }
        }

        public virtual void PrintComponent()
        {
            MmalLog.Logger.LogInformation($"Component: {Name}");

            var sb = new StringBuilder();

            for (var i = 0; i < Inputs.Count; i++)
            {
                if (Inputs[i].EncodingType == null)
                    continue;

                sb.Append($"    INPUT port {i} encoding: {Inputs[i].NativeEncodingType.ParseEncoding().EncodingName}. \n");
                sb.Append($"        Width: {Inputs[i].Resolution.Width}. Height: {Inputs[i].Resolution.Height} \n");
                sb.Append($"        Num buffers: {Inputs[i].BufferNum}. Buffer size: {Inputs[i].BufferSize}. \n");
                sb.Append($"        Rec num buffers: {Inputs[i].BufferNumRecommended}. Rec buffer size: {Inputs[i].BufferSizeRecommended} \n");
                sb.Append($"        Resolution: {Inputs[i].Resolution.Width} x {Inputs[i].Resolution.Height} \n");
                sb.Append($"        Crop: {Inputs[i].Crop.Width} x {Inputs[i].Crop.Height} \n \n");
            }

            for (var i = 0; i < Outputs.Count; i++)
            {
                if (Outputs[i].EncodingType == null)
                    continue;

                sb.Append($"    OUTPUT port {i} encoding: {Outputs[i].NativeEncodingType.ParseEncoding().EncodingName}. \n");
                sb.Append($"        Width: {Outputs[i].Resolution.Width}. Height: {Outputs[i].Resolution.Height} \n");
                sb.Append($"        Num buffers: {Outputs[i].BufferNum}. Buffer size: {Outputs[i].BufferSize}. \n");
                sb.Append($"        Rec num buffers: {Outputs[i].BufferNumRecommended}. Rec buffer size: {Outputs[i].BufferSizeRecommended} \n");
                sb.Append($"        Resolution: {Outputs[i].Resolution.Width} x {Outputs[i].Resolution.Height} \n");
                sb.Append($"        Crop: {Outputs[i].Crop.Width} x {Outputs[i].Crop.Height} \n \n");
            }

            MmalLog.Logger.LogInformation(sb.ToString());
        }

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

        public void AcquireComponent() => MmalComponent.Acquire(Ptr);
        public void ReleaseComponent() => MmalCheck(MmalComponent.Release(Ptr), "Unable to release component");
        public void DestroyComponent() => MmalCheck(MmalComponent.Destroy(Ptr), "Unable to destroy component");

        public void EnableComponent()
        {
            if (!Enabled)
                MmalCheck(MmalComponent.Enable(Ptr), "Unable to enable component");
        }

        public void DisableComponent()
        {
            if (Enabled)
                MmalCheck(MmalComponent.Disable(Ptr), "Unable to disable component");
        }

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

        static MmalComponentType* CreateComponent(string name)
        {
            var ptr = IntPtr.Zero;
            MmalCheck(MmalComponent.Create(name, &ptr), "Unable to create component");

            var compPtr = (MmalComponentType*)ptr.ToPointer();

            return compPtr;
        }
    }
}
