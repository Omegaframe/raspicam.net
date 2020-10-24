using MMALSharp.Callbacks;
using MMALSharp.Components;
using MMALSharp.Handlers;
using MMALSharp.Ports.Inputs;

namespace MMALSharp.Ports.Outputs
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
