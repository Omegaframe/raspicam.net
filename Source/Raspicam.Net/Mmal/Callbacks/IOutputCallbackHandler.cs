namespace Raspicam.Net.Mmal.Callbacks
{
    interface IOutputCallbackHandler : ICallbackHandler
    {
        void Callback(IBuffer buffer);
    }
}
