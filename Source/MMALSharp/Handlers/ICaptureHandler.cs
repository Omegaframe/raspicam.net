using System;

namespace MMALSharp.Handlers
{
    interface ICaptureHandler : IDisposable
    {
        void Process(ImageContext context);
        void PostProcess();
    }
}
