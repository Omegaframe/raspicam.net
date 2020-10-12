namespace MMALSharp.Processing.Handlers
{
    public interface IFileStreamCaptureHandler : IOutputCaptureHandler
    {
        void NewFile();
        string GetFilepath();
        string GetFilename();
    }
}
