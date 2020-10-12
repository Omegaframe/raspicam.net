using System;

namespace MMALSharp
{
    public interface IMmalObject : IDisposable
    {
        bool CheckState();
        bool IsDisposed { get; }
    }
}
