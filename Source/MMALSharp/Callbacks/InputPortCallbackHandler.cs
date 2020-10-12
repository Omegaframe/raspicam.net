using Microsoft.Extensions.Logging;
using MMALSharp.Common.Utility;
using MMALSharp.Handlers;
using MMALSharp.Ports.Inputs;

namespace MMALSharp.Callbacks
{
    public abstract class InputPortCallbackHandler<TPort, TCaptureHandler> : IInputCallbackHandler
        where TPort : IInputPort
        where TCaptureHandler : IInputCaptureHandler
    {
        public TPort WorkingPort { get; }

        public TCaptureHandler CaptureHandler { get; }

        protected InputPortCallbackHandler(TPort port, TCaptureHandler handler)
        {
            WorkingPort = port;
            CaptureHandler = handler;
        }

        public virtual ProcessResult CallbackWithResult(IBuffer buffer)
        {
            if (MMALCameraConfig.Debug)
                MMALLog.Logger.LogDebug($"In managed {this.WorkingPort.PortType.GetPortType()} callback");

            MMALLog.Logger.LogInformation($"Feeding: {Helpers.ConvertBytesToMegabytes(buffer.AllocSize)}. Total processed: {this.CaptureHandler?.TotalProcessed()}.");

            return CaptureHandler?.Process(buffer.AllocSize);
        }
    }
}
