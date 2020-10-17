using MMALSharp.Common;
using System;

namespace MMALSharp.Processing.Handlers
{
    public interface ICaptureHandler : IDisposable
    {
        void Process(ImageContext context);
        void PostProcess();
    }
}
