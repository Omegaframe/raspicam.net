using MMALSharp.Ports;
using MMALSharp.Processing.Handlers;

namespace MMALSharp.Callbacks
{
    public class DefaultPortCallbackHandler : PortCallbackHandler<IPort, IOutputCaptureHandler>
    {
        public DefaultPortCallbackHandler(IPort port, IOutputCaptureHandler handler) : base(port, handler) { }
    }
}
