using MMALSharp.Common;

namespace MMALSharp.Processing.Processors
{
    public interface IFrameAnalyser
    {
        void Apply(ImageContext context);
    }
}
