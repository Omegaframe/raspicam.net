using MMALSharp.Mmal.Callbacks;
using MMALSharp.Mmal.Components;
using MMALSharp.Mmal.Handlers;
using MMALSharp.Mmal.Ports.Inputs;

namespace MMALSharp.Mmal.Ports.Outputs
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
