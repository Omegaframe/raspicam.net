using System.IO;

namespace MMALSharp.Processing.Handlers
{
    public class MemoryStreamCaptureHandler : StreamCaptureHandler<MemoryStream>
    {
        public MemoryStreamCaptureHandler()
        {
            CurrentStream = new MemoryStream();
        }

        public MemoryStreamCaptureHandler(int size)
        {
            CurrentStream = new MemoryStream(size);
        }
    }
}
