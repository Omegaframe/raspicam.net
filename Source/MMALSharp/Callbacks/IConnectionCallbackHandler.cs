namespace MMALSharp.Callbacks
{
    public interface IConnectionCallbackHandler : ICallbackHandler
    {
        IConnection WorkingConnection { get; }

        void InputCallback(IBuffer buffer);

        void OutputCallback(IBuffer buffer);
    }
}