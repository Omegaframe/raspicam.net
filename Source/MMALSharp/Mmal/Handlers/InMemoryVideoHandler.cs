using System;

namespace MMALSharp.Mmal.Handlers
{
    class InMemoryVideoHandler : ICaptureHandler
    {
        Action<byte[]> _onVideoDataAvailable;

        public void SetOnVideoDataAvailable(Action<byte[]> onDataAvailable)
        {
            _onVideoDataAvailable = onDataAvailable;
        }

        public void Process(ImageContext context) => _onVideoDataAvailable?.Invoke(context.Data);

        public void PostProcess() { }

        public void Dispose()
        {
            _onVideoDataAvailable = null;
        }
    }
}
