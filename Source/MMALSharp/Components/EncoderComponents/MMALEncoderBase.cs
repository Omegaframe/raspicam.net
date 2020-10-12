namespace MMALSharp.Components.EncoderComponents
{
    public abstract class MmalEncoderBase : MmalDownstreamHandlerComponent, IEncoder
    {
        protected MmalEncoderBase(string encoderName) : base(encoderName) { }
    }
}
