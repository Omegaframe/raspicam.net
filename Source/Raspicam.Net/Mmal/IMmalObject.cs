using System;

namespace Raspicam.Net.Mmal
{
    interface IMmalObject : IDisposable
    {
        bool CheckState();
        bool IsDisposed { get; }
    }
}
