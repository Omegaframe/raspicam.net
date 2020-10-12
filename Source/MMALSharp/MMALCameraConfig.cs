using MMALSharp.Common;
using MMALSharp.Common.Utility;
using MMALSharp.Components;
using MMALSharp.Config;
using MMALSharp.Native;

namespace MMALSharp
{
    public static class MmalCameraConfig
    {
        public static bool Debug { get; set; }
        public static MmalSensorMode SensorMode { get; set; }
        public static int Sharpness { get; set; }
        public static int Contrast { get; set; }
        public static int Brightness { get; set; } = 50;
        public static int Saturation { get; set; }
        public static int Iso { get; set; }
        public static int ExposureCompensation { get; set; }
        public static MmalParamExposuremodeT ExposureMode { get; set; } = MmalParamExposuremodeT.MmalParamExposuremodeAuto;
        public static MmalParamExposuremeteringmodeT ExposureMeterMode { get; set; } = MmalParamExposuremeteringmodeT.MmalParamExposuremeteringmodeAverage;
        public static MmalParamAwbmodeT AwbMode { get; set; } = MmalParamAwbmodeT.MmalParamAwbmodeAuto;
        public static MmalParamImagefxT ImageFx { get; set; } = MmalParamImagefxT.MmalParamImagefxNone;
        public static ColorEffects ColorFx { get; set; }
        public static int Rotation { get; set; }
        public static MmalParamMirrorT Flips { get; set; } = MmalParamMirrorT.MmalParamMirrorNone;
        public static Zoom Roi { get; set; }
        public static int ShutterSpeed { get; set; }
        public static double AwbGainsR { get; set; }
        public static double AwbGainsB { get; set; }
        public static double AnalogGain { get; set; }
        public static double DigitalGain { get; set; }
        public static MmalParameterDrcStrengthT DrcLevel { get; set; } = MmalParameterDrcStrengthT.MmalParameterDrcStrengthOff;
        public static bool StatsPass { get; set; }
        public static AnnotateImage Annotate { get; set; }
        public static StereoMode StereoMode { get; set; } = new StereoMode();
        public static bool SetChangeEventRequest { get; set; }
        public static MmalParameterCameraConfigTimestampModeT ClockMode { get; set; } = MmalParameterCameraConfigTimestampModeT.MmalParamTimestampModeResetStc;
        public static MmalEncoding Encoding { get; set; } = MmalEncoding.Opaque;
        public static MmalEncoding EncodingSubFormat { get; set; } = MmalEncoding.I420;
        public static Resolution Resolution { get; set; } = Resolution.As720p;
        public static double Framerate { get; set; } = 30;
        public static bool VideoStabilisation { get; set; } = false;
        public static MmalParametersVideo.MmalVideoRatecontrolT RateControl { get; set; } = MmalParametersVideo.MmalVideoRatecontrolT.MmalVideoRatecontrolDefault;
        public static int IntraPeriod { get; set; } = -1;
        public static MmalParametersVideo.MmalVideoProfileT VideoProfile { get; set; } = MmalParametersVideo.MmalVideoProfileT.MmalVideoProfileH264High;
        public static MmalParametersVideo.MmalVideoLevelT VideoLevel { get; set; } = MmalParametersVideo.MmalVideoLevelT.MmalVideoLevelH2644;
        public static bool ImmutableInput { get; set; } = true;
        public static bool InlineHeaders { get; set; }
        public static bool InlineMotionVectors { get; set; }
        public static MmalParametersVideo.MmalVideoIntraRefreshT IntraRefresh { get; set; } = MmalParametersVideo.MmalVideoIntraRefreshT.MmalVideoIntraRefreshDisabled;
        public static MmalEncoding VideoColorSpace { get; set; }
        public static bool StillBurstMode { get; set; }
    }
}
