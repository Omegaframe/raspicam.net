using Microsoft.Extensions.Logging;
using MMALSharp.Common;
using MMALSharp.Common.Utility;
using MMALSharp.Extensions;
using MMALSharp.Handlers;
using MMALSharp.Native;
using MMALSharp.Ports;

namespace MMALSharp.Callbacks
{   
    public abstract class PortCallbackHandler<TPort, TCaptureHandler> : IOutputCallbackHandler
        where TPort : IPort
        where TCaptureHandler : IOutputCaptureHandler
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
            if (MMALCameraConfig.Debug)            
                MMALLog.Logger.LogDebug($"In managed {WorkingPort.PortType.GetPortType()} callback");            

            long? pts = null;

            var data = buffer.GetBufferData();
            var eos = buffer.AssertProperty(MMALBufferProperties.MMAL_BUFFER_HEADER_FLAG_FRAME_END) || buffer.AssertProperty(MMALBufferProperties.MMAL_BUFFER_HEADER_FLAG_EOS);
            var containsIFrame = buffer.AssertProperty(MMALBufferProperties.MMAL_BUFFER_HEADER_FLAG_CONFIG);

            if (this is IVideoOutputCallbackHandler &&
                !buffer.AssertProperty(MMALBufferProperties.MMAL_BUFFER_HEADER_FLAG_CONFIG) &&
                buffer.Pts != MMALUtil.MMAL_TIME_UNKNOWN &&
                (buffer.Pts != _ptsLastTime || !_ptsLastTime.HasValue))
            {
                if (!_ptsStartTime.HasValue)                
                    _ptsStartTime = buffer.Pts;                

                _ptsLastTime = buffer.Pts;
                pts = buffer.Pts - _ptsStartTime.Value;
            }

            if (MMALCameraConfig.Debug)            
                MMALLog.Logger.LogDebug("Attempting to process data.");            
            
            CaptureHandler?.Process(new ImageContext
            {
                Data = data,
                Eos = eos,
                IFrame = containsIFrame,
                Resolution = WorkingPort.Resolution,
                Encoding = WorkingPort.EncodingType,
                PixelFormat = WorkingPort.PixelFormat,
                Raw = WorkingPort.EncodingType.EncType == MMALEncoding.EncodingType.PixelFormat,
                Pts = pts,
                Stride = MMALUtil.mmal_encoding_width_to_stride(WorkingPort.PixelFormat?.EncodingVal ?? WorkingPort.EncodingType.EncodingVal, WorkingPort.Resolution.Width)
            });

            if (eos)
            {
                // Once we have a full frame, perform any post processing as required.
                CaptureHandler?.PostProcess();
            }
        }
    }
}
