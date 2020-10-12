namespace MMALSharp.Processing.Handlers
{
    public interface IInputCaptureHandler : ICaptureHandler
    {
        ProcessResult Process(uint allocSize);
    }
}
