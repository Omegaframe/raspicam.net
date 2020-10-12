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
            MmalCameraConfig.AwbMode = MmalParamAwbmodeT.MmalParamAwbmodeAuto;
            MmalCameraConfig.ColorFx = default(ColorEffects);
            MmalCameraConfig.ExposureCompensation = -1;
            MmalCameraConfig.ExposureMeterMode = MmalParamExposuremeteringmodeT.MmalParamExposuremeteringmodeAverage;
            MmalCameraConfig.ExposureMode = MmalParamExposuremodeT.MmalParamExposuremodeAuto;
            MmalCameraConfig.Roi = default(Zoom);
            MmalCameraConfig.Iso = 0;
            MmalCameraConfig.StatsPass = false;
            MmalCameraConfig.Flips = MmalParamMirrorT.MmalParamMirrorNone;
            MmalCameraConfig.ImageFx = MmalParamImagefxT.MmalParamImagefxNone;
            MmalCameraConfig.Rotation = 0;
            MmalCameraConfig.DrcLevel = MmalParameterDrcStrengthT.MmalParameterDrcStrengthOff;
            MmalCameraConfig.ShutterSpeed = 0;
            MmalCameraConfig.SensorMode = MmalSensorMode.Mode0;
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