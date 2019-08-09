﻿// <copyright file="MMALDownstreamComponent.cs" company="Techyian">
// Copyright (c) Ian Auty. All rights reserved.
// Licensed under the MIT License. Please see LICENSE.txt for License info.
// </copyright>

using System;
using System.Collections.Generic;
using System.Drawing;
using MMALSharp.Callbacks;
using MMALSharp.Callbacks.Providers;
using MMALSharp.Common.Utility;
using MMALSharp.Native;
using MMALSharp.Ports;
using MMALSharp.Ports.Outputs;

namespace MMALSharp.Components
{
    /// <summary>
    /// Represents a downstream component. A downstream component is a component that can have data passed to it from further up the component
    /// heirarchy.
    /// </summary>
    public abstract class MMALDownstreamComponent : MMALComponentBase, IDownstreamComponent
    {
        /// <summary>
        /// A list of working ports which are processing data in the component pipeline.
        /// </summary>
        public Dictionary<int, IOutputPort> ProcessingPorts { get; set; }

        /// <summary>
        /// Creates a new instance of a Downstream component.
        /// </summary>
        /// <param name="name">The name of the component.</param>
        protected MMALDownstreamComponent(string name)
            : base(name)
        {
            MMALBootstrapper.DownstreamComponents.Add(this);
            this.ProcessingPorts = new Dictionary<int, IOutputPort>();
        }

        /// <summary>
        /// Registers a <see cref="ICallbackHandler"/>.
        /// </summary>
        /// <param name="handler">The port handler.</param>
        public void RegisterPortCallback(ICallbackHandler handler) => PortCallbackProvider.RegisterCallback(handler);

        /// <summary>
        /// If it exists, removes a <see cref="ICallbackHandler"/> on this component's input port.
        /// </summary>
        /// <param name="port">The port to remove a handler on.</param>
        public void RemovePortCallback(IPort port) => PortCallbackProvider.RemoveCallback(port);
        
        /// <summary>
        /// Registers a <see cref="IConnectionCallbackHandler"/>.
        /// </summary>
        /// <param name="handler">The output handler.</param>
        public void RegisterConnectionCallback(IConnectionCallbackHandler handler) => ConnectionCallbackProvider.RegisterCallback(handler);

        /// <summary>
        /// If it exists, removes a <see cref="IConnectionCallbackHandler"/> on the port specified.
        /// </summary>
        /// <param name="port">The port with a created connection.</param>
        public void RemoveConnectionCallback(IPort port) =>
            ConnectionCallbackProvider.RemoveCallback(port.ConnectedReference);

        /// <summary>
        /// Configures a specific input port on a downstream component. This method will perform a shallow copy of the output
        /// port it is to be connected to.
        /// </summary>
        /// <param name="config">User provided port configuration object.</param>
        /// <param name="copyPort">The output port we are copying format data from.</param>
        /// <param name="zeroCopy">Instruct MMAL to not copy buffers to ARM memory (useful for large buffers and handling raw data).</param>
        /// <returns>This <see cref="MMALDownstreamComponent"/>.</returns>
        public virtual unsafe IDownstreamComponent ConfigureInputPort(MMALPortConfig config, IPort copyPort, bool zeroCopy = false)
        {
            this.Inputs[0].Configure(config, copyPort, zeroCopy);

            if (this.Outputs.Count > 0 && this.Outputs[0].Ptr->Format->Type == MMALFormat.MMAL_ES_TYPE_T.MMAL_ES_TYPE_UNKNOWN)
            {
                throw new PiCameraError("Unable to determine settings for output port.");
            }

            return this;
        }

        /// <summary>
        /// Call to configure changes on a downstream component input port.
        /// </summary>
        /// <param name="config">User provided port configuration object.</param>
        /// <returns>This <see cref="MMALDownstreamComponent"/>.</returns>
        public virtual unsafe IDownstreamComponent ConfigureInputPort(MMALPortConfig config)
        {
            this.Inputs[0].Configure(config);

            if (this.Outputs.Count > 0 && this.Outputs[0].Ptr->Format->Type == MMALFormat.MMAL_ES_TYPE_T.MMAL_ES_TYPE_UNKNOWN)
            {
                throw new PiCameraError("Unable to determine settings for output port.");
            }

            return this;
        }
        
        /// <summary>
        /// Call to configure changes on the first downstream component output port.
        /// </summary>
        /// <param name="config">User provided port configuration object.</param>
        /// <returns>This <see cref="MMALDownstreamComponent"/>.</returns>
        public virtual IDownstreamComponent ConfigureOutputPort(MMALPortConfig config)
        {
            return this.ConfigureOutputPort(0, config);
        }

        /// <summary>
        /// Call to configure changes on a downstream component output port.
        /// </summary>
        /// <param name="outputPort">The output port number to configure.</param>
        /// <param name="config">User provided port configuration object.</param>
        /// <returns>This <see cref="MMALDownstreamComponent"/>.</returns>
        public virtual unsafe IDownstreamComponent ConfigureOutputPort(int outputPort, MMALPortConfig config)
        {
            if (this.ProcessingPorts.ContainsKey(outputPort))
            {
                this.ProcessingPorts.Remove(outputPort);
            }

            this.ProcessingPorts.Add(outputPort, this.Outputs[outputPort]);
            
            this.Outputs[outputPort].Configure(config, this.Inputs[0]);

            return this;
        }

        /// <summary>
        /// Disposes of the current component, closes all connections and frees all associated unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            MMALLog.Logger.Debug("Removing downstream component");

            this.ClosePipelineConnections();

            MMALBootstrapper.DownstreamComponents.Remove(this);

            MMALLog.Logger.Debug($"Remaining components in pipeline: {MMALBootstrapper.DownstreamComponents.Count}");

            base.Dispose();
        }
        
        /// <summary>
        /// Responsible for closing and destroying any connections associated with this component prior to disposing.
        /// </summary>
        private void ClosePipelineConnections()
        {
            // Close any connection held by this component
            foreach (var input in this.Inputs)
            {
                if (input.ConnectedReference != null)
                {
                    MMALLog.Logger.Debug($"Removing {input.ConnectedReference.ToString()}");
                    input.ConnectedReference.OutputPort.ConnectedReference?.Dispose();
                    input.ConnectedReference.Dispose();
                }
            }

            foreach (var output in this.Outputs)
            {
                if (output.ConnectedReference != null)
                {
                    MMALLog.Logger.Debug($"Removing {output.ConnectedReference.ToString()}");
                    output.ConnectedReference.Dispose();
                }
            }
        }
    }
}
