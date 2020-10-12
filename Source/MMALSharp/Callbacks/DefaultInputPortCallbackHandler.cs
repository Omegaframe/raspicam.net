using MMALSharp.Ports.Inputs;
using MMALSharp.Processing.Handlers;

namespace MMALSharp.Callbacks
{
    public class DefaultInputPortCallbackHandler : InputPortCallbackHandler<IInputPort, IInputCaptureHandler>
    {
        public DefaultInputPortCallbackHandler(IInputPort port, IInputCaptureHandler handler) : base(port, handler) { }
    }
}
