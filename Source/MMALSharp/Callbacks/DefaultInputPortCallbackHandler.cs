using MMALSharp.Handlers;
using MMALSharp.Ports.Inputs;

namespace MMALSharp.Callbacks
{
    public class DefaultInputPortCallbackHandler : InputPortCallbackHandler<IInputPort, IInputCaptureHandler>
    {
        public DefaultInputPortCallbackHandler(IInputPort port, IInputCaptureHandler handler) : base(port, handler) { }
    }
}
