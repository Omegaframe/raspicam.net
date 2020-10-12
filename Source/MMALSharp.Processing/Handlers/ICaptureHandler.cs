using System;

namespace MMALSharp.Handlers
{
    public interface ICaptureHandler : IDisposable
    {
        string TotalProcessed();
    }
}
