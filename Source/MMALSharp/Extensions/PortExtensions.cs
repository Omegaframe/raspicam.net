using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using MMALSharp.Config;
using MMALSharp.Native.Parameters;
using MMALSharp.Native.Port;
using MMALSharp.Native.Util;
using MMALSharp.Ports;
using MMALSharp.Utility;
using static MMALSharp.MmalNativeExceptionHelper;
using static MMALSharp.Native.Parameters.MmalParametersCamera;

namespace MMALSharp.Extensions
{
    static class PortExtensions
    {
        public static unsafe dynamic GetParameter(this IPort port, int key)
        {
            var t = MmalParameterHelpers.ParameterHelper.FirstOrDefault(c => c.ParamValue == key);

            if (t == null)
                throw new PiCameraError($"Could not find parameter {key}");

            MmalLog.Logger.LogDebug($"Getting parameter {t.ParamName}");

            try
            {
                switch (t.ParamType.Name)
                {
                    case nameof(MmalParameterBooleanType):
                        var boolVal = 0;
                        MmalCheck(MmalUtil.GetBoolean(port.Ptr, (uint)key, ref boolVal), "Unable to get boolean value");
                        return boolVal == 1;
                    case nameof(MmalParameterUint64Type):
                        var ulongVal = 0UL;
                        MmalCheck(MmalUtil.GetUint64(port.Ptr, (uint)key, ref ulongVal), "Unable to get ulong value");
                        return ulongVal;
                    case nameof(MmalParameterInt64Type):
                        var longVal = 0L;
                        MmalCheck(MmalUtil.GetInt64(port.Ptr, (uint)key, ref longVal), "Unable to get long value");
                        return longVal;
                    case nameof(MmalParameterUint32Type):
                        var uintVal = 0U;
                        MmalCheck(MmalUtil.GetUint32(port.Ptr, (uint)key, ref uintVal), "Unable to get uint value");
                        return uintVal;
                    case nameof(MmalParameterInt32Type):
                        var intVal = 0;
                        MmalCheck(MmalUtil.GetInt32(port.Ptr, (uint)key, ref intVal), "Unable to get int value");
                        return intVal;
                    case nameof(MmalParameterRationalType):
                        var ratVal = default(MmalRational);
                        MmalCheck(MmalUtil.GetRational(port.Ptr, (uint)key, ref ratVal), "Unable to get rational value");
                        return (double)ratVal.Num / ratVal.Den;
                    default:
                        throw new NotSupportedException($"The parameter type is unknown: {t.ParamType.Name}");
                }
            }
            catch
            {
                MmalLog.Logger.LogWarning($"Unable to get port parameter {t.ParamName}");
                throw;
            }
        }

        public static bool GetRawCapture(this IPort port)
        {
            return port.GetParameter(MmalParameterEnableRawCapture);
        }

        public static unsafe int[] GetSupportedEncodings(this IPort port)
        {
            var ptr1 = Marshal.AllocHGlobal(Marshal.SizeOf<MmalParameterEncodingType>() + 20);
            var str1 = (MmalParameterHeaderType*)ptr1;

            str1->Id = MmalParametersCommon.MmalParameterSupportedEncodings;

            // Deliberately undersize to check if running on older firmware.
            str1->Size = Marshal.SizeOf<MmalParameterEncodingType>() + 20;

            try
            {
                MmalCheck(MmalPort.GetParameter(port.Ptr, str1), "Unable to get supported encodings");
                var encodings = (MmalParameterEncodingType)Marshal.PtrToStructure(ptr1, typeof(MmalParameterEncodingType));
                return encodings.Value;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr1);
            }
        }

        public static string GetPortType(this PortType type)
        {
            return type switch
            {
                PortType.Input => "Input",
                PortType.Output => "Output",
                PortType.Clock => "Clock",
                PortType.Control => "Control",
                _ => string.Empty
            };
        }

        internal static unsafe void SetParameter(this IPort port, int key, dynamic value)
        {
            var t = MmalParameterHelpers.ParameterHelper.FirstOrDefault(c => c.ParamValue == key);

            if (t == null)
                throw new PiCameraError($"Could not find parameter {key}");

            MmalLog.Logger.LogDebug($"Setting parameter {t.ParamName}");

            try
            {
                switch (t.ParamType.Name)
                {
                    case nameof(MmalParameterBooleanType):
                        int i = (bool)value ? 1 : 0;
                        MmalCheck(MmalUtil.SetBoolean(port.Ptr, (uint)key, i), "Unable to set boolean value");
                        break;
                    case nameof(MmalParameterUint64Type):
                        MmalCheck(MmalUtil.SetUint64(port.Ptr, (uint)key, (ulong)value), "Unable to set ulong value");
                        break;
                    case nameof(MmalParameterInt64Type):
                        MmalCheck(MmalUtil.SetInt64(port.Ptr, (uint)key, (long)value), "Unable to set long value");
                        break;
                    case nameof(MmalParameterUint32Type):
                        MmalCheck(MmalUtil.SetUint32(port.Ptr, (uint)key, (uint)value), "Unable to set uint value");
                        break;
                    case nameof(MmalParameterInt32Type):
                        MmalCheck(MmalUtil.SetInt32(port.Ptr, (uint)key, (int)value), "Unable to set int value");
                        break;
                    case nameof(MmalParameterRationalType):
                        MmalCheck(MmalUtil.SetRational(port.Ptr, (uint)key, (MmalRational)value), "Unable to set rational value");
                        break;
                    case nameof(MmalParameterStringType):
                        MmalCheck(MmalUtil.SetString(port.Ptr, (uint)key, (string)value), "Unable to set rational value");
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
            catch
            {
                MmalLog.Logger.LogWarning($"Unable to set port parameter {t.ParamName}");
                throw;
            }
        }

        internal static void SetImageCapture(this IPort port, bool enable)
        {
            port.SetParameter(MmalParameterCapture, enable);
        }

        internal static void SetRawCapture(this IPort port, bool raw)
        {
            port.SetParameter(MmalParameterEnableRawCapture, raw);
        }

        internal static unsafe void SetStereoMode(this IPort port, StereoMode mode)
        {
            var stereo = new MmalParameterStereoscopicModeType(
                new MmalParameterHeaderType(MmalParameterStereoscopicMode, Marshal.SizeOf<MmalParameterStereoscopicModeType>()),
                mode.Mode,
                mode.Decimate,
                mode.SwapEyes);

            MmalCheck(MmalPort.SetParameter(port.Ptr, &stereo.Hdr), "Unable to set Stereo mode");
        }

        internal static void CheckSupportedEncoding(this IPort port, MmalEncoding encoding)
        {
            var encodings = port.GetSupportedEncodings();

            if (encodings.All(c => c != encoding.EncodingVal))
                throw new PiCameraError("Unsupported encoding type for this port");
        }

        internal static bool RgbOrderFixed(this IPort port)
        {
            var newFirmware = 0;
            var encodings = port.GetSupportedEncodings();

            foreach (var enc in encodings)
            {
                if (enc == Helpers.FourCcFromString("BGR3"))
                    break;

                if (enc == Helpers.FourCcFromString("RGB3"))
                    newFirmware = 1;
            }

            return newFirmware == 1;
        }
        
        public static unsafe MmalParameterFpsRangeType GetFramerateRange(this IPort port)
        {
            var str = new MmalParameterFpsRangeType(
                    new MmalParameterHeaderType(
                        MmalParametersCamera.MmalParameterFpsRange,
                        Marshal.SizeOf<MmalParameterFpsRangeType>()), default, default);

            MmalCheck(MmalPort.GetParameter(port.Ptr, &str.Hdr), "Unable to get framerate range for port.");

            return str;
        }

        internal static unsafe void SetFramerateRange(this IPort port, MmalRational fpsLow, MmalRational fpsHigh)
        {
            var str = new MmalParameterFpsRangeType(
                    new MmalParameterHeaderType(
                        MmalParametersCamera.MmalParameterFpsRange,
                        Marshal.SizeOf<MmalParameterFpsRangeType>()), fpsLow, fpsHigh);

            MmalCheck(MmalPort.SetParameter(port.Ptr, &str.Hdr), "Unable to set framerate range for port.");
        }
    }
}