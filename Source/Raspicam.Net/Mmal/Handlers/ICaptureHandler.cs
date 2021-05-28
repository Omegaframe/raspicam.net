using System;

namespace Raspicam.Net.Mmal.Handlers
{
    interface ICaptureHandler : IDisposable
    {
        void Process(byte[] data);
        void PostProcess();
    }
}
