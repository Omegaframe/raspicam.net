using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using MMALSharp.Common;
using MMALSharp.Common.Utility;
using MMALSharp.Extensions;
using MMALSharp.Handlers;
using MMALSharp.Native.Parameters;
using MMALSharp.Native.Port;
using MMALSharp.Native.Util;
using MMALSharp.Ports;
using MMALSharp.Ports.Inputs;
using MMALSharp.Ports.Outputs;
using static MMALSharp.MmalNativeExceptionHelper;

namespace MMALSharp.Components.EncoderComponents
{
    unsafe class MmalVideoEncoder : MmalEncoderBase, IVideoEncoder
    {
        public const int MaxBitrateMJPEG = 25000000;
        public const int MaxBitrateLevel4 = 25000000; // 25Mbits/s
        public const int MaxBitrateLevel42 = 62500000; // 62.5Mbits/s

        int Quality { get; set; }

        public MmalVideoEncoder() : base(MmalParameters.MmalComponentDefaultVideoEncoder)
        {
            Inputs.Add(new InputPort((IntPtr)(&(*Ptr->Input[0])), this, Guid.NewGuid()));
            Outputs.Add(new VideoPort((IntPtr)(&(*Ptr->Output[0])), this, Guid.NewGuid()));
        }

        public override IDownstreamComponent ConfigureOutputPort(int outputPort, IMmalPortConfig config, ICaptureHandler handler)
        {
            Quality = config.Quality;

            int bufferSize;

            if (config.EncodingType == MmalEncoding.H264)
                bufferSize = Math.Max(Outputs[outputPort].Ptr->BufferSizeRecommended, Outputs[outputPort].Ptr->BufferSizeMin);
            else
                bufferSize = Math.Max(Outputs[outputPort].Ptr->BufferSizeRecommended, 256 << 10);

            var bitrate = GetValidBitrate(outputPort, config);

            // Force framerate to be 0 in case it was provided by user.
            config = new MmalPortConfig(
                config.EncodingType,
                config.PixelFormat,
                config.Quality,
                bitrate,
                config.Timeout,
                config.Split,
                config.StoreMotionVectors,
                config.Width,
                config.Height,
                config.Framerate,
                config.ZeroCopy,
                config.BufferNum,
                bufferSize,
                config.Crop);

            base.ConfigureOutputPort(outputPort, config, handler);

            if (Outputs[outputPort].EncodingType == MmalEncoding.H264)
            {
                ConfigureIntraPeriod(outputPort);

                ConfigureVideoProfile(outputPort);

                ConfigureInlineHeaderFlag(outputPort);

                ConfigureInlineVectorsFlag(outputPort);

                ConfigureIntraRefresh(outputPort);

                ConfigureQuantisationParameter(outputPort);
            }

            ConfigureImmutableInput(outputPort);

            return this;
        }

        public void RequestIFrame()
        {
            if (Outputs[0].EncodingType != MmalEncoding.H264)
            {
                MmalLog.Logger.LogWarning("Output port encoding type not set to H.264. This method has no effect.");
                return;
            }

            Outputs[0].SetParameter(MmalParametersVideo.MmalParameterVideoRequestIFrame, true);
        }

        int GetValidBitrate(int outputPort, IMmalPortConfig config)
        {
            var bitrate = config.Bitrate;

            if (Outputs[outputPort].EncodingType == MmalEncoding.H264)
            {
                List<VideoLevel> levelList;

                if (MmalCameraConfig.VideoProfile == MmalParametersVideo.MmalVideoProfileT.MmalVideoProfileH264High)
                    levelList = GetHighLevelLimits();
                else if (MmalCameraConfig.VideoProfile == MmalParametersVideo.MmalVideoProfileT.MmalVideoProfileH264High10)
                    levelList = GetHigh10LevelLimits();
                else
                    levelList = GetNormalLevelLimits();

                var level = levelList.First(c => c.Level == MmalCameraConfig.VideoLevel);

                if (config.Bitrate > level.Maxbitrate)
                    throw new PiCameraError("Bitrate requested exceeds maximum for selected Video Level and Profile");
            }
            else if (Outputs[outputPort].EncodingType == MmalEncoding.MJpeg && Outputs[outputPort].Bitrate > MaxBitrateMJPEG)
            {
                MmalLog.Logger.LogWarning("Bitrate too high: Reducing to 25MBit/s");
                bitrate = MaxBitrateMJPEG;
            }

            return bitrate;
        }

        void ConfigureRateControl(int outputPort)
        {
            var param = new MmalParameterVideoRateControlType(new MmalParameterHeaderType(MmalParametersVideo.MmalParameterRatecontrol, Marshal.SizeOf<MmalParameterVideoRateControlType>()), MmalCameraConfig.RateControl);
            MmalCheck(MmalPort.SetParameter(Outputs[outputPort].Ptr, param.HdrPtr), "Unable to set ratecontrol.");
        }

        void ConfigureIntraPeriod(int outputPort)
        {
            if (Outputs[outputPort].EncodingType == MmalEncoding.H264 && MmalCameraConfig.IntraPeriod != -1)
                Outputs[outputPort].SetParameter(MmalParametersVideo.MmalParameterIntraperiod, MmalCameraConfig.IntraPeriod);
        }

        void ConfigureQuantisationParameter(int outputPort)
        {
            Outputs[outputPort].SetParameter(MmalParametersVideo.MmalParameterVideoEncodeInitialQuant, Quality);
            Outputs[outputPort].SetParameter(MmalParametersVideo.MmalParameterVideoEncodeMinQuant, Quality);
            Outputs[outputPort].SetParameter(MmalParametersVideo.MmalParameterVideoEncodeMaxQuant, Quality);
        }

        void ConfigureVideoProfile(int outputPort)
        {
            var rational = new MmalRational(MmalCameraConfig.Framerate);
            var macroblocks = (MmalCameraConfig.Resolution.Width >> 4) * (MmalCameraConfig.Resolution.Height >> 4);
            var macroblocksPSec = macroblocks * (rational.Num / rational.Den);

            var videoLevels = GetNormalLevelLimits();

            var level = videoLevels.Where(c => c.Level == MmalCameraConfig.VideoLevel).First();

            if (macroblocks > level.MacroblocksLimit)
                throw new PiCameraError("Resolution exceeds macroblock limit for selected profile and level.");

            if (macroblocksPSec > level.MacroblocksPerSecLimit)
                throw new PiCameraError("Resolution exceeds macroblocks/s limit for selected profile and level.");

            var p = new MmalParameterVideoProfileS(MmalCameraConfig.VideoProfile, MmalCameraConfig.VideoLevel);

            var arr = new MmalParameterVideoProfileS[1] { p };

            var param = new MmalParameterVideoProfileType(new MmalParameterHeaderType(MmalParametersVideo.MmalParameterProfile, Marshal.SizeOf<MmalParameterVideoProfileType>()), arr);

            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(param));

            Marshal.StructureToPtr(param, ptr, false);

            try
            {
                MmalCheck(
                    MmalPort.SetParameter(Outputs[outputPort].Ptr, (MmalParameterHeaderType*)ptr),
                    "Unable to set video profile.");
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        void ConfigureImmutableInput(int _)
        {
            Inputs[0].SetParameter(MmalParametersVideo.MmalParameterVideoImmutableInput, MmalCameraConfig.ImmutableInput);
        }

        void ConfigureInlineHeaderFlag(int outputPort)
        {
            Outputs[outputPort].SetParameter(MmalParametersVideo.MmalParameterVideoEncodeInlineHeader, MmalCameraConfig.InlineHeaders);
        }

        void ConfigureInlineVectorsFlag(int outputPort)
        {
            Outputs[outputPort].SetParameter(MmalParametersVideo.MmalParameterVideoEncodeInlineVectors, MmalCameraConfig.InlineMotionVectors);
        }

        void ConfigureIntraRefresh(int outputPort)
        {
            var param = new MmalParameterVideoIntraRefreshType(new MmalParameterHeaderType(MmalParametersVideo.MmalParameterVideoIntraRefresh, Marshal.SizeOf<MmalParameterVideoIntraRefreshType>()), MmalParametersVideo.MmalVideoIntraRefreshT.MmalVideoIntraRefreshBoth, 0, 0, 0, 0);

            int airMbs = 0, airRef = 0, cirMbs = 0, pirMbs = 0;

            try
            {
                MmalCheck(MmalPort.GetParameter(Outputs[outputPort].Ptr, param.HdrPtr), "Unable to set video profile.");
                airMbs = param.AirMbs;
                airRef = param.AirRef;
                cirMbs = param.CirMbs;
                pirMbs = param.PirMbs;
            }
            catch
            {
                /* catch all */
            }

            param = new MmalParameterVideoIntraRefreshType(new MmalParameterHeaderType(MmalParametersVideo.MmalParameterVideoIntraRefresh, Marshal.SizeOf<MmalParameterVideoIntraRefreshType>()), MmalCameraConfig.IntraRefresh, airMbs, airRef, cirMbs, pirMbs);

            MmalCheck(MmalPort.SetParameter(Outputs[outputPort].Ptr, param.HdrPtr), "Unable to set video intra refresh.");
        }

        static List<VideoLevel> GetNormalLevelLimits()
        {
            var videoLevels = new List<VideoLevel>
            {
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH2641, 1485, 99, 64000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH2641B, 1485, 99, 128000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26411, 3000, 396, 192000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26412, 6000, 396, 384000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26413, 11880, 396, 768000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH2642, 11880, 396, 2000000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26421, 19800, 792, 4000000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26422, 20250, 1620, 4000000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH2643, 40500, 1620, 10000000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26431, 108000, 3600, 14000000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26432, 216000, 5120, 20000000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH2644, 245760, 8192, 20000000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26441, 245760, 8192, 50000000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26442, 522240, 8704, 50000000)
            };

            return videoLevels;
        }

        static List<VideoLevel> GetHighLevelLimits()
        {
            var videoLevels = new List<VideoLevel>
            {
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH2641, 1485, 99, 80000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH2641B, 1485, 99, 160000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26411, 3000, 396, 240000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26412, 6000, 396, 480000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26413, 11880, 396, 960000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH2642, 11880, 396, 2500000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26421, 19800, 792, 5000000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26422, 20250, 1620, 5000000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH2643, 40500, 1620, 12500000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26431, 108000, 3600, 17500000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26432, 216000, 5120, 25000000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH2644, 245760, 8192, 25000000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26441, 245760, 8192, 62500000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26442, 522240, 8704, 62500000)
            };

            return videoLevels;
        }

        static List<VideoLevel> GetHigh10LevelLimits()
        {
            var videoLevels = new List<VideoLevel>
            {
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH2641, 1485, 99, 192000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH2641B, 1485, 99, 384000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26411, 3000, 396, 576000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26412, 6000, 396, 1152000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26413, 11880, 396, 2304000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH2642, 11880, 396, 6000000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26421, 19800, 792, 12000000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26422, 20250, 1620, 12000000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH2643, 40500, 1620, 30000000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26431, 108000, 3600, 42000000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26432, 216000, 5120, 60000000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH2644, 245760, 8192, 60000000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26441, 245760, 8192, 150000000),
                new VideoLevel(MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH26442, 522240, 8704, 150000000)
            };

            return videoLevels;
        }
    }

    public class VideoLevel
    {
        public MmalParametersVideo.MmalVideoLevelT Level { get; set; }
        public int MacroblocksPerSecLimit { get; set; }
        public int MacroblocksLimit { get; set; }
        public int Maxbitrate { get; set; }

        public VideoLevel(MmalParametersVideo.MmalVideoLevelT level, int mcbps, int mcb, int bitrate)
        {
            Level = level;
            MacroblocksPerSecLimit = mcbps;
            MacroblocksLimit = mcb;
            Maxbitrate = bitrate;
        }
    }
}
