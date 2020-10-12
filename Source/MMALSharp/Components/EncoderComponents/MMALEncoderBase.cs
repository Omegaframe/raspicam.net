using MMALSharp.Components.EncoderComponents;

namespace MMALSharp.Components
{
    public abstract class MMALEncoderBase : MMALDownstreamHandlerComponent, IEncoder
    {
        protected MMALEncoderBase(string encoderName) : base(encoderName) { }
    }
}
