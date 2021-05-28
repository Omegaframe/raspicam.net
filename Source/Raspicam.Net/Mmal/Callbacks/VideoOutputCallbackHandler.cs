using Raspicam.Net.Mmal.Handlers;
using Raspicam.Net.Mmal.Ports.Outputs;

namespace Raspicam.Net.Mmal.Callbacks
{
    class VideoOutputCallbackHandler : PortCallbackHandler<IVideoPort, ICaptureHandler>, IVideoOutputCallbackHandler
    {
        public VideoOutputCallbackHandler(IVideoPort port, ICaptureHandler handler) : base(port, handler) { }
    }
}
