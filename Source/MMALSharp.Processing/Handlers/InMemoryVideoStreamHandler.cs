using MMALSharp.Common;
using System;
using System.IO;

namespace MMALSharp.Processing.Handlers
{
    public class InMemoryVideoStreamHandler : StreamCaptureHandler<Stream>, IVideoCaptureHandler
    {
        Action<byte[]> _onDataAvailable;

        public InMemoryVideoStreamHandler()
        {
            CurrentStream = new MemoryStream();
        }

        public void SetOnDataAvailable(Action<byte[]> onDataAvailable)
        {
            _onDataAvailable = onDataAvailable;
        }

        public override void Process(ImageContext context)
        {
            _onDataAvailable?.Invoke(context.Data);
        }

        public void Split() { }
    }
}
