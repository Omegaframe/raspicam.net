using System.Collections.Generic;
using MMALSharp.Mmal.Handlers;
using MMALSharp.Mmal.Ports;
using MMALSharp.Mmal.Ports.Outputs;

namespace MMALSharp.Mmal.Components
{
    interface IDownstreamComponent : IComponent
    {
        Dictionary<int, IOutputPort> ProcessingPorts { get; }

        IDownstreamComponent ConfigureOutputPort(IMmalPortConfig config, ICaptureHandler handler);
        IDownstreamComponent ConfigureOutputPort(int outputPort, IMmalPortConfig config, ICaptureHandler handler);
        IDownstreamComponent ConfigureOutputPort<TPort>(int outputPort, IMmalPortConfig config, ICaptureHandler handler) where TPort : IOutputPort;
    }
}
