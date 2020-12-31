using System;

namespace MMALSharp.Mmal
{
    interface IMmalObject : IDisposable
    {
        bool CheckState();
        bool IsDisposed { get; }
    }
}
