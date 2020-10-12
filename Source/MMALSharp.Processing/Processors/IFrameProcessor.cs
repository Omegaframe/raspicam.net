using MMALSharp.Common;

namespace MMALSharp.Processing.Processors
{
    public interface IFrameProcessor
    {
        void Apply(ImageContext context);
    }
}
