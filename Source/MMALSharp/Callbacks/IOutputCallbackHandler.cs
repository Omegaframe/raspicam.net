namespace MMALSharp.Callbacks
{
    public interface IOutputCallbackHandler : ICallbackHandler
    {
        void Callback(IBuffer buffer);
    }
}
