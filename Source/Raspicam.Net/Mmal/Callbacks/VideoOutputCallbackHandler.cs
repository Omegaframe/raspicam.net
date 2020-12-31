using MMALSharp.Mmal.Handlers;
using MMALSharp.Mmal.Ports.Outputs;

namespace MMALSharp.Mmal.Callbacks
{
    class VideoOutputCallbackHandler : PortCallbackHandler<IVideoPort, ICaptureHandler>, IVideoOutputCallbackHandler
    {
        public VideoOutputCallbackHandler(IVideoPort port, ICaptureHandler handler) : base(port, handler) { }
    }
}
