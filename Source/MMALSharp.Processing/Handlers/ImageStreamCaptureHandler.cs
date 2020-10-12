using System.IO;

namespace MMALSharp.Handlers
{
    /// <summary>
    /// Processes Image data to a <see cref="FileStream"/>.
    /// </summary>
    public class ImageStreamCaptureHandler : FileStreamCaptureHandler
    {
        public ImageStreamCaptureHandler(string directory, string extension) : base(directory, extension) { }
        public ImageStreamCaptureHandler(string fullPath) : base(fullPath) { }
    }
}
