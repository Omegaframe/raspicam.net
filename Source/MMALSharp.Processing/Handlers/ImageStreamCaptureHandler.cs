namespace MMALSharp.Processing.Handlers
{
    public class ImageStreamCaptureHandler : FileStreamCaptureHandler
    {
        public ImageStreamCaptureHandler(string directory, string extension) : base(directory, extension) { }
        public ImageStreamCaptureHandler(string fullPath) : base(fullPath) { }
    }
}
