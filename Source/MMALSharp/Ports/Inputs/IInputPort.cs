using MMALSharp.Ports.Outputs;

namespace MMALSharp.Ports.Inputs
{
    interface IInputPort : IPort
    {
        void ConnectTo(IOutputPort outputPort, IConnection connection);
        void Configure(IMmalPortConfig config, IPort copyPort);
        void Enable();
        void ReleaseBuffer(IBuffer bufferImpl);
        void Start();
    }
}
