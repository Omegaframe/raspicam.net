using MMALSharp.Ports.Outputs;
using MMALSharp.Processing.Handlers;

namespace MMALSharp.Callbacks
{
    public class DefaultOutputPortCallbackHandler : PortCallbackHandler<IOutputPort, ICaptureHandler>
    {
        public DefaultOutputPortCallbackHandler(IOutputPort port, ICaptureHandler handler) : base(port, handler) { }
    }
}
