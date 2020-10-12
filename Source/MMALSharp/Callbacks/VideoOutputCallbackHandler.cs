using System;
using Microsoft.Extensions.Logging;
using MMALSharp.Common;
using MMALSharp.Common.Utility;
using MMALSharp.Config;
using MMALSharp.Extensions;
using MMALSharp.Native;
using MMALSharp.Native.Buffer;
using MMALSharp.Ports.Outputs;
using MMALSharp.Processing.Handlers;
using MMALSharp.Processing.Processors.Motion;

namespace MMALSharp.Callbacks
{
    public class VideoOutputCallbackHandler : PortCallbackHandler<IVideoPort, IVideoCaptureHandler>, IVideoOutputCallbackHandler
    {
        public Split Split { get; }

        public DateTime? LastSplit { get; private set; }

        bool PrepareSplit { get; set; }
        bool StoreMotionVectors { get; }


        public VideoOutputCallbackHandler(IVideoPort port, IVideoCaptureHandler handler, Split split, bool storeMotionVectors = false) : base(port, handler)
        {
            var motionType = WorkingPort.EncodingType == MmalEncoding.H264
                ? MotionType.MotionVector
                : MotionType.FrameDiff;

            if (handler is IMotionCaptureHandler)
            {
                var motionHandler = handler as IMotionCaptureHandler;
                motionHandler.MotionType = motionType;
            }

            Split = split;
            StoreMotionVectors = storeMotionVectors;
        }

        public override void Callback(IBuffer buffer)
        {
            if (MmalCameraConfig.Debug)
                MmalLog.Logger.LogDebug("In video output callback");

            if (PrepareSplit && buffer.AssertProperty(MmalBufferProperties.MmalBufferHeaderFlagConfig))
            {
                CaptureHandler.Split();
                LastSplit = DateTime.Now;
                PrepareSplit = false;
            }

            // Ensure that if we need to split then this is done before processing the buffer data.
            if (Split != null)
            {
                LastSplit ??= DateTime.Now;

                if (DateTime.Now.CompareTo(CalculateSplit()) > 0)
                {
                    MmalLog.Logger.LogInformation("Preparing to split.");
                    PrepareSplit = true;
                    WorkingPort.SetParameter(MmalParametersVideo.MmalParameterVideoRequestIFrame, true);
                }
            }

            if (StoreMotionVectors && buffer.AssertProperty(MmalBufferProperties.MmalBufferHeaderFlagCodecSideInfo))
            {
                // This is data containing Motion vectors. Check if the capture handler supports storing motion vectors.
                if (CaptureHandler is IMotionVectorCaptureHandler handler)
                    handler.ProcessMotionVectors(buffer.GetBufferData());
            }
            else
            {
                // If user has requested to store motion vectors separately, do not store the motion vector data in the same file
                // as image frame data.
                base.Callback(buffer);
            }
        }

        DateTime CalculateSplit()
        {
            var tempDt = new DateTime(LastSplit.Value.Ticks);

            return Split.Mode switch
            {
                TimelapseMode.Millisecond => tempDt.AddMilliseconds(Split.Value),
                TimelapseMode.Second => tempDt.AddSeconds(Split.Value),
                TimelapseMode.Minute => tempDt.AddMinutes(Split.Value),
                _ => tempDt.AddMinutes(Split.Value),
            };
        }
    }
}
