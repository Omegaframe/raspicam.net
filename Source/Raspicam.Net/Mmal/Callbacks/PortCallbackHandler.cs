using Raspicam.Net.Mmal.Handlers;
using Raspicam.Net.Mmal.Ports;
using Raspicam.Net.Native.Buffer;
using Raspicam.Net.Native.Util;

namespace Raspicam.Net.Mmal.Callbacks
{
    abstract class PortCallbackHandler<TPort, TCaptureHandler> : IOutputCallbackHandler
        where TPort : IPort
        where TCaptureHandler : ICaptureHandler
    {
        public TPort WorkingPort { get; }

        public TCaptureHandler CaptureHandler { get; }

        long? _ptsStartTime;
        long? _ptsLastTime;

        protected PortCallbackHandler(TPort port, TCaptureHandler handler)
        {
            WorkingPort = port;
            CaptureHandler = handler;
        }

        public virtual void Callback(IBuffer buffer)
        {
            long? pts = null;

            var data = buffer.GetBufferData();
            var eos = buffer.AssertProperty(MmalBufferProperties.MmalBufferHeaderFlagFrameEnd) || buffer.AssertProperty(MmalBufferProperties.MmalBufferHeaderFlagEos);
            var containsIFrame = buffer.AssertProperty(MmalBufferProperties.MmalBufferHeaderFlagConfig);

            if (this is IVideoOutputCallbackHandler &&
                !buffer.AssertProperty(MmalBufferProperties.MmalBufferHeaderFlagConfig) &&
                buffer.Pts != MmalUtil.MmalTimeUnknown &&
                buffer.Pts != _ptsLastTime)
            {
                _ptsStartTime ??= buffer.Pts;
                _ptsLastTime = buffer.Pts;
                pts = buffer.Pts - _ptsStartTime.Value;
            }

            CaptureHandler?.Process(new ImageContext
            {
                Data = data,
                IsEos = eos,
                IsIFrame = containsIFrame,
                Resolution = WorkingPort.Resolution,
                Encoding = WorkingPort.EncodingType,
                PixelFormat = WorkingPort.PixelFormat,
                IsRaw = WorkingPort.EncodingType.EncType == MmalEncoding.EncodingType.PixelFormat,
                Pts = pts,
                Stride = MmalUtil.EncodingWidthToStride(WorkingPort.PixelFormat?.EncodingVal ?? WorkingPort.EncodingType.EncodingVal, WorkingPort.Resolution.Width)
            });

            if (eos)
                CaptureHandler?.PostProcess();
        }
    }
}
