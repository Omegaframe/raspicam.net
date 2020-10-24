using System;

namespace MMALSharp.Mmal.Handlers
{
    interface ICaptureHandler : IDisposable
    {
        void Process(ImageContext context);
        void PostProcess();
    }
}
