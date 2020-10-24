using System;
using System.IO;
using MMALSharp.Common;

namespace MMALSharp.Handlers
{
    class InMemoryHandler : ICaptureHandler
    {
        MemoryStream _currentStream;
        Action<Stream> _onFullFrameAvailable;
        Action<byte[]> _onVideoDataAvailable;

        public InMemoryHandler()
        {
            _currentStream = new MemoryStream();
        }

        public void SetOnFullFrameAvailable(Action<Stream> onDataAvailable)
        {
            _onFullFrameAvailable = onDataAvailable;
        }

        public void SetOnVideoDataAvailable(Action<byte[]> onDataAvailable)
        {
            _onVideoDataAvailable = onDataAvailable;
        }

        public void PostProcess()
        {
            if (_currentStream == null || _onFullFrameAvailable == null)
                return;

            var stream = new MemoryStream();

            _currentStream.Seek(0, SeekOrigin.Begin);
            _currentStream.CopyTo(stream);

            stream.Seek(0, SeekOrigin.Begin);

            _currentStream.Dispose();
            _currentStream = new MemoryStream();

            _onFullFrameAvailable?.Invoke(stream);
        }

        public void Process(ImageContext context)
        {
            _currentStream?.Write(context.Data);
            _onVideoDataAvailable?.Invoke(context.Data);
        }

        public void Dispose()
        {
            _onFullFrameAvailable = null;
            _onVideoDataAvailable = null;
            _currentStream?.Dispose();
            _currentStream = null;
        }
    }
}
