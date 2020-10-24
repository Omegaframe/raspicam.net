using System;
using Microsoft.Extensions.Logging;
using MMALSharp.Extensions;
using MMALSharp.Mmal.Callbacks;
using MMALSharp.Mmal.Components;
using MMALSharp.Mmal.Handlers;
using MMALSharp.Mmal.Ports.Inputs;
using MMALSharp.Native.Buffer;
using MMALSharp.Native.Parameters;
using MMALSharp.Native.Port;
using MMALSharp.Utility;

namespace MMALSharp.Mmal.Ports.Outputs
{
    unsafe class SplitterVideoPort : VideoPort
    {
        public SplitterVideoPort(IntPtr ptr, IComponent comp, Guid guid) : base(ptr, comp, guid) { }

        public SplitterVideoPort(IPort copyFrom) : base((IntPtr)copyFrom.Ptr, copyFrom.ComponentReference, copyFrom.Guid) { }

        public override void Configure(IMmalPortConfig config, IInputPort copyFrom, ICaptureHandler handler)
        {
            // The splitter component should not have its resolution set on the output port so override method accordingly.
            if (config != null)
            {
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

            CallbackHandler = new VideoOutputCallbackHandler(this, (ICaptureHandler)handler, null);
        }

        internal override void NativeOutputPortCallback(MmalPortType* port, MmalBufferHeader* buffer)
        {
            if (CameraConfig.Debug)
                MmalLog.Logger.LogDebug($"{Name}: In native {nameof(SplitterVideoPort)} output callback");

            base.NativeOutputPortCallback(port, buffer);
        }
    }
}
