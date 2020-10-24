using System.Collections.Generic;
using MMALSharp.Handlers;
using MMALSharp.Ports;
using MMALSharp.Ports.Outputs;

namespace MMALSharp.Components
{
    public interface IDownstreamComponent : IComponent
    {
        Dictionary<int, IOutputPort> ProcessingPorts { get; }

        IDownstreamComponent ConfigureOutputPort(IMmalPortConfig config, ICaptureHandler handler);
        IDownstreamComponent ConfigureOutputPort(int outputPort, IMmalPortConfig config, ICaptureHandler handler);
        IDownstreamComponent ConfigureOutputPort<TPort>(int outputPort, IMmalPortConfig config, ICaptureHandler handler) where TPort : IOutputPort;
    }
}
