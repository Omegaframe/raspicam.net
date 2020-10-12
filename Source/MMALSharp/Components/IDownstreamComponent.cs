using System.Collections.Generic;
using MMALSharp.Ports;
using MMALSharp.Ports.Inputs;
using MMALSharp.Ports.Outputs;
using MMALSharp.Processing.Handlers;

namespace MMALSharp.Components
{
    public interface IDownstreamComponent : IComponent
    {
        Dictionary<int, IOutputPort> ProcessingPorts { get; }

        IDownstreamComponent ConfigureInputPort(IMmalPortConfig config, IPort copyPort, IInputCaptureHandler handler);
        IDownstreamComponent ConfigureInputPort(IMmalPortConfig config, IInputCaptureHandler handler);
        IDownstreamComponent ConfigureInputPort<TPort>(IMmalPortConfig config, IInputCaptureHandler handler) where TPort : IInputPort;
        IDownstreamComponent ConfigureOutputPort(IMmalPortConfig config, IOutputCaptureHandler handler);
        IDownstreamComponent ConfigureOutputPort(int outputPort, IMmalPortConfig config, IOutputCaptureHandler handler);
        IDownstreamComponent ConfigureOutputPort<TPort>(int outputPort, IMmalPortConfig config, IOutputCaptureHandler handler) where TPort : IOutputPort;
    }
}
