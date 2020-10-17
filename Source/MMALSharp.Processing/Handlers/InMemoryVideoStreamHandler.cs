using MMALSharp.Common;
using System;

namespace MMALSharp.Processing.Handlers
{
    public class InMemoryVideoStreamHandler : ICaptureHandler
    {
        Action<byte[]> _onDataAvailable;

        public void SetOnDataAvailable(Action<byte[]> onDataAvailable)
        {
            _onDataAvailable = onDataAvailable;
        }

        public void Process(ImageContext context)
        {
            _onDataAvailable?.Invoke(context.Data);
        }

        public void PostProcess() { }

        public void Dispose() { }
    }
}
