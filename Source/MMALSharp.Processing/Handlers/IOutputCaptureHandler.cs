using MMALSharp.Common;

namespace MMALSharp.Processing.Handlers
{
    public interface IOutputCaptureHandler : ICaptureHandler
    {
        void Process(ImageContext context);
        void PostProcess();
    }
}
