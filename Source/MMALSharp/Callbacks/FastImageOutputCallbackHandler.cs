using MMALSharp.Handlers;
using MMALSharp.Native;
using MMALSharp.Ports.Outputs;

namespace MMALSharp.Callbacks
{
    public class FastImageOutputCallbackHandler : PortCallbackHandler<IVideoPort, IOutputCaptureHandler>
    {
        public FastImageOutputCallbackHandler(IVideoPort port, IOutputCaptureHandler handler) : base(port, handler) { }

        public override void Callback(IBuffer buffer)
        {
            base.Callback(buffer);

            var eos = buffer.AssertProperty(MMALBufferProperties.MMAL_BUFFER_HEADER_FLAG_FRAME_END) ||
                      buffer.AssertProperty(MMALBufferProperties.MMAL_BUFFER_HEADER_FLAG_EOS);

            if (eos && CaptureHandler is IFileStreamCaptureHandler handler)
                handler.NewFile();
        }
    }
}