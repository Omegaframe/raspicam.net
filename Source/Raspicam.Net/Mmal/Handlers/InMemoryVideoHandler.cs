using System;

namespace Raspicam.Net.Mmal.Handlers
{
    class InMemoryVideoHandler : ICaptureHandler
    {
        Action<byte[]> _onVideoDataAvailable;

        public void SetOnVideoDataAvailable(Action<byte[]> onDataAvailable)
        {
            _onVideoDataAvailable = onDataAvailable;
        }

        public void Process(byte[] data) => _onVideoDataAvailable?.Invoke(data);

        public void PostProcess() { }

        public void Dispose()
        {
            _onVideoDataAvailable = null;
        }
    }
}
