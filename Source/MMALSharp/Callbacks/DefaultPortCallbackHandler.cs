using MMALSharp.Handlers;
using MMALSharp.Ports;

namespace MMALSharp.Callbacks
{
    public class DefaultPortCallbackHandler : PortCallbackHandler<IPort, IOutputCaptureHandler>
    {
        public DefaultPortCallbackHandler(IPort port, IOutputCaptureHandler handler) : base(port, handler) { }
    }
}
