using Microsoft.Extensions.Logging;
using MMALSharp.Common;
using MMALSharp.Common.Utility;
using MMALSharp.Extensions;
using MMALSharp.Handlers;
using MMALSharp.Native.Buffer;
using MMALSharp.Native.Util;
using MMALSharp.Ports;

namespace MMALSharp.Callbacks
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
            if (MmalCameraConfig.Debug)            
                MmalLog.Logger.LogDebug($"In managed {WorkingPort.PortType.GetPortType()} callback");            

            long? pts = null;

            var data = buffer.GetBufferData();
            var eos = buffer.AssertProperty(MmalBufferProperties.MmalBufferHeaderFlagFrameEnd) || buffer.AssertProperty(MmalBufferProperties.MmalBufferHeaderFlagEos);
            var containsIFrame = buffer.AssertProperty(MmalBufferProperties.MmalBufferHeaderFlagConfig);

            if (this is IVideoOutputCallbackHandler &&
                !buffer.AssertProperty(MmalBufferProperties.MmalBufferHeaderFlagConfig) &&
                buffer.Pts != MmalUtil.MmalTimeUnknown &&
                (buffer.Pts != _ptsLastTime || !_ptsLastTime.HasValue))
            {
                _ptsStartTime ??= buffer.Pts;    
                _ptsLastTime = buffer.Pts;
                pts = buffer.Pts - _ptsStartTime.Value;
            }

            if (MmalCameraConfig.Debug)            
                MmalLog.Logger.LogDebug("Attempting to process data.");            
            
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
            {
                // Once we have a full frame, perform any post processing as required.
                CaptureHandler?.PostProcess();
            }
        }
    }
}
