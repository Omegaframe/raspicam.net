using MMALSharp.Ports;
using MMALSharp.Processing.Handlers;

namespace MMALSharp.Callbacks
{
    public class DefaultPortCallbackHandler : PortCallbackHandler<IPort, ICaptureHandler>
    {
        public DefaultPortCallbackHandler(IPort port, ICaptureHandler handler) : base(port, handler) { }
    }
}
