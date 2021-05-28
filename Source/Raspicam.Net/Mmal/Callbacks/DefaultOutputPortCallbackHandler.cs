using Raspicam.Net.Mmal.Handlers;
using Raspicam.Net.Mmal.Ports.Outputs;

namespace Raspicam.Net.Mmal.Callbacks
{
    class DefaultOutputPortCallbackHandler : PortCallbackHandler<IOutputPort, ICaptureHandler>
    {
        public DefaultOutputPortCallbackHandler(IOutputPort port) : base(port, null) { }
    }
}
