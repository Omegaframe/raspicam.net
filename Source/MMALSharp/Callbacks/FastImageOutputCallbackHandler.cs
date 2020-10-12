using MMALSharp.Native;
using MMALSharp.Ports.Outputs;
using MMALSharp.Processing.Handlers;

namespace MMALSharp.Callbacks
{
    public class FastImageOutputCallbackHandler : PortCallbackHandler<IVideoPort, IOutputCaptureHandler>
    {
        public FastImageOutputCallbackHandler(IVideoPort port, IOutputCaptureHandler handler) : base(port, handler) { }

        public override void Callback(IBuffer buffer)
        {
            base.Callback(buffer);

            var eos = buffer.AssertProperty(MmalBufferProperties.MmalBufferHeaderFlagFrameEnd) ||
                      buffer.AssertProperty(MmalBufferProperties.MmalBufferHeaderFlagEos);

            if (eos && CaptureHandler is IFileStreamCaptureHandler handler)
                handler.NewFile();
        }
    }
}