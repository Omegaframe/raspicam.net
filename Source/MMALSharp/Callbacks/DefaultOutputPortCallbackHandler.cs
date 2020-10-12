using MMALSharp.Handlers;
using MMALSharp.Ports.Outputs;

namespace MMALSharp.Callbacks
{
    public class DefaultOutputPortCallbackHandler : PortCallbackHandler<IOutputPort, IOutputCaptureHandler>
    {
        public DefaultOutputPortCallbackHandler(IOutputPort port, IOutputCaptureHandler handler) : base(port, handler) { }
    }
}
