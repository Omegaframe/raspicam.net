using System;

namespace MMALSharp
{
    interface IMmalObject : IDisposable
    {
        bool CheckState();
        bool IsDisposed { get; }
    }
}
