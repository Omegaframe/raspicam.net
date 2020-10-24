namespace MMALSharp.Callbacks
{
    interface IOutputCallbackHandler : ICallbackHandler
    {
        void Callback(IBuffer buffer);
    }
}
