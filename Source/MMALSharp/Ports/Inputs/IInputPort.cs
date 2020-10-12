using MMALSharp.Callbacks;
using MMALSharp.Ports.Outputs;
using MMALSharp.Processing.Handlers;

namespace MMALSharp.Ports.Inputs
{
    public interface IInputPort : IPort
    {
        void ConnectTo(IOutputPort outputPort, IConnection connection);
        void Configure(IMmalPortConfig config, IPort copyPort, IInputCaptureHandler handler);
        void Enable();
        void ReleaseBuffer(IBuffer bufferImpl);
        void Start();
        void RegisterCallbackHandler(IInputCallbackHandler callbackHandler);
    }
}
