using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.Logging;
using MMALSharp.Common.Utility;
using MMALSharp.Components;
using MMALSharp.Components.EncoderComponents;
using MMALSharp.Config;
using MMALSharp.Native;
using MMALSharp.Ports.Controls;
using MMALSharp.Utility;
using static MMALSharp.MmalNativeExceptionHelper;
using static MMALSharp.Native.MmalParametersCamera;

namespace MMALSharp.Extensions
{
    public static unsafe class MmalCameraComponentExtensions
    {
        internal static void SetCameraConfig(this MmalCameraComponent camera, MMAL_PARAMETER_CAMERA_CONFIG_T value)
        {
            MmalCheck(MmalPort.mmal_port_parameter_set(camera.Control.Ptr, &value.Hdr), "Unable to set camera config.");
        }

        internal static void SetChangeEventRequest(this IControlPort controlPort, MMAL_PARAMETER_CHANGE_EVENT_REQUEST_T value)
        {
            MmalCheck(MmalPort.mmal_port_parameter_set(controlPort.Ptr, &value.Hdr), "Unable to set camera event request.");
        }

        public static bool GetIsEnabledAnnotateSettings(this MmalCameraComponent camera)
        {
            var annotate = new MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterAnnotate, Marshal.SizeOf<MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T>()),
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null);

            MmalCheck(MmalPort.mmal_port_parameter_get(camera.Control.Ptr, &annotate.Hdr), "Unable to get camera annotate settings");

            return annotate.Enable == 1;
        }

        public static bool GetShowShutterAnnotateSettings(this MmalCameraComponent camera)
        {
            var annotate = new MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterAnnotate, Marshal.SizeOf<MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T>()),
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null);

            MmalCheck(MmalPort.mmal_port_parameter_get(camera.Control.Ptr, &annotate.Hdr), "Unable to get camera annotate settings");

            return annotate.ShowShutter == 1;
        }

        public static bool GetShowCafAnnotateSettings(this MmalCameraComponent camera)
        {
            var annotate = new MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterAnnotate, Marshal.SizeOf<MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T>()),
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null);

            MmalCheck(MmalPort.mmal_port_parameter_get(camera.Control.Ptr, &annotate.Hdr), "Unable to get camera annotate settings");

            return annotate.ShowCaf == 1;
        }

        public static bool GetShowGainAnnotateSettings(this MmalCameraComponent camera)
        {
            var annotate = new MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterAnnotate, Marshal.SizeOf<MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T>()),
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null);

            MmalCheck(MmalPort.mmal_port_parameter_get(camera.Control.Ptr, &annotate.Hdr), "Unable to get camera annotate settings");

            return annotate.ShowAnalogGain == 1;
        }

        public static bool GetShowLensAnnotateSettings(this MmalCameraComponent camera)
        {
            var annotate = new MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterAnnotate, Marshal.SizeOf<MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T>()),
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null);

            MmalCheck(MmalPort.mmal_port_parameter_get(camera.Control.Ptr, &annotate.Hdr), "Unable to get camera annotate settings");

            return annotate.ShowLens == 1;
        }

        public static bool GetShowMotionAnnotateSettings(this MmalCameraComponent camera)
        {
            var annotate = new MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterAnnotate, Marshal.SizeOf<MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T>()),
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null);

            MmalCheck(MmalPort.mmal_port_parameter_get(camera.Control.Ptr, &annotate.Hdr), "Unable to get camera annotate settings");

            return annotate.ShowMotion == 1;
        }

        public static bool GetShowFrameNumberAnnotateSettings(this MmalCameraComponent camera)
        {
            var annotate = new MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterAnnotate, Marshal.SizeOf<MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T>()),
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null);

            MmalCheck(MmalPort.mmal_port_parameter_get(camera.Control.Ptr, &annotate.Hdr), "Unable to get camera annotate settings");

            return annotate.ShowFrameNum == 1;
        }

        public static bool GetShowBlackBackgroundAnnotateSettings(this MmalCameraComponent camera)
        {
            var annotate = new MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterAnnotate, Marshal.SizeOf<MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T>()),
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null);

            MmalCheck(MmalPort.mmal_port_parameter_get(camera.Control.Ptr, &annotate.Hdr), "Unable to get camera annotate settings");

            return annotate.EnableTextBackground == 1;
        }

        public static string GetCustomTextAnnotateSettings(this MmalCameraComponent camera)
        {
            var annotate = new MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterAnnotate, Marshal.SizeOf<MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T>()),
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null);

            MmalCheck(MmalPort.mmal_port_parameter_get(camera.Control.Ptr, &annotate.Hdr), "Unable to get camera annotate settings");

            return Encoding.ASCII.GetString(annotate.Text);
        }

        public static Color GetTextColourAnnotateSettings(this MmalCameraComponent camera)
        {
            var annotate = new MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterAnnotate, Marshal.SizeOf<MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T>()),
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null);

            MmalCheck(MmalPort.mmal_port_parameter_get(camera.Control.Ptr, &annotate.Hdr), "Unable to get camera annotate settings");

            return MmalColor.FromYuvBytes(annotate.CustomTextY, annotate.CustomTextU, annotate.CustomTextV);
        }

        public static Color GetBackgroundColourAnnotateSettings(this MmalCameraComponent camera)
        {
            var annotate = new MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterAnnotate, Marshal.SizeOf<MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T>()),
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null);

            MmalCheck(MmalPort.mmal_port_parameter_get(camera.Control.Ptr, &annotate.Hdr), "Unable to get camera annotate settings");

            return MmalColor.FromYuvBytes(annotate.CustomBackgroundY, annotate.CustomBackgroundU, annotate.CustomBackgroundV);
        }

        internal static void SetAnnotateSettings(this MmalCameraComponent camera)
        {
            if (MmalCameraConfig.Annotate == null)
                return;

            MmalLog.Logger.LogDebug("Setting annotate");

            var sb = new StringBuilder();

            var showShutter = 0;
            var showAnalogGain = 0;
            var showLens = 0;
            var showCaf = 0;
            var showMotion = 0;
            var showFrame = 0;
            var enableTextBackground = 0;
            var customTextColor = 0;
            var customTextY = (byte)0;
            var customTextU = (byte)0;
            var customTextV = (byte)0;
            var customBackgroundColor = 0;
            var customBackgroundY = (byte)0;
            var customBackgroundU = (byte)0;
            var customBackgroundV = (byte)0;
            var justify = MmalCameraConfig.Annotate.Justify;
            var xOffset = MmalCameraConfig.Annotate.XOffset;
            var yOffset = MmalCameraConfig.Annotate.YOffset;

            if (!string.IsNullOrEmpty(MmalCameraConfig.Annotate.CustomText))
                sb.Append(MmalCameraConfig.Annotate.CustomText + " ");

            if (MmalCameraConfig.Annotate.ShowTimeText)
                sb.Append(DateTime.Now.ToString(MmalCameraConfig.Annotate.TimeFormat) + " ");

            if (MmalCameraConfig.Annotate.ShowDateText)
                sb.Append(DateTime.Now.ToString(MmalCameraConfig.Annotate.DateFormat) + " ");

            if (MmalCameraConfig.Annotate.ShowShutterSettings)
                showShutter = 1;

            if (MmalCameraConfig.Annotate.ShowGainSettings)
                showAnalogGain = 1;

            if (MmalCameraConfig.Annotate.ShowLensSettings)
                showLens = 1;

            if (MmalCameraConfig.Annotate.ShowCafSettings)
                showCaf = 1;

            if (MmalCameraConfig.Annotate.ShowMotionSettings)
                showMotion = 1;

            if (MmalCameraConfig.Annotate.ShowFrameNumber)
                showFrame = 1;

            if (MmalCameraConfig.Annotate.AllowCustomBackgroundColour)
                enableTextBackground = 1;

            var textSize = Convert.ToByte(MmalCameraConfig.Annotate.TextSize);

            if (MmalCameraConfig.Annotate.TextColour != Color.Empty)
            {
                customTextColor = 1;

                var (y, u, v) = MmalColor.RgbToYuvBytes(MmalCameraConfig.Annotate.TextColour);
                customTextY = y;
                customTextU = u;
                customTextV = v;
            }

            if (MmalCameraConfig.Annotate.BgColour != Color.Empty)
            {
                customBackgroundColor = 1;
                var (y, u, v) = MmalColor.RgbToYuvBytes(MmalCameraConfig.Annotate.BgColour);
                customBackgroundY = y;
                customBackgroundU = u;
                customBackgroundV = v;
            }

            // .NET Core has an issue with marshalling arrays "ByValArray". The array being passed MUST equal the size
            // specified in the "SizeConst" field or you will receive an exception. Mono does not have this restriction
            // and is quite happy to pass an array of a lower size if asked. In order to get around this, I am creating
            // an array equaling "SizeConst" and copying the contents of the annotation text into it, minus the EOL character.
            var text = sb.ToString() + char.MinValue;
            var arr = new byte[MmalParametersCamera.MmalCameraAnnotateMaxTextLenV3];
            var bytes = Encoding.ASCII.GetBytes(text);

            Array.Copy(bytes, arr, bytes.Length);

            var strV4 = new MMAL_PARAMETER_CAMERA_ANNOTATE_V4_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterAnnotate, Marshal.SizeOf<MMAL_PARAMETER_CAMERA_ANNOTATE_V4_T>() + (arr.Length - 1)),
                1, showShutter, showAnalogGain, showLens,
                showCaf, showMotion, showFrame, enableTextBackground,
                customBackgroundColor, customBackgroundY, customBackgroundU, customBackgroundV, 0, customTextColor,
                customTextY, customTextU, customTextV, textSize, arr, (int)justify, xOffset, yOffset);

            var ptrV4 = Marshal.AllocHGlobal(Marshal.SizeOf<MMAL_PARAMETER_CAMERA_ANNOTATE_V4_T>() + (arr.Length - 1));
            Marshal.StructureToPtr(strV4, ptrV4, false);

            try
            {
                MmalCheck(MmalPort.mmal_port_parameter_set(camera.Control.Ptr, (MMAL_PARAMETER_HEADER_T*)ptrV4), "Unable to set annotate");
            }
            catch
            {
                Marshal.FreeHGlobal(ptrV4);
                ptrV4 = IntPtr.Zero;

                MmalLog.Logger.LogWarning("Unable to set V4 annotation structure. Trying V3. Please update firmware to latest version.");

                var str = new MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T(
                    new MMAL_PARAMETER_HEADER_T(MmalParameterAnnotate, Marshal.SizeOf<MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T>() + (arr.Length - 1)),
                    1, showShutter, showAnalogGain, showLens,
                    showCaf, showMotion, showFrame, enableTextBackground,
                    customBackgroundColor, customBackgroundY, customBackgroundU, customBackgroundV, 0, customTextColor,
                    customTextY, customTextU, customTextV, textSize, arr);

                var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T>() + (arr.Length - 1));
                Marshal.StructureToPtr(str, ptr, false);

                try
                {
                    MmalCheck(MmalPort.mmal_port_parameter_set(camera.Control.Ptr, (MMAL_PARAMETER_HEADER_T*)ptr), "Unable to set annotate V3.");
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
            finally
            {
                if (ptrV4 != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptrV4);
            }
        }

        internal static void DisableAnnotate(this MmalCameraComponent camera)
        {
            var annotate = new MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterAnnotate, Marshal.SizeOf<MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T>()),
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null);

            MmalCheck(MmalPort.mmal_port_parameter_get(camera.Control.Ptr, &annotate.Hdr), "Unable to get camera annotate settings");

            var disableAnnotate = new MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterAnnotate, Marshal.SizeOf<MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T>()), 0,
                annotate.ShowShutter, annotate.ShowAnalogGain, annotate.ShowLens, annotate.ShowCaf, annotate.ShowMotion, annotate.ShowFrameNum,
                annotate.EnableTextBackground, annotate.CustomBackgroundColor, annotate.CustomBackgroundY, annotate.CustomBackgroundU, annotate.CustomBackgroundV, 0,
                annotate.CustomTextColor, annotate.CustomTextY, annotate.CustomTextU, annotate.CustomTextV, annotate.TextSize, annotate.Text);

            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T>());
            Marshal.StructureToPtr(disableAnnotate, ptr, false);

            try
            {
                MmalCheck(MmalPort.mmal_port_parameter_set(camera.Control.Ptr, (MMAL_PARAMETER_HEADER_T*)ptr), "Unable to set annotate");
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public static MmalSensorMode GetSensorMode(this MmalCameraComponent camera)
        {
            return (MmalSensorMode)(int)camera.Control.GetParameter(MmalParameterCameraCustomSensorConfig);
        }

        internal static void SetSensorMode(this MmalCameraComponent camera, MmalSensorMode mode)
        {
            var currentMode = (int)camera.Control.GetParameter(MmalParameterCameraCustomSensorConfig);

            // Don't try and set the sensor mode if we aren't changing it.
            if (currentMode != 0 || MmalCameraConfig.SensorMode != 0)
                camera.Control.SetParameter(MmalParameterCameraCustomSensorConfig, MmalCameraConfig.SensorMode);
        }

        public static int GetSaturation(this MmalCameraComponent camera)
        {
            return (int)camera.Control.GetParameter(MmalParameterSaturation);
        }

        internal static void SetSaturation(this MmalCameraComponent camera, int saturation)
        {
            MmalLog.Logger.LogDebug($"Setting saturation: {saturation}");

            var value = new MMAL_RATIONAL_T(saturation, 100);

            if (saturation >= -100 && saturation <= 100)
                camera.Control.SetParameter(MmalParameterSaturation, value);
            else
                throw new Exception("Invalid saturation value");
        }

        public static int GetSharpness(this MmalCameraComponent camera)
        {
            return (int)camera.Control.GetParameter(MmalParameterSharpness);
        }

        internal static void SetSharpness(this MmalCameraComponent camera, int sharpness)
        {
            MmalLog.Logger.LogDebug($"Setting sharpness: {sharpness}");

            var value = new MMAL_RATIONAL_T(sharpness, 100);

            if (sharpness >= -100 && sharpness <= 100)
                camera.Control.SetParameter(MmalParameterSharpness, value);
            else
                throw new Exception("Invalid sharpness value");
        }

        public static int GetContrast(this MmalCameraComponent camera)
        {
            return (int)camera.Control.GetParameter(MmalParameterContrast);
        }

        internal static void SetContrast(this MmalCameraComponent camera, int contrast)
        {
            MmalLog.Logger.LogDebug($"Setting contrast: {contrast}");

            var value = new MMAL_RATIONAL_T(contrast, 100);

            if (contrast >= -100 && contrast <= 100)
                camera.Control.SetParameter(MmalParameterContrast, value);
            else
                throw new Exception("Invalid contrast value");
        }

        internal static void SetDisableExif(this MmalImageEncoder encoder, bool disable)
        {
            encoder.Outputs[0].SetParameter(MmalParameterExifDisable, disable);
        }

        public static int GetBrightness(this MmalCameraComponent camera)
        {
            return (int)camera.Control.GetParameter(MmalParameterBrightness);
        }

        internal static void SetBrightness(this MmalCameraComponent camera, int brightness)
        {
            MmalLog.Logger.LogDebug($"Setting brightness: {brightness}");

            var value = new MMAL_RATIONAL_T(brightness, 100);

            if (brightness >= 0 && brightness <= 100)
                camera.Control.SetParameter(MmalParameterBrightness, value);
            else
                throw new Exception("Invalid brightness value");
        }

        public static int GetIso(this MmalCameraComponent camera)
        {
            return (int)camera.Control.GetParameter(MmalParameterIso);
        }

        internal static void SetIso(this MmalCameraComponent camera, int iso)
        {
            MmalLog.Logger.LogDebug($"Setting Iso: {iso}");

            // 0 = auto
            if ((iso < 100 || iso > 800) && iso > 0)
                throw new ArgumentOutOfRangeException(nameof(iso), iso, "Invalid Iso setting. Valid values: 100 - 800");

            camera.Control.SetParameter(MmalParameterIso, iso);
        }

        public static bool GetVideoStabilisation(this MmalCameraComponent camera)
        {
            return camera.Control.GetParameter(MmalParameterVideoStabilisation);
        }

        internal static void SetVideoStabilisation(this MmalCameraComponent camera, bool vstabilisation)
        {
            MmalLog.Logger.LogDebug($"Setting video stabilisation: {vstabilisation}");

            camera.Control.SetParameter(MmalParameterVideoStabilisation, vstabilisation);
        }

        public static int GetExposureCompensation(this MmalCameraComponent camera)
        {
            return camera.Control.GetParameter(MmalParameterExposureComp);
        }

        internal static void SetExposureCompensation(this MmalCameraComponent camera, int expCompensation)
        {
            MmalLog.Logger.LogDebug($"Setting exposure compensation: {expCompensation}");

            if (expCompensation < -10 || expCompensation > 10)
                throw new ArgumentOutOfRangeException(nameof(expCompensation), expCompensation, "Invalid exposure compensation value. Valid values (-10 - 10)");

            camera.Control.SetParameter(MmalParameterExposureComp, expCompensation);
        }

        public static MmalParamExposuremodeT GetExposureMode(this MmalCameraComponent camera)
        {
            var expMode = new MMAL_PARAMETER_EXPOSUREMODE_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterExposureMode, Marshal.SizeOf<MMAL_PARAMETER_EXPOSUREMODE_T>()),
                default(MmalParamExposuremodeT));

            MmalCheck(MmalPort.mmal_port_parameter_get(camera.Control.Ptr, &expMode.Hdr), "Unable to get exposure mode");

            return expMode.Value;
        }

        internal static void SetExposureMode(this MmalCameraComponent camera, MmalParamExposuremodeT mode)
        {
            MmalLog.Logger.LogDebug($"Setting exposure mode: {mode}");

            var expMode = new MMAL_PARAMETER_EXPOSUREMODE_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterExposureMode, Marshal.SizeOf<MMAL_PARAMETER_EXPOSUREMODE_T>()),
                mode);

            MmalCheck(MmalPort.mmal_port_parameter_set(camera.Control.Ptr, &expMode.Hdr), "Unable to set exposure mode");
        }

        public static MmalParamExposuremeteringmodeT GetExposureMeteringMode(this MmalCameraComponent camera)
        {
            var expMode = new MMAL_PARAMETER_EXPOSUREMETERINGMODE_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterExpMeteringMode, Marshal.SizeOf<MMAL_PARAMETER_EXPOSUREMETERINGMODE_T>()),
                default(MmalParamExposuremeteringmodeT));

            MmalCheck(MmalPort.mmal_port_parameter_get(camera.Control.Ptr, &expMode.Hdr), "Unable to get exposure metering mode");

            return expMode.Value;
        }

        internal static void SetExposureMeteringMode(this MmalCameraComponent camera, MmalParamExposuremeteringmodeT mode)
        {
            MmalLog.Logger.LogDebug($"Setting exposure metering mode: {mode}");

            var expMode = new MMAL_PARAMETER_EXPOSUREMETERINGMODE_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterExpMeteringMode, Marshal.SizeOf<MMAL_PARAMETER_EXPOSUREMETERINGMODE_T>()),
                                                                                                        mode);

            MmalCheck(MmalPort.mmal_port_parameter_set(camera.Control.Ptr, &expMode.Hdr), "Unable to set exposure metering mode");
        }

        public static MmalParamAwbmodeT GetAwbMode(this MmalCameraComponent camera)
        {
            var awbMode = new MMAL_PARAMETER_AWBMODE_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterAwbMode, Marshal.SizeOf<MMAL_PARAMETER_AWBMODE_T>()),
                                                                                                        default(MmalParamAwbmodeT));

            MmalCheck(MmalPort.mmal_port_parameter_get(camera.Control.Ptr, &awbMode.Hdr), "Unable to get awb mode");

            return awbMode.Value;
        }

        internal static void SetAwbMode(this MmalCameraComponent camera, MmalParamAwbmodeT mode)
        {
            MmalLog.Logger.LogDebug($"Setting AWB mode: {mode}");

            var awbMode = new MMAL_PARAMETER_AWBMODE_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterAwbMode, Marshal.SizeOf<MMAL_PARAMETER_AWBMODE_T>()),
                                                                                                        mode);

            MmalCheck(MmalPort.mmal_port_parameter_set(camera.Control.Ptr, &awbMode.Hdr), "Unable to set awb mode");
        }

        public static int GetExposureSpeed(this MmalCameraComponent camera)
        {
            var settings = new MMAL_PARAMETER_CAMERA_SETTINGS_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterCameraSettings, Marshal.SizeOf<MMAL_PARAMETER_CAMERA_SETTINGS_T>()),
                0, new MMAL_RATIONAL_T(0, 0), new MMAL_RATIONAL_T(0, 0),
                new MMAL_RATIONAL_T(0, 0), new MMAL_RATIONAL_T(0, 0), 0);

            MmalCheck(MmalPort.mmal_port_parameter_get(camera.Control.Ptr, &settings.Hdr), "Unable to get camera settings");

            return settings.Exposure;
        }

        public static int GetFocusPosition(this MmalCameraComponent camera)
        {
            var settings = new MMAL_PARAMETER_CAMERA_SETTINGS_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterCameraSettings, Marshal.SizeOf<MMAL_PARAMETER_CAMERA_SETTINGS_T>()),
                0, new MMAL_RATIONAL_T(0, 0), new MMAL_RATIONAL_T(0, 0),
                new MMAL_RATIONAL_T(0, 0), new MMAL_RATIONAL_T(0, 0), 0);

            MmalCheck(MmalPort.mmal_port_parameter_get(camera.Control.Ptr, &settings.Hdr), "Unable to get camera settings");

            return settings.FocusPosition;
        }

        public static double GetAwbRedGain(this MmalCameraComponent camera)
        {
            var settings = new MMAL_PARAMETER_CAMERA_SETTINGS_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterCameraSettings, Marshal.SizeOf<MMAL_PARAMETER_CAMERA_SETTINGS_T>()),
                0, new MMAL_RATIONAL_T(0, 0), new MMAL_RATIONAL_T(0, 0),
                new MMAL_RATIONAL_T(0, 0), new MMAL_RATIONAL_T(0, 0), 0);

            MmalCheck(MmalPort.mmal_port_parameter_get(camera.Control.Ptr, &settings.Hdr), "Unable to get camera settings");

            return Convert.ToDouble(settings.AwbRedGain.Num / settings.AwbRedGain.Den);
        }

        public static double GetAwbBlueGain(this MmalCameraComponent camera)
        {
            var settings = new MMAL_PARAMETER_CAMERA_SETTINGS_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterCameraSettings, Marshal.SizeOf<MMAL_PARAMETER_CAMERA_SETTINGS_T>()),
                0, new MMAL_RATIONAL_T(0, 0), new MMAL_RATIONAL_T(0, 0),
                new MMAL_RATIONAL_T(0, 0), new MMAL_RATIONAL_T(0, 0), 0);

            MmalCheck(MmalPort.mmal_port_parameter_get(camera.Control.Ptr, &settings.Hdr), "Unable to get camera settings");

            return Convert.ToDouble(settings.AwbBlueGain.Num / settings.AwbBlueGain.Den);
        }

        internal static void SetAwbGains(this MmalCameraComponent camera, double rGain, double bGain)
        {
            MmalLog.Logger.LogDebug($"Setting AWB gains: {rGain}, {bGain}");

            if (MmalCameraConfig.AwbMode != MmalParamAwbmodeT.MmalParamAwbmodeOff && (rGain > 0 || bGain > 0))
                throw new PiCameraError("AWB Mode must be off when setting AWB gains");

            var awbGains = new MMAL_PARAMETER_AWB_GAINS_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterCustomAwbGains, Marshal.SizeOf<MMAL_PARAMETER_AWB_GAINS_T>()),
                                                                                                        new MMAL_RATIONAL_T((int)(rGain * 65536), 65536),
                                                                                                        new MMAL_RATIONAL_T((int)(bGain * 65536), 65536));

            MmalCheck(MmalPort.mmal_port_parameter_set(camera.Control.Ptr, &awbGains.Hdr), "Unable to set awb gains");
        }

        public static MmalParamImagefxT GetImageFx(this MmalCameraComponent camera)
        {
            var imgFx = new MMAL_PARAMETER_IMAGEFX_T(new MMAL_PARAMETER_HEADER_T(MmalParameterImageEffect, Marshal.SizeOf<MMAL_PARAMETER_IMAGEFX_T>()), default(MmalParamImagefxT));

            MmalCheck(MmalPort.mmal_port_parameter_get(camera.Control.Ptr, &imgFx.Hdr), "Unable to get image fx");

            return imgFx.Value;
        }

        internal static void SetImageFx(this MmalCameraComponent camera, MmalParamImagefxT imageFx)
        {
            MmalLog.Logger.LogDebug($"Setting Image FX: {imageFx}");

            var imgFx = new MMAL_PARAMETER_IMAGEFX_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterImageEffect, Marshal.SizeOf<MMAL_PARAMETER_IMAGEFX_T>()),
                imageFx);

            MmalCheck(MmalPort.mmal_port_parameter_set(camera.Control.Ptr, &imgFx.Hdr), "Unable to set image fx");
        }

        public static ColorEffects GetColourFx(this MmalCameraComponent camera)
        {
            var colFx = new MMAL_PARAMETER_COLOURFX_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterColorEffect, Marshal.SizeOf<MMAL_PARAMETER_COLOURFX_T>()),
                                                                                                        0,
                                                                                                        0,
                                                                                                        0);

            MmalCheck(MmalPort.mmal_port_parameter_get(camera.Control.Ptr, &colFx.Hdr), "Unable to get colour fx");

            var fx = new ColorEffects(colFx.Enable == 1, MmalColor.FromYuvBytes(0, (byte)colFx.U, (byte)colFx.V));

            return fx;
        }

        internal static void SetColourFx(this MmalCameraComponent camera, ColorEffects colorFx)
        {
            MmalLog.Logger.LogDebug("Setting colour effects");

            var (_, u, v) = MmalColor.RgbToYuvBytes(colorFx.Color);

            var colFx = new MMAL_PARAMETER_COLOURFX_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterColorEffect, Marshal.SizeOf<MMAL_PARAMETER_COLOURFX_T>()),
                                                                                                        colorFx.Enable ? 1 : 0,
                                                                                                        u,
                                                                                                        v);

            MmalCheck(MmalPort.mmal_port_parameter_set(camera.Control.Ptr, &colFx.Hdr), "Unable to set colour fx");
        }

        public static int GetRotation(this MmalCameraComponent camera)
        {
            return camera.StillPort.GetParameter(MmalParameterRotation);
        }

        internal static void SetRotation(this MmalCameraComponent camera, int rotation)
        {
            var rot = ((rotation % 360) / 90) * 90;

            MmalLog.Logger.LogDebug($"Setting rotation: {rot}");

            camera.StillPort.SetParameter(MmalParameterRotation, rot);
        }

        public static MmalParamMirrorT GetFlips(this MmalCameraComponent camera) => GetStillFlips(camera);

        public static MmalParamMirrorT GetVideoFlips(this MmalCameraComponent camera)
        {
            var mirror = new MMAL_PARAMETER_MIRROR_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterMirror, Marshal.SizeOf<MMAL_PARAMETER_MIRROR_T>()),
                MmalParamMirrorT.MmalParamMirrorNone);

            MmalCheck(MmalPort.mmal_port_parameter_get(camera.VideoPort.Ptr, &mirror.Hdr), "Unable to get flips");

            return mirror.Value;
        }

        public static MmalParamMirrorT GetStillFlips(this MmalCameraComponent camera)
        {
            var mirror = new MMAL_PARAMETER_MIRROR_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterMirror, Marshal.SizeOf<MMAL_PARAMETER_MIRROR_T>()),
                MmalParamMirrorT.MmalParamMirrorNone);

            MmalCheck(MmalPort.mmal_port_parameter_get(camera.StillPort.Ptr, &mirror.Hdr), "Unable to get flips");

            return mirror.Value;
        }

        internal static void SetFlips(this MmalCameraComponent camera, MmalParamMirrorT flips)
        {
            var mirror = new MMAL_PARAMETER_MIRROR_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterMirror, Marshal.SizeOf<MMAL_PARAMETER_MIRROR_T>()),
                flips);

            MmalCheck(MmalPort.mmal_port_parameter_set(camera.StillPort.Ptr, &mirror.Hdr), "Unable to set flips");
            MmalCheck(MmalPort.mmal_port_parameter_set(camera.VideoPort.Ptr, &mirror.Hdr), "Unable to set flips");
        }

        public static MMAL_RECT_T GetZoom(this MmalCameraComponent camera)
        {
            var crop = new MMAL_PARAMETER_INPUT_CROP_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterInputCrop, Marshal.SizeOf<MMAL_PARAMETER_INPUT_CROP_T>()),
                default(MMAL_RECT_T));

            MmalCheck(MmalPort.mmal_port_parameter_get(camera.Control.Ptr, &crop.Hdr), "Unable to get zoom");

            return crop.Rect;
        }

        internal static void SetZoom(this MmalCameraComponent camera, Zoom rect)
        {
            if (rect.X > 1.0 || rect.Y > 1.0 || rect.Height > 1.0 || rect.Width > 1.0)
                throw new ArgumentOutOfRangeException(nameof(rect), "Invalid zoom settings. Value mustn't be greater than 1.0");

            var crop = new MMAL_PARAMETER_INPUT_CROP_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterInputCrop, Marshal.SizeOf<MMAL_PARAMETER_INPUT_CROP_T>()),
                new MMAL_RECT_T(Convert.ToInt32(65536 * rect.X), Convert.ToInt32(65536 * rect.Y), Convert.ToInt32(65536 * rect.Width), Convert.ToInt32(65536 * rect.Height)));

            MmalCheck(MmalPort.mmal_port_parameter_set(camera.Control.Ptr, &crop.Hdr), "Unable to set zoom");
        }

        public static int GetShutterSpeed(this MmalCameraComponent camera)
        {
            return (int)camera.Control.GetParameter(MmalParameterShutterSpeed);
        }

        internal static void SetShutterSpeed(this MmalCameraComponent camera, int speed)
        {
            MmalLog.Logger.LogDebug($"Setting shutter speed: {speed}");

            if (speed > 6000000)
                MmalLog.Logger.LogWarning("Shutter speed exceeds upper supported limit of 6000ms. Undefined behaviour may result.");

            camera.Control.SetParameter(MmalParameterShutterSpeed, speed);
        }

        public static MmalParameterDrcStrengthT GetDrc(this MmalCameraComponent camera)
        {
            var drc = new MMAL_PARAMETER_DRC_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterDynamicRangeCompression, Marshal.SizeOf<MMAL_PARAMETER_DRC_T>()),
                default(MmalParameterDrcStrengthT));

            MmalCheck(MmalPort.mmal_port_parameter_get(camera.Control.Ptr, &drc.Hdr), "Unable to get DRC");

            return drc.Strength;
        }

        internal static void SetDrc(this MmalCameraComponent camera, MmalParameterDrcStrengthT strength)
        {
            var drc = new MMAL_PARAMETER_DRC_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterDynamicRangeCompression, Marshal.SizeOf<MMAL_PARAMETER_DRC_T>()),
                strength);

            MmalCheck(MmalPort.mmal_port_parameter_set(camera.Control.Ptr, &drc.Hdr), "Unable to set DRC");
        }

        public static bool GetStatsPass(this MmalCameraComponent camera)
        {
            return camera.Control.GetParameter(MmalParameterCaptureStatsPass);
        }

        internal static void SetStatsPass(this MmalCameraComponent camera, bool statsPass)
        {
            camera.Control.SetParameter(MmalParameterCaptureStatsPass, statsPass);
        }

        public static bool GetBurstMode(this MmalCameraComponent camera)
        {
            return camera.StillPort.GetParameter(MmalParameterCameraBurstCapture);
        }

        internal static void SetBurstMode(this MmalCameraComponent camera, bool burstMode)
        {
            camera.StillPort.SetParameter(MmalParameterCameraBurstCapture, burstMode);
        }

        public static double GetAnalogGain(this MmalCameraComponent camera)
        {
            return (double)camera.Control.GetParameter(MmalParameterAnalogGain);
        }

        internal static void SetAnalogGain(this MmalCameraComponent camera, double analogGain)
        {
            if (analogGain > 0 && (analogGain < 1.0 || analogGain > 8.0))
                throw new ArgumentOutOfRangeException(nameof(analogGain), "Invalid analog gain settings. Value must be between 1.0 and 8.0.");

            var num = (int)analogGain * 65536;

            camera.Control.SetParameter(MmalParameterAnalogGain, new MMAL_RATIONAL_T(num, 65536));
        }

        public static double GetDigitalGain(this MmalCameraComponent camera)
        {
            return (double)camera.Control.GetParameter(MmalParameterDigitalGain);
        }

        internal static void SetDigitalGain(this MmalCameraComponent camera, double digitalGain)
        {
            if (digitalGain > 0 && (digitalGain < 1.0 || digitalGain > 255.0))
                throw new ArgumentOutOfRangeException(nameof(digitalGain), "Invalid digital gain settings. Value must be between 1.0 and 255.0.");

            var num = (int)digitalGain * 65536;

            camera.Control.SetParameter(MmalParameterDigitalGain, new MMAL_RATIONAL_T(num, 65536));
        }
    }
}