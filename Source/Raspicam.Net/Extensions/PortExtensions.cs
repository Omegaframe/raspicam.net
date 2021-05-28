using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Raspicam.Net.Config;
using Raspicam.Net.Mmal;
using Raspicam.Net.Mmal.Ports;
using Raspicam.Net.Native.Parameters;
using Raspicam.Net.Native.Port;
using Raspicam.Net.Native.Util;
using Raspicam.Net.Utility;
using static Raspicam.Net.MmalNativeExceptionHelper;
using static Raspicam.Net.Native.Parameters.MmalParametersCamera;

namespace Raspicam.Net.Extensions
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

        internal static unsafe void SetStereoMode(this IPort port, StereoMode mode)
        {
            var stereo = new MmalParameterStereoscopicModeType(
                new MmalParameterHeaderType(MmalParameterStereoscopicMode, Marshal.SizeOf<MmalParameterStereoscopicModeType>()),
                mode.Mode,
                mode.Decimate,
                mode.SwapEyes);

            MmalCheck(MmalPort.SetParameter(port.Ptr, &stereo.Hdr), "Unable to set Stereo mode");
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