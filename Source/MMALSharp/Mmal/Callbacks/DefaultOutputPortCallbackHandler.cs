using MMALSharp.Mmal.Handlers;
using MMALSharp.Mmal.Ports.Outputs;

namespace MMALSharp.Mmal.Callbacks
{
    class DefaultOutputPortCallbackHandler : PortCallbackHandler<IOutputPort, ICaptureHandler>
    {
        public DefaultOutputPortCallbackHandler(IOutputPort port, ICaptureHandler handler) : base(port, handler) { }
    }
}
