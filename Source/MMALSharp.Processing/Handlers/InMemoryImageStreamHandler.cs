using Microsoft.Extensions.Logging;
using MMALSharp.Common;
using MMALSharp.Common.Utility;
using System;
using System.IO;

namespace MMALSharp.Processing.Handlers
{
    public class InMemoryImageStreamHandler : IOutputCaptureHandler
    {
        long _processed;
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
            if (_currentStream == null || !_currentStream.CanSeek || !_currentStream.CanRead)
                return;

            var stream = new MemoryStream();

            _currentStream.Seek(0, SeekOrigin.Begin);
            _currentStream.CopyTo(stream);

            stream.Seek(0, SeekOrigin.Begin);

            _currentStream.Dispose();
            _currentStream = new MemoryStream();

            _onImageAvailable?.Invoke(stream);
        }

        public void Process(ImageContext context)
        {
            if (_currentStream == null || !_currentStream.CanSeek || !_currentStream.CanWrite)
                return;

            _currentStream.Write(context.Data);

            _processed += context.Data.Length;
        }

        public string TotalProcessed() => $"{Helpers.ConvertBytesToMegabytes(_processed)}";

        public void Dispose()
        {
            MmalLog.Logger.LogInformation($"Successfully processed {Helpers.ConvertBytesToMegabytes(_processed)}.");
            _currentStream?.Dispose();
        }
    }
}
