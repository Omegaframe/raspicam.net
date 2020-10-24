using System;
using MMALSharp.Common;

namespace MMALSharp.Handlers
{
    interface ICaptureHandler : IDisposable
    {
        void Process(ImageContext context);
        void PostProcess();
    }
}
