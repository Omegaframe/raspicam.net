using MMALSharp.Handlers;
using MMALSharp.Ports;

namespace MMALSharp.Callbacks
{
    public class DefaultPortCallbackHandler : PortCallbackHandler<IPort, ICaptureHandler>
    {
        public DefaultPortCallbackHandler(IPort port, ICaptureHandler handler) : base(port, handler) { }
    }
}
