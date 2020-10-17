using MMALSharp.Ports.Outputs;
using MMALSharp.Processing.Handlers;

namespace MMALSharp.Callbacks
{
    public class FastImageOutputCallbackHandler : PortCallbackHandler<IVideoPort, ICaptureHandler>
    {
        public FastImageOutputCallbackHandler(IVideoPort port, ICaptureHandler handler) : base(port, handler) { }

        public override void Callback(IBuffer buffer)
        {
            base.Callback(buffer);
        }
    }
}