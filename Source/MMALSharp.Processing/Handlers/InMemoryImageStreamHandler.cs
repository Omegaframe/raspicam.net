using MMALSharp.Common;
using System;
using System.IO;

namespace MMALSharp.Processing.Handlers
{
    public class InMemoryImageStreamHandler : ICaptureHandler
    {
        MemoryStream _currentStream;
        Action<Stream> _onImageAvailable;

        public InMemoryImageStreamHandler()
        {
            _currentStream = new MemoryStream();
        }

        public void SetOnImageAvailable(Action<Stream> onDataAvailable)
        {
            _onImageAvailable = onDataAvailable;
        }

        public void PostProcess()
        {
            if (_currentStream == null || _onImageAvailable == null)
                return;

            var stream = new MemoryStream();

            _currentStream.Seek(0, SeekOrigin.Begin);
            _currentStream.CopyTo(stream);

            stream.Seek(0, SeekOrigin.Begin);

            _currentStream.Dispose();
            _currentStream = new MemoryStream();

            _onImageAvailable?.Invoke(stream);
        }

        public void Process(ImageContext context) => _currentStream?.Write(context.Data);

        public void Dispose()
        {
            _onImageAvailable = null;
            _currentStream?.Dispose();
            _currentStream = null;
        }
    }
}
