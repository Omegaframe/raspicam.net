using System.IO;
using Microsoft.Extensions.Logging;
using MMALSharp.Common;
using MMALSharp.Common.Utility;
using MMALSharp.Components;
using MMALSharp.Config;
using MMALSharp.Native;

namespace MMALSharp.Tests
{
    public class TestHelper
    {
        public static void SetConfigurationDefaults()
        {
            MmalCameraConfig.Debug = true;
            MmalCameraConfig.Brightness = 70;
            MmalCameraConfig.Sharpness = 60;
            MmalCameraConfig.Contrast = 60;
            MmalCameraConfig.Saturation = 50;
            MmalCameraConfig.AwbGainsB = 0;
            MmalCameraConfig.AwbGainsR = 0;
            MmalCameraConfig.AwbMode = MMAL_PARAM_AWBMODE_T.MMAL_PARAM_AWBMODE_AUTO;
            MmalCameraConfig.ColourFx = default(ColourEffects);
            MmalCameraConfig.ExposureCompensation = -1;
            MmalCameraConfig.ExposureMeterMode = MMAL_PARAM_EXPOSUREMETERINGMODE_T.MMAL_PARAM_EXPOSUREMETERINGMODE_AVERAGE;
            MmalCameraConfig.ExposureMode = MMAL_PARAM_EXPOSUREMODE_T.MMAL_PARAM_EXPOSUREMODE_AUTO;
            MmalCameraConfig.Roi = default(Zoom);
            MmalCameraConfig.Iso = 0;
            MmalCameraConfig.StatsPass = false;
            MmalCameraConfig.Flips = MMAL_PARAM_MIRROR_T.MMAL_PARAM_MIRROR_NONE;
            MmalCameraConfig.ImageFx = MMAL_PARAM_IMAGEFX_T.MMAL_PARAM_IMAGEFX_NONE;
            MmalCameraConfig.Rotation = 0;
            MmalCameraConfig.DrcLevel = MMAL_PARAMETER_DRC_STRENGTH_T.MMAL_PARAMETER_DRC_STRENGTH_OFF;
            MmalCameraConfig.ShutterSpeed = 0;
            MmalCameraConfig.SensorMode = MMALSensorMode.Mode0;
            MmalCameraConfig.VideoStabilisation = true;
            MmalCameraConfig.Framerate = 10;
            MmalCameraConfig.Encoding = MmalEncoding.Opaque;
            MmalCameraConfig.EncodingSubFormat = MmalEncoding.I420;
            MmalCameraConfig.VideoColorSpace = MmalEncoding.MmalColorSpaceIturBt709;
            MmalCameraConfig.InlineMotionVectors = false;
            MmalCameraConfig.Resolution = Resolution.As03MPixel;
            MmalCameraConfig.AnalogGain = 0;
            MmalCameraConfig.DigitalGain = 0;
            MmalCameraConfig.Annotate = null;
        }

        public static void CleanDirectory(string directory)
        {
            try
            {
                var files = Directory.GetFiles(directory);

                // Clear directory first
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
            catch
            {
            }
        }
        
        public static void BeginTest(string name) => MmalLog.Logger.LogInformation($"Running test: {name}.");
        
        public static void BeginTest(string name, string encodingType, string pixelFormat)
            => MmalLog.Logger.LogInformation($"Running test: {name}. Encoding type: {encodingType}. Pixel format: {pixelFormat}.");
    }
}