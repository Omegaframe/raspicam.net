using System;

namespace MMALSharp.Processing.Handlers
{
    public interface ICaptureHandler : IDisposable
    {
        string TotalProcessed();
    }
}
