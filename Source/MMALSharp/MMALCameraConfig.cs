using MMALSharp.Components;
using MMALSharp.Config;
using MMALSharp.Native.Parameters;
using MMALSharp.Utility;

namespace MMALSharp
{
    public static class MmalCameraConfig
    {
        public static bool Debug { get; set; }
        public static SensorMode SensorMode { get; set; }
        public static int Sharpness { get; set; }
        public static int Contrast { get; set; }
        public static int Brightness { get; set; } = 50;
        public static int Saturation { get; set; }
        public static int Iso { get; set; }
        public static int ExposureCompensation { get; set; }
        public static MmalParamExposuremodeType ExposureMode { get; set; } = MmalParamExposuremodeType.MmalParamExposuremodeAuto;
        public static MmalParamExposuremeteringmodeType ExposureMeterMode { get; set; } = MmalParamExposuremeteringmodeType.MmalParamExposuremeteringmodeAverage;
        public static MmalParamAwbmodeType AwbMode { get; set; } = MmalParamAwbmodeType.MmalParamAwbmodeAuto;
        public static MmalParamImagefxType ImageFx { get; set; } = MmalParamImagefxType.MmalParamImagefxNone;
        public static ColorEffects ColorFx { get; set; }
        public static int Rotation { get; set; }
        public static MmalParamMirrorType Flips { get; set; } = MmalParamMirrorType.MmalParamMirrorNone;
        public static Zoom Roi { get; set; }
        public static int ShutterSpeed { get; set; }
        public static double AwbGainsR { get; set; }
        public static double AwbGainsB { get; set; }
        public static double AnalogGain { get; set; }
        public static double DigitalGain { get; set; }
        public static MmalParameterDrcStrengthType DrcLevel { get; set; } = MmalParameterDrcStrengthType.MmalParameterDrcStrengthOff;
        public static bool StatsPass { get; set; }
        public static AnnotateImage Annotate { get; set; }
        public static StereoMode StereoMode { get; set; } = new StereoMode();
        public static bool SetChangeEventRequest { get; set; }
        public static MmalParameterCameraConfigTimestampModeType ClockMode { get; set; } = MmalParameterCameraConfigTimestampModeType.MmalParamTimestampModeResetStc;
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
