using MMALSharp.Ports.Outputs;
using MMALSharp.Processing.Handlers;

namespace MMALSharp.Callbacks
{
    public class DefaultOutputPortCallbackHandler : PortCallbackHandler<IOutputPort, IOutputCaptureHandler>
    {
        public DefaultOutputPortCallbackHandler(IOutputPort port, IOutputCaptureHandler handler) : base(port, handler) { }
    }
}
