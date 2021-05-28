using Raspicam.Net.Mmal.Callbacks;
using Raspicam.Net.Mmal.Components;
using Raspicam.Net.Mmal.Handlers;
using Raspicam.Net.Mmal.Ports.Inputs;

namespace Raspicam.Net.Mmal.Ports.Outputs
{
    interface IOutputPort : IPort
    {
        void Configure(IMmalPortConfig config, IInputPort copyFrom, ICaptureHandler handler);
        IConnection ConnectTo(IDownstreamComponent component, int inputPort = 0, bool useCallback = false);
        void Start();
        void ReleaseBuffer(IBuffer bufferImpl, bool eos);
        void RegisterCallbackHandler(IOutputCallbackHandler callbackHandler);
    }
}
