using MMALSharp.Mmal.Handlers;
using MMALSharp.Mmal.Ports;

namespace MMALSharp.Mmal.Callbacks
{
    class DefaultPortCallbackHandler : PortCallbackHandler<IPort, ICaptureHandler>
    {
        public DefaultPortCallbackHandler(IPort port, ICaptureHandler handler) : base(port, handler) { }
    }
}
