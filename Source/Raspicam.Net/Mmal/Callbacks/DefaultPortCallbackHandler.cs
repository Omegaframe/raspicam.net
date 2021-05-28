using Raspicam.Net.Mmal.Handlers;
using Raspicam.Net.Mmal.Ports;

namespace Raspicam.Net.Mmal.Callbacks
{
    class DefaultPortCallbackHandler : PortCallbackHandler<IPort, ICaptureHandler>
    {
        public DefaultPortCallbackHandler(IPort port, ICaptureHandler handler) : base(port, handler) { }
    }
}
