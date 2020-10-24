using MMALSharp.Mmal.Handlers;
using MMALSharp.Mmal.Ports.Outputs;

namespace MMALSharp.Mmal.Callbacks
{
    class FastImageOutputCallbackHandler : PortCallbackHandler<IVideoPort, ICaptureHandler>
    {
        public FastImageOutputCallbackHandler(IVideoPort port, ICaptureHandler handler) : base(port, handler) { }

        public override void Callback(IBuffer buffer)
        {
            base.Callback(buffer);
        }
    }
}