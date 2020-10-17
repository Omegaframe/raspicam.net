using MMALSharp.Callbacks;
using MMALSharp.Components;
using MMALSharp.Ports.Inputs;
using MMALSharp.Processing.Handlers;

namespace MMALSharp.Ports.Outputs
{
    public interface IOutputPort : IPort
    {
        void Configure(IMmalPortConfig config, IInputPort copyFrom, ICaptureHandler handler);
        IConnection ConnectTo(IDownstreamComponent component, int inputPort = 0, bool useCallback = false);
        void Start();
        void ReleaseBuffer(IBuffer bufferImpl, bool eos);
        void RegisterCallbackHandler(IOutputCallbackHandler callbackHandler);
    }
}
