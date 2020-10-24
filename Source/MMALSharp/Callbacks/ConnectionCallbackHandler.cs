using Microsoft.Extensions.Logging;
using MMALSharp.Common.Utility;

namespace MMALSharp.Callbacks
{
    abstract class ConnectionCallbackHandler : IConnectionCallbackHandler
    {
        public IConnection WorkingConnection { get; }

        protected ConnectionCallbackHandler(IConnection connection)
        {
            WorkingConnection = connection;
        }

        public virtual void InputCallback(IBuffer buffer)
        {
            if (MmalCameraConfig.Debug)
                MmalLog.Logger.LogDebug("Inside Managed input port connection callback");
        }

        public virtual void OutputCallback(IBuffer buffer)
        {
            if (MmalCameraConfig.Debug)
                MmalLog.Logger.LogDebug("Inside Managed output port connection callback");
        }
    }
}
