using Raspicam.Net.Mmal.Handlers;
using Raspicam.Net.Mmal.Ports.Outputs;

namespace Raspicam.Net.Mmal.Callbacks
{
    class FastImageOutputCallbackHandler : PortCallbackHandler<IVideoPort, ICaptureHandler>
    {
        public FastImageOutputCallbackHandler(IVideoPort port, ICaptureHandler handler) : base(port, handler) { }
    }
}