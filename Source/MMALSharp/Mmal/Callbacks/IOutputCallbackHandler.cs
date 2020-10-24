namespace MMALSharp.Mmal.Callbacks
{
    interface IOutputCallbackHandler : ICallbackHandler
    {
        void Callback(IBuffer buffer);
    }
}
