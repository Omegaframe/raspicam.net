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
using MMALSharp.Native.Parameters;
using MMALSharp.Native.Port;
using MMALSharp.Native.Util;
using MMALSharp.Ports.Controls;
using MMALSharp.Utility;
using static MMALSharp.MmalNativeExceptionHelper;
using static MMALSharp.Native.Parameters.MmalParametersCamera;

namespace MMALSharp.Extensions
{
    public static unsafe class MmalCameraComponentExtensions
    {
        internal static void SetCameraConfig(this MmalCameraComponent camera, MmalParameterCameraConfigType value)
        {
            MmalCheck(MmalPort.SetParameter(camera.Control.Ptr, &value.Hdr), "Unable to set camera config.");
        }

        internal static void SetChangeEventRequest(this IControlPort controlPort, MmalParameterChangeEventRequestType value)
        {
            MmalCheck(MmalPort.SetParameter(controlPort.Ptr, &value.Hdr), "Unable to set camera event request.");
        }

        public static bool GetIsEnabledAnnotateSettings(this MmalCameraComponent camera)
        {
            var annotate = new MmalParameterCameraAnnotateV3Type(
                new MmalParameterHeaderType(MmalParameterAnnotate, Marshal.SizeOf<MmalParameterCameraAnnotateV3Type>()),
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null);

            MmalCheck(MmalPort.GetParameter(camera.Control.Ptr, &annotate.Hdr), "Unable to get camera annotate settings");

            return annotate.Enable == 1;
        }

        public static bool GetShowShutterAnnotateSettings(this MmalCameraComponent camera)
        {
            var annotate = new MmalParameterCameraAnnotateV3Type(
                new MmalParameterHeaderType(MmalParameterAnnotate, Marshal.SizeOf<MmalParameterCameraAnnotateV3Type>()),
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null);

            MmalCheck(MmalPort.GetParameter(camera.Control.Ptr, &annotate.Hdr), "Unable to get camera annotate settings");

            return annotate.ShowShutter == 1;
        }

        public static bool GetShowCafAnnotateSettings(this MmalCameraComponent camera)
        {
            var annotate = new MmalParameterCameraAnnotateV3Type(
                new MmalParameterHeaderType(MmalParameterAnnotate, Marshal.SizeOf<MmalParameterCameraAnnotateV3Type>()),
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null);

            MmalCheck(MmalPort.GetParameter(camera.Control.Ptr, &annotate.Hdr), "Unable to get camera annotate settings");

            return annotate.ShowCaf == 1;
        }

        public static bool GetShowGainAnnotateSettings(this MmalCameraComponent camera)
        {
            var annotate = new MmalParameterCameraAnnotateV3Type(
                new MmalParameterHeaderType(MmalParameterAnnotate, Marshal.SizeOf<MmalParameterCameraAnnotateV3Type>()),
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null);

            MmalCheck(MmalPort.GetParameter(camera.Control.Ptr, &annotate.Hdr), "Unable to get camera annotate settings");

            return annotate.ShowAnalogGain == 1;
        }

        public static bool GetShowLensAnnotateSettings(this MmalCameraComponent camera)
        {
            var annotate = new MmalParameterCameraAnnotateV3Type(
                new MmalParameterHeaderType(MmalParameterAnnotate, Marshal.SizeOf<MmalParameterCameraAnnotateV3Type>()),
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null);

            MmalCheck(MmalPort.GetParameter(camera.Control.Ptr, &annotate.Hdr), "Unable to get camera annotate settings");

            return annotate.ShowLens == 1;
        }

        public static bool GetShowMotionAnnotateSettings(this MmalCameraComponent camera)
        {
            var annotate = new MmalParameterCameraAnnotateV3Type(
                new MmalParameterHeaderType(MmalParameterAnnotate, Marshal.SizeOf<MmalParameterCameraAnnotateV3Type>()),
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null);

            MmalCheck(MmalPort.GetParameter(camera.Control.Ptr, &annotate.Hdr), "Unable to get camera annotate settings");

            return annotate.ShowMotion == 1;
        }

        public static bool GetShowFrameNumberAnnotateSettings(this MmalCameraComponent camera)
        {
            var annotate = new MmalParameterCameraAnnotateV3Type(
                new MmalParameterHeaderType(MmalParameterAnnotate, Marshal.SizeOf<MmalParameterCameraAnnotateV3Type>()),
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null);

            MmalCheck(MmalPort.GetParameter(camera.Control.Ptr, &annotate.Hdr), "Unable to get camera annotate settings");

            return annotate.ShowFrameNum == 1;
        }

        public static bool GetShowBlackBackgroundAnnotateSettings(this MmalCameraComponent camera)
        {
            var annotate = new MmalParameterCameraAnnotateV3Type(
                new MmalParameterHeaderType(MmalParameterAnnotate, Marshal.SizeOf<MmalParameterCameraAnnotateV3Type>()),
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null);

            MmalCheck(MmalPort.GetParameter(camera.Control.Ptr, &annotate.Hdr), "Unable to get camera annotate settings");

            return annotate.EnableTextBackground == 1;
        }

        public static string GetCustomTextAnnotateSettings(this MmalCameraComponent camera)
        {
            var annotate = new MmalParameterCameraAnnotateV3Type(
                new MmalParameterHeaderType(MmalParameterAnnotate, Marshal.SizeOf<MmalParameterCameraAnnotateV3Type>()),
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null);

            MmalCheck(MmalPort.GetParameter(camera.Control.Ptr, &annotate.Hdr), "Unable to get camera annotate settings");

            return Encoding.ASCII.GetString(annotate.Text);
        }

        public static Color GetTextColourAnnotateSettings(this MmalCameraComponent camera)
        {
            var annotate = new MmalParameterCameraAnnotateV3Type(
                new MmalParameterHeaderType(MmalParameterAnnotate, Marshal.SizeOf<MmalParameterCameraAnnotateV3Type>()),
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null);

            MmalCheck(MmalPort.GetParameter(camera.Control.Ptr, &annotate.Hdr), "Unable to get camera annotate settings");

            return MmalColor.FromYuvBytes(annotate.CustomTextY, annotate.CustomTextU, annotate.CustomTextV);
        }

        public static Color GetBackgroundColourAnnotateSettings(this MmalCameraComponent camera)
        {
            var annotate = new MmalParameterCameraAnnotateV3Type(
                new MmalParameterHeaderType(MmalParameterAnnotate, Marshal.SizeOf<MmalParameterCameraAnnotateV3Type>()),
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null);

            MmalCheck(MmalPort.GetParameter(camera.Control.Ptr, &annotate.Hdr), "Unable to get camera annotate settings");

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

            var strV4 = new MmalParameterCameraAnnotateV4Type(
                new MmalParameterHeaderType(MmalParameterAnnotate, Marshal.SizeOf<MmalParameterCameraAnnotateV4Type>() + (arr.Length - 1)),
                1, showShutter, showAnalogGain, showLens,
                showCaf, showMotion, showFrame, enableTextBackground,
                customBackgroundColor, customBackgroundY, customBackgroundU, customBackgroundV, 0, customTextColor,
                customTextY, customTextU, customTextV, textSize, arr, (int)justify, xOffset, yOffset);

            var ptrV4 = Marshal.AllocHGlobal(Marshal.SizeOf<MmalParameterCameraAnnotateV4Type>() + (arr.Length - 1));
            Marshal.StructureToPtr(strV4, ptrV4, false);

            try
            {
                MmalCheck(MmalPort.SetParameter(camera.Control.Ptr, (MmalParameterHeaderType*)ptrV4), "Unable to set annotate");
            }
            catch
            {
                Marshal.FreeHGlobal(ptrV4);
                ptrV4 = IntPtr.Zero;

                MmalLog.Logger.LogWarning("Unable to set V4 annotation structure. Trying V3. Please update firmware to latest version.");

                var str = new MmalParameterCameraAnnotateV3Type(
                    new MmalParameterHeaderType(MmalParameterAnnotate, Marshal.SizeOf<MmalParameterCameraAnnotateV3Type>() + (arr.Length - 1)),
                    1, showShutter, showAnalogGain, showLens,
                    showCaf, showMotion, showFrame, enableTextBackground,
                    customBackgroundColor, customBackgroundY, customBackgroundU, customBackgroundV, 0, customTextColor,
                    customTextY, customTextU, customTextV, textSize, arr);

                var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<MmalParameterCameraAnnotateV3Type>() + (arr.Length - 1));
                Marshal.StructureToPtr(str, ptr, false);

                try
                {
                    MmalCheck(MmalPort.SetParameter(camera.Control.Ptr, (MmalParameterHeaderType*)ptr), "Unable to set annotate V3.");
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
            var annotate = new MmalParameterCameraAnnotateV3Type(
                new MmalParameterHeaderType(MmalParameterAnnotate, Marshal.SizeOf<MmalParameterCameraAnnotateV3Type>()),
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null);

            MmalCheck(MmalPort.GetParameter(camera.Control.Ptr, &annotate.Hdr), "Unable to get camera annotate settings");

            var disableAnnotate = new MmalParameterCameraAnnotateV3Type(
                new MmalParameterHeaderType(MmalParameterAnnotate, Marshal.SizeOf<MmalParameterCameraAnnotateV3Type>()), 0,
                annotate.ShowShutter, annotate.ShowAnalogGain, annotate.ShowLens, annotate.ShowCaf, annotate.ShowMotion, annotate.ShowFrameNum,
                annotate.EnableTextBackground, annotate.CustomBackgroundColor, annotate.CustomBackgroundY, annotate.CustomBackgroundU, annotate.CustomBackgroundV, 0,
                annotate.CustomTextColor, annotate.CustomTextY, annotate.CustomTextU, annotate.CustomTextV, annotate.TextSize, annotate.Text);

            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<MmalParameterCameraAnnotateV3Type>());
            Marshal.StructureToPtr(disableAnnotate, ptr, false);

            try
            {
                MmalCheck(MmalPort.SetParameter(camera.Control.Ptr, (MmalParameterHeaderType*)ptr), "Unable to set annotate");
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

            var value = new MmalRational(saturation, 100);

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

            var value = new MmalRational(sharpness, 100);

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

            var value = new MmalRational(contrast, 100);

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

            var value = new MmalRational(brightness, 100);

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

        public static MmalParamExposuremodeType GetExposureMode(this MmalCameraComponent camera)
        {
            var expMode = new MmalParameterExposuremodeType(
                new MmalParameterHeaderType(MmalParameterExposureMode, Marshal.SizeOf<MmalParameterExposuremodeType>()),
                default(MmalParamExposuremodeType));

            MmalCheck(MmalPort.GetParameter(camera.Control.Ptr, &expMode.Hdr), "Unable to get exposure mode");

            return expMode.Value;
        }

        internal static void SetExposureMode(this MmalCameraComponent camera, MmalParamExposuremodeType mode)
        {
            MmalLog.Logger.LogDebug($"Setting exposure mode: {mode}");

            var expMode = new MmalParameterExposuremodeType(
                new MmalParameterHeaderType(MmalParameterExposureMode, Marshal.SizeOf<MmalParameterExposuremodeType>()),
                mode);

            MmalCheck(MmalPort.SetParameter(camera.Control.Ptr, &expMode.Hdr), "Unable to set exposure mode");
        }

        public static MmalParamExposuremeteringmodeType GetExposureMeteringMode(this MmalCameraComponent camera)
        {
            var expMode = new MmalParameterExposuremeteringmodeType(
                new MmalParameterHeaderType(MmalParameterExpMeteringMode, Marshal.SizeOf<MmalParameterExposuremeteringmodeType>()),
                default(MmalParamExposuremeteringmodeType));

            MmalCheck(MmalPort.GetParameter(camera.Control.Ptr, &expMode.Hdr), "Unable to get exposure metering mode");

            return expMode.Value;
        }

        internal static void SetExposureMeteringMode(this MmalCameraComponent camera, MmalParamExposuremeteringmodeType mode)
        {
            MmalLog.Logger.LogDebug($"Setting exposure metering mode: {mode}");

            var expMode = new MmalParameterExposuremeteringmodeType(
                new MmalParameterHeaderType(MmalParameterExpMeteringMode, Marshal.SizeOf<MmalParameterExposuremeteringmodeType>()),
                                                                                                        mode);

            MmalCheck(MmalPort.SetParameter(camera.Control.Ptr, &expMode.Hdr), "Unable to set exposure metering mode");
        }

        public static MmalParamAwbmodeType GetAwbMode(this MmalCameraComponent camera)
        {
            var awbMode = new MmalParameterAwbModeType(
                new MmalParameterHeaderType(MmalParameterAwbMode, Marshal.SizeOf<MmalParameterAwbModeType>()),
                                                                                                        default(MmalParamAwbmodeType));

            MmalCheck(MmalPort.GetParameter(camera.Control.Ptr, &awbMode.Hdr), "Unable to get awb mode");

            return awbMode.Value;
        }

        internal static void SetAwbMode(this MmalCameraComponent camera, MmalParamAwbmodeType mode)
        {
            MmalLog.Logger.LogDebug($"Setting AWB mode: {mode}");

            var awbMode = new MmalParameterAwbModeType(
                new MmalParameterHeaderType(MmalParameterAwbMode, Marshal.SizeOf<MmalParameterAwbModeType>()),
                                                                                                        mode);

            MmalCheck(MmalPort.SetParameter(camera.Control.Ptr, &awbMode.Hdr), "Unable to set awb mode");
        }

        public static int GetExposureSpeed(this MmalCameraComponent camera)
        {
            var settings = new MmalParameterCameraSettingsType(
                new MmalParameterHeaderType(MmalParameterCameraSettings, Marshal.SizeOf<MmalParameterCameraSettingsType>()),
                0, new MmalRational(0, 0), new MmalRational(0, 0),
                new MmalRational(0, 0), new MmalRational(0, 0), 0);

            MmalCheck(MmalPort.GetParameter(camera.Control.Ptr, &settings.Hdr), "Unable to get camera settings");

            return settings.Exposure;
        }

        public static int GetFocusPosition(this MmalCameraComponent camera)
        {
            var settings = new MmalParameterCameraSettingsType(
                new MmalParameterHeaderType(MmalParameterCameraSettings, Marshal.SizeOf<MmalParameterCameraSettingsType>()),
                0, new MmalRational(0, 0), new MmalRational(0, 0),
                new MmalRational(0, 0), new MmalRational(0, 0), 0);

            MmalCheck(MmalPort.GetParameter(camera.Control.Ptr, &settings.Hdr), "Unable to get camera settings");

            return settings.FocusPosition;
        }

        public static double GetAwbRedGain(this MmalCameraComponent camera)
        {
            var settings = new MmalParameterCameraSettingsType(
                new MmalParameterHeaderType(MmalParameterCameraSettings, Marshal.SizeOf<MmalParameterCameraSettingsType>()),
                0, new MmalRational(0, 0), new MmalRational(0, 0),
                new MmalRational(0, 0), new MmalRational(0, 0), 0);

            MmalCheck(MmalPort.GetParameter(camera.Control.Ptr, &settings.Hdr), "Unable to get camera settings");

            return Convert.ToDouble(settings.AwbRedGain.Num / settings.AwbRedGain.Den);
        }

        public static double GetAwbBlueGain(this MmalCameraComponent camera)
        {
            var settings = new MmalParameterCameraSettingsType(
                new MmalParameterHeaderType(MmalParameterCameraSettings, Marshal.SizeOf<MmalParameterCameraSettingsType>()),
                0, new MmalRational(0, 0), new MmalRational(0, 0),
                new MmalRational(0, 0), new MmalRational(0, 0), 0);

            MmalCheck(MmalPort.GetParameter(camera.Control.Ptr, &settings.Hdr), "Unable to get camera settings");

            return Convert.ToDouble(settings.AwbBlueGain.Num / settings.AwbBlueGain.Den);
        }

        internal static void SetAwbGains(this MmalCameraComponent camera, double rGain, double bGain)
        {
            MmalLog.Logger.LogDebug($"Setting AWB gains: {rGain}, {bGain}");

            if (MmalCameraConfig.AwbMode != MmalParamAwbmodeType.MmalParamAwbmodeOff && (rGain > 0 || bGain > 0))
                throw new PiCameraError("AWB Mode must be off when setting AWB gains");

            var awbGains = new MmalParameterAwbGainsType(
                new MmalParameterHeaderType(MmalParameterCustomAwbGains, Marshal.SizeOf<MmalParameterAwbGainsType>()),
                                                                                                        new MmalRational((int)(rGain * 65536), 65536),
                                                                                                        new MmalRational((int)(bGain * 65536), 65536));

            MmalCheck(MmalPort.SetParameter(camera.Control.Ptr, &awbGains.Hdr), "Unable to set awb gains");
        }

        public static MmalParamImagefxType GetImageFx(this MmalCameraComponent camera)
        {
            var imgFx = new MmalParameterImageFxType(new MmalParameterHeaderType(MmalParameterImageEffect, Marshal.SizeOf<MmalParameterImageFxType>()), default(MmalParamImagefxType));

            MmalCheck(MmalPort.GetParameter(camera.Control.Ptr, &imgFx.Hdr), "Unable to get image fx");

            return imgFx.Value;
        }

        internal static void SetImageFx(this MmalCameraComponent camera, MmalParamImagefxType imageFx)
        {
            MmalLog.Logger.LogDebug($"Setting Image FX: {imageFx}");

            var imgFx = new MmalParameterImageFxType(
                new MmalParameterHeaderType(MmalParameterImageEffect, Marshal.SizeOf<MmalParameterImageFxType>()),
                imageFx);

            MmalCheck(MmalPort.SetParameter(camera.Control.Ptr, &imgFx.Hdr), "Unable to set image fx");
        }

        public static ColorEffects GetColourFx(this MmalCameraComponent camera)
        {
            var colFx = new MmalParameterColorFxType(
                new MmalParameterHeaderType(MmalParameterColorEffect, Marshal.SizeOf<MmalParameterColorFxType>()),
                                                                                                        0,
                                                                                                        0,
                                                                                                        0);

            MmalCheck(MmalPort.GetParameter(camera.Control.Ptr, &colFx.Hdr), "Unable to get colour fx");

            var fx = new ColorEffects(colFx.Enable == 1, MmalColor.FromYuvBytes(0, (byte)colFx.U, (byte)colFx.V));

            return fx;
        }

        internal static void SetColourFx(this MmalCameraComponent camera, ColorEffects colorFx)
        {
            MmalLog.Logger.LogDebug("Setting colour effects");

            var (_, u, v) = MmalColor.RgbToYuvBytes(colorFx.Color);

            var colFx = new MmalParameterColorFxType(
                new MmalParameterHeaderType(MmalParameterColorEffect, Marshal.SizeOf<MmalParameterColorFxType>()),
                                                                                                        colorFx.Enable ? 1 : 0,
                                                                                                        u,
                                                                                                        v);

            MmalCheck(MmalPort.SetParameter(camera.Control.Ptr, &colFx.Hdr), "Unable to set colour fx");
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

        public static MmalParamMirrorType GetFlips(this MmalCameraComponent camera) => GetStillFlips(camera);

        public static MmalParamMirrorType GetVideoFlips(this MmalCameraComponent camera)
        {
            var mirror = new MmalParameterMirrorType(
                new MmalParameterHeaderType(MmalParameterMirror, Marshal.SizeOf<MmalParameterMirrorType>()),
                MmalParamMirrorType.MmalParamMirrorNone);

            MmalCheck(MmalPort.GetParameter(camera.VideoPort.Ptr, &mirror.Hdr), "Unable to get flips");

            return mirror.Value;
        }

        public static MmalParamMirrorType GetStillFlips(this MmalCameraComponent camera)
        {
            var mirror = new MmalParameterMirrorType(
                new MmalParameterHeaderType(MmalParameterMirror, Marshal.SizeOf<MmalParameterMirrorType>()),
                MmalParamMirrorType.MmalParamMirrorNone);

            MmalCheck(MmalPort.GetParameter(camera.StillPort.Ptr, &mirror.Hdr), "Unable to get flips");

            return mirror.Value;
        }

        internal static void SetFlips(this MmalCameraComponent camera, MmalParamMirrorType flips)
        {
            var mirror = new MmalParameterMirrorType(
                new MmalParameterHeaderType(MmalParameterMirror, Marshal.SizeOf<MmalParameterMirrorType>()),
                flips);

            MmalCheck(MmalPort.SetParameter(camera.StillPort.Ptr, &mirror.Hdr), "Unable to set flips");
            MmalCheck(MmalPort.SetParameter(camera.VideoPort.Ptr, &mirror.Hdr), "Unable to set flips");
        }

        public static MmalRect GetZoom(this MmalCameraComponent camera)
        {
            var crop = new MmalParameterInputCropType(
                new MmalParameterHeaderType(MmalParameterInputCrop, Marshal.SizeOf<MmalParameterInputCropType>()),
                default(MmalRect));

            MmalCheck(MmalPort.GetParameter(camera.Control.Ptr, &crop.Hdr), "Unable to get zoom");

            return crop.Rect;
        }

        internal static void SetZoom(this MmalCameraComponent camera, Zoom rect)
        {
            if (rect.X > 1.0 || rect.Y > 1.0 || rect.Height > 1.0 || rect.Width > 1.0)
                throw new ArgumentOutOfRangeException(nameof(rect), "Invalid zoom settings. Value mustn't be greater than 1.0");

            var crop = new MmalParameterInputCropType(
                new MmalParameterHeaderType(MmalParameterInputCrop, Marshal.SizeOf<MmalParameterInputCropType>()),
                new MmalRect(Convert.ToInt32(65536 * rect.X), Convert.ToInt32(65536 * rect.Y), Convert.ToInt32(65536 * rect.Width), Convert.ToInt32(65536 * rect.Height)));

            MmalCheck(MmalPort.SetParameter(camera.Control.Ptr, &crop.Hdr), "Unable to set zoom");
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

        public static MmalParameterDrcStrengthType GetDrc(this MmalCameraComponent camera)
        {
            var drc = new MmalParameterDrcType(
                new MmalParameterHeaderType(MmalParameterDynamicRangeCompression, Marshal.SizeOf<MmalParameterDrcType>()),
                default(MmalParameterDrcStrengthType));

            MmalCheck(MmalPort.GetParameter(camera.Control.Ptr, &drc.Hdr), "Unable to get DRC");

            return drc.Strength;
        }

        internal static void SetDrc(this MmalCameraComponent camera, MmalParameterDrcStrengthType strength)
        {
            var drc = new MmalParameterDrcType(
                new MmalParameterHeaderType(MmalParameterDynamicRangeCompression, Marshal.SizeOf<MmalParameterDrcType>()),
                strength);

            MmalCheck(MmalPort.SetParameter(camera.Control.Ptr, &drc.Hdr), "Unable to set DRC");
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

            camera.Control.SetParameter(MmalParameterAnalogGain, new MmalRational(num, 65536));
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

            camera.Control.SetParameter(MmalParameterDigitalGain, new MmalRational(num, 65536));
        }
    }
}