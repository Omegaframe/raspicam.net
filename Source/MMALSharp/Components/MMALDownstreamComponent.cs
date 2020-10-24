using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using MMALSharp.Handlers;
using MMALSharp.Ports;
using MMALSharp.Ports.Outputs;
using MMALSharp.Utility;

namespace MMALSharp.Components
{
    abstract class MmalDownstreamComponent : MmalComponentBase, IDownstreamComponent
    {
        public Dictionary<int, IOutputPort> ProcessingPorts { get; set; }

        protected MmalDownstreamComponent(string name) : base(name)
        {
            MmalBootstrapper.DownstreamComponents.Add(this);
            ProcessingPorts = new Dictionary<int, IOutputPort>();
        }

        

        public virtual IDownstreamComponent ConfigureOutputPort(IMmalPortConfig config, ICaptureHandler handler) => ConfigureOutputPort(0, config, handler);

        public virtual IDownstreamComponent ConfigureOutputPort(int outputPort, IMmalPortConfig config, ICaptureHandler handler)
        {
            if (ProcessingPorts.ContainsKey(outputPort))
                ProcessingPorts.Remove(outputPort);

            ProcessingPorts.Add(outputPort, Outputs[outputPort]);

            Outputs[outputPort].Configure(config, Inputs[0], handler);

            return this;
        }

        public virtual unsafe IDownstreamComponent ConfigureOutputPort<TPort>(int outputPort, IMmalPortConfig config, ICaptureHandler handler) where TPort : IOutputPort
        {
            Outputs[outputPort] = (IOutputPort)Activator.CreateInstance(typeof(TPort), (IntPtr)(&(*Ptr->Output[outputPort])), this, Guid.NewGuid());

            return ConfigureOutputPort(outputPort, config, handler);
        }

        public override void Dispose()
        {
            ClosePipelineConnections();

            MmalBootstrapper.DownstreamComponents.Remove(this);

            MmalLog.Logger.LogDebug($"Remaining components in pipeline: {MmalBootstrapper.DownstreamComponents.Count}");

            base.Dispose();
        }

        void ClosePipelineConnections()
        {
            // Close any connection held by this component
            foreach (var input in Inputs.Where(i => i.ConnectedReference != null))
            {
                MmalLog.Logger.LogDebug($"Removing {input.ConnectedReference}");
                input.ConnectedReference.Dispose();
            }

            foreach (var output in Outputs.Where(o => o.ConnectedReference != null))
            {
                MmalLog.Logger.LogDebug($"Removing {output.ConnectedReference}");
                output.ConnectedReference.Dispose();
            }
        }
    }
}
