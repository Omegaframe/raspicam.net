namespace MMALSharp.Processing.Processors
{
    public interface IFrameProcessingContext
    {
        IFrameProcessingContext Apply(IFrameProcessor processor);
    }
}
