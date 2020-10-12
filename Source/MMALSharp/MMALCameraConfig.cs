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
        public static MMALSensorMode SensorMode { get; set; }
        public static int Sharpness { get; set; }
        public static int Contrast { get; set; }
        public static int Brightness { get; set; } = 50;
        public static int Saturation { get; set; }
        public static int Iso { get; set; }
        public static int ExposureCompensation { get; set; }
        public static MMAL_PARAM_EXPOSUREMODE_T ExposureMode { get; set; } = MMAL_PARAM_EXPOSUREMODE_T.MMAL_PARAM_EXPOSUREMODE_AUTO;
        public static MMAL_PARAM_EXPOSUREMETERINGMODE_T ExposureMeterMode { get; set; } = MMAL_PARAM_EXPOSUREMETERINGMODE_T.MMAL_PARAM_EXPOSUREMETERINGMODE_AVERAGE;
        public static MMAL_PARAM_AWBMODE_T AwbMode { get; set; } = MMAL_PARAM_AWBMODE_T.MMAL_PARAM_AWBMODE_AUTO;
        public static MMAL_PARAM_IMAGEFX_T ImageFx { get; set; } = MMAL_PARAM_IMAGEFX_T.MMAL_PARAM_IMAGEFX_NONE;
        public static ColourEffects ColourFx { get; set; }
        public static int Rotation { get; set; }
        public static MMAL_PARAM_MIRROR_T Flips { get; set; } = MMAL_PARAM_MIRROR_T.MMAL_PARAM_MIRROR_NONE;
        public static Zoom Roi { get; set; }
        public static int ShutterSpeed { get; set; }
        public static double AwbGainsR { get; set; }
        public static double AwbGainsB { get; set; }
        public static double AnalogGain { get; set; }
        public static double DigitalGain { get; set; }
        public static MMAL_PARAMETER_DRC_STRENGTH_T DrcLevel { get; set; } = MMAL_PARAMETER_DRC_STRENGTH_T.MMAL_PARAMETER_DRC_STRENGTH_OFF;
        public static bool StatsPass { get; set; }
        public static AnnotateImage Annotate { get; set; }
        public static StereoMode StereoMode { get; set; } = new StereoMode();
        public static bool SetChangeEventRequest { get; set; }
        public static MMAL_PARAMETER_CAMERA_CONFIG_TIMESTAMP_MODE_T ClockMode { get; set; } = MMAL_PARAMETER_CAMERA_CONFIG_TIMESTAMP_MODE_T.MMAL_PARAM_TIMESTAMP_MODE_RESET_STC;
        public static MmalEncoding Encoding { get; set; } = MmalEncoding.Opaque;
        public static MmalEncoding EncodingSubFormat { get; set; } = MmalEncoding.I420;
        public static Resolution Resolution { get; set; } = Resolution.As720p;
        public static double Framerate { get; set; } = 30;
        public static bool VideoStabilisation { get; set; } = false;
        public static MMALParametersVideo.MMAL_VIDEO_RATECONTROL_T RateControl { get; set; } = MMALParametersVideo.MMAL_VIDEO_RATECONTROL_T.MMAL_VIDEO_RATECONTROL_DEFAULT;
        public static int IntraPeriod { get; set; } = -1;
        public static MMALParametersVideo.MMAL_VIDEO_PROFILE_T VideoProfile { get; set; } = MMALParametersVideo.MMAL_VIDEO_PROFILE_T.MMAL_VIDEO_PROFILE_H264_HIGH;
        public static MMALParametersVideo.MMAL_VIDEO_LEVEL_T VideoLevel { get; set; } = MMALParametersVideo.MMAL_VIDEO_LEVEL_T.MMAL_VIDEO_LEVEL_H264_4;
        public static bool ImmutableInput { get; set; } = true;
        public static bool InlineHeaders { get; set; }
        public static bool InlineMotionVectors { get; set; }
        public static MMALParametersVideo.MMAL_VIDEO_INTRA_REFRESH_T IntraRefresh { get; set; } = MMALParametersVideo.MMAL_VIDEO_INTRA_REFRESH_T.MMAL_VIDEO_INTRA_REFRESH_DISABLED;
        public static MmalEncoding VideoColorSpace { get; set; }
        public static bool StillBurstMode { get; set; }
    }
}
