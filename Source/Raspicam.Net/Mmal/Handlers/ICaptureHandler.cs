using System;

namespace Raspicam.Net.Mmal.Handlers
{
    interface ICaptureHandler : IDisposable
    {
        void Process(ImageContext context);
        void PostProcess();
    }
}
