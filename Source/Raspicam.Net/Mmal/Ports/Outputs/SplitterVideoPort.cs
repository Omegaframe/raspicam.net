using System;
using Microsoft.Extensions.Logging;
using Raspicam.Net.Extensions;
using Raspicam.Net.Mmal.Callbacks;
using Raspicam.Net.Mmal.Components;
using Raspicam.Net.Mmal.Handlers;
using Raspicam.Net.Mmal.Ports.Inputs;
using Raspicam.Net.Native.Parameters;
using Raspicam.Net.Utility;

namespace Raspicam.Net.Mmal.Ports.Outputs
{
    unsafe class SplitterVideoPort : VideoPort
    {
        public SplitterVideoPort(IntPtr ptr, IComponent comp, Guid guid) : base(ptr, comp, guid) { }

        public override void Configure(IMmalPortConfig config, IInputPort copyFrom, ICaptureHandler handler)
        {
            CallbackHandler = new VideoOutputCallbackHandler(this, handler);

            if (config == null)
                return;

            PortConfig = config;

            copyFrom?.ShallowCopy(this);

            if (config.EncodingType != null)
                NativeEncodingType = config.EncodingType.EncodingVal;

            if (config.PixelFormat != null)
                NativeEncodingSubformat = config.PixelFormat.EncodingVal;

            var tempVid = Ptr->Format->Es->Video;

            try
            {
                Commit();
            }
            catch
            {
                // If commit fails using new settings, attempt to reset using old temp MMAL_VIDEO_FORMAT_T.
                MmalLog.Logger.LogWarning($"{Name}: Commit of output port failed. Attempting to reset values.");
                Ptr->Format->Es->Video = tempVid;
                Commit();
            }

            if (config.ZeroCopy)
            {
                ZeroCopy = true;
                this.SetParameter(MmalParametersCommon.MmalParameterZeroCopy, true);
            }

            if (CameraConfig.VideoColorSpace != null && CameraConfig.VideoColorSpace.EncType == MmalEncoding.EncodingType.ColorSpace)
                VideoColorSpace = CameraConfig.VideoColorSpace;

            if (config.Bitrate > 0)
                Bitrate = config.Bitrate;

            EncodingType = config.EncodingType;
            PixelFormat = config.PixelFormat;

            BufferNum = Math.Max(BufferNumMin, config.BufferNum > 0 ? config.BufferNum : BufferNumRecommended);
            BufferSize = Math.Max(BufferSizeMin, config.BufferSize > 0 ? config.BufferSize : BufferSizeRecommended);

            Commit();
        }
    }
}
