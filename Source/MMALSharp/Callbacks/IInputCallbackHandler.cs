using MMALSharp.Handlers;

namespace MMALSharp.Callbacks
{
    public interface IInputCallbackHandler : ICallbackHandler
    {
        ProcessResult CallbackWithResult(IBuffer buffer);
    }
}
