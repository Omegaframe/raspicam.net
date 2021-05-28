using Raspicam.Net.Mmal.Handlers;
using Raspicam.Net.Mmal.Ports;
using Raspicam.Net.Native.Buffer;

namespace Raspicam.Net.Mmal.Callbacks
{
    abstract class PortCallbackHandler<TPort, TCaptureHandler> : IOutputCallbackHandler
        where TPort : IPort
        where TCaptureHandler : ICaptureHandler
    {
        public TPort WorkingPort { get; }

        public TCaptureHandler CaptureHandler { get; }

        protected PortCallbackHandler(TPort port, TCaptureHandler handler)
        {
            WorkingPort = port;
            CaptureHandler = handler;
        }

        public virtual void Callback(IBuffer buffer)
        {
            var data = buffer.GetBufferData();
            var eos = buffer.AssertProperty(MmalBufferProperties.MmalBufferHeaderFlagFrameEnd) || buffer.AssertProperty(MmalBufferProperties.MmalBufferHeaderFlagEos);

            CaptureHandler?.Process(data);

            if (eos)
                CaptureHandler?.PostProcess();
        }
    }
}
