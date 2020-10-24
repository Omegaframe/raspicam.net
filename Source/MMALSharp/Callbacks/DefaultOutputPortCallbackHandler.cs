using MMALSharp.Handlers;
using MMALSharp.Ports.Outputs;

namespace MMALSharp.Callbacks
{
    class DefaultOutputPortCallbackHandler : PortCallbackHandler<IOutputPort, ICaptureHandler>
    {
        public DefaultOutputPortCallbackHandler(IOutputPort port, ICaptureHandler handler) : base(port, handler) { }
    }
}
