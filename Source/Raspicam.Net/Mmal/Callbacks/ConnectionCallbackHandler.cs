namespace MMALSharp.Mmal.Callbacks
{
    abstract class ConnectionCallbackHandler : IConnectionCallbackHandler
    {
        public IConnection WorkingConnection { get; }

        protected ConnectionCallbackHandler(IConnection connection)
        {
            WorkingConnection = connection;
        }

        public virtual void InputCallback(IBuffer buffer) { }

        public virtual void OutputCallback(IBuffer buffer) { }
    }
}
