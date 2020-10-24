using System;
using MMALSharp.Common;

namespace MMALSharp.Handlers
{
    public interface ICaptureHandler : IDisposable
    {
        void Process(ImageContext context);
        void PostProcess();
    }
}
