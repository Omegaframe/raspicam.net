using MMALSharp.Handlers;
using MMALSharp.Ports.Outputs;

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