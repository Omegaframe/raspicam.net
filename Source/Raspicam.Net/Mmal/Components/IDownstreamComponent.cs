using System.Collections.Generic;
using Raspicam.Net.Mmal.Handlers;
using Raspicam.Net.Mmal.Ports;
using Raspicam.Net.Mmal.Ports.Outputs;

namespace Raspicam.Net.Mmal.Components
{
    interface IDownstreamComponent : IComponent
    {
        Dictionary<int, IOutputPort> ProcessingPorts { get; }

        IDownstreamComponent ConfigureOutputPort(IMmalPortConfig config, ICaptureHandler handler);
        IDownstreamComponent ConfigureOutputPort(int outputPort, IMmalPortConfig config, ICaptureHandler handler);
        IDownstreamComponent ConfigureOutputPort<TPort>(int outputPort, IMmalPortConfig config, ICaptureHandler handler) where TPort : IOutputPort;
    }
}
