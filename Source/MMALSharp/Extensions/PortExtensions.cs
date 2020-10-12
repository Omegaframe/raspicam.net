using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using MMALSharp.Common;
using MMALSharp.Common.Utility;
using MMALSharp.Config;
using MMALSharp.Native;
using MMALSharp.Native.Port;
using MMALSharp.Ports;
using static MMALSharp.MmalNativeExceptionHelper;
using static MMALSharp.Native.MmalParametersCamera;

namespace MMALSharp.Extensions
{
    public static class PortExtensions
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
                    case nameof(MMAL_PARAMETER_BOOLEAN_T):
                        var boolVal = 0;
                        MmalCheck(MmalUtil.mmal_port_parameter_get_boolean(port.Ptr, (uint)key, ref boolVal), "Unable to get boolean value");
                        return boolVal == 1;
                    case nameof(MMAL_PARAMETER_UINT64_T):
                        var ulongVal = 0UL;
                        MmalCheck(MmalUtil.mmal_port_parameter_get_uint64(port.Ptr, (uint)key, ref ulongVal), "Unable to get ulong value");
                        return ulongVal;
                    case nameof(MMAL_PARAMETER_INT64_T):
                        var longVal = 0L;
                        MmalCheck(MmalUtil.mmal_port_parameter_get_int64(port.Ptr, (uint)key, ref longVal), "Unable to get long value");
                        return longVal;
                    case nameof(MMAL_PARAMETER_UINT32_T):
                        var uintVal = 0U;
                        MmalCheck(MmalUtil.mmal_port_parameter_get_uint32(port.Ptr, (uint)key, ref uintVal), "Unable to get uint value");
                        return uintVal;
                    case nameof(MMAL_PARAMETER_INT32_T):
                        var intVal = 0;
                        MmalCheck(MmalUtil.mmal_port_parameter_get_int32(port.Ptr, (uint)key, ref intVal), "Unable to get int value");
                        return intVal;
                    case nameof(MMAL_PARAMETER_RATIONAL_T):
                        var ratVal = default(MMAL_RATIONAL_T);
                        MmalCheck(MmalUtil.mmal_port_parameter_get_rational(port.Ptr, (uint)key, ref ratVal), "Unable to get rational value");
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
            var ptr1 = Marshal.AllocHGlobal(Marshal.SizeOf<MMAL_PARAMETER_ENCODING_T>() + 20);
            var str1 = (MMAL_PARAMETER_HEADER_T*)ptr1;

            str1->Id = MmalParametersCommon.MmalParameterSupportedEncodings;

            // Deliberately undersize to check if running on older firmware.
            str1->Size = Marshal.SizeOf<MMAL_PARAMETER_ENCODING_T>() + 20;

            try
            {
                MmalCheck(MmalPort.GetParameter(port.Ptr, str1), "Unable to get supported encodings");
                var encodings = (MMAL_PARAMETER_ENCODING_T)Marshal.PtrToStructure(ptr1, typeof(MMAL_PARAMETER_ENCODING_T));
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
                    case nameof(MMAL_PARAMETER_BOOLEAN_T):
                        int i = (bool)value ? 1 : 0;
                        MmalCheck(MmalUtil.mmal_port_parameter_set_boolean(port.Ptr, (uint)key, i), "Unable to set boolean value");
                        break;
                    case nameof(MMAL_PARAMETER_UINT64_T):
                        MmalCheck(MmalUtil.mmal_port_parameter_set_uint64(port.Ptr, (uint)key, (ulong)value), "Unable to set ulong value");
                        break;
                    case nameof(MMAL_PARAMETER_INT64_T):
                        MmalCheck(MmalUtil.mmal_port_parameter_set_int64(port.Ptr, (uint)key, (long)value), "Unable to set long value");
                        break;
                    case nameof(MMAL_PARAMETER_UINT32_T):
                        MmalCheck(MmalUtil.mmal_port_parameter_set_uint32(port.Ptr, (uint)key, (uint)value), "Unable to set uint value");
                        break;
                    case nameof(MMAL_PARAMETER_INT32_T):
                        MmalCheck(MmalUtil.mmal_port_parameter_set_int32(port.Ptr, (uint)key, (int)value), "Unable to set int value");
                        break;
                    case nameof(MMAL_PARAMETER_RATIONAL_T):
                        MmalCheck(MmalUtil.mmal_port_parameter_set_rational(port.Ptr, (uint)key, (MMAL_RATIONAL_T)value), "Unable to set rational value");
                        break;
                    case nameof(MMAL_PARAMETER_STRING_T):
                        MmalCheck(MmalUtil.mmal_port_parameter_set_string(port.Ptr, (uint)key, (string)value), "Unable to set rational value");
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
            var stereo = new MMAL_PARAMETER_STEREOSCOPIC_MODE_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterStereoscopicMode, Marshal.SizeOf<MMAL_PARAMETER_STEREOSCOPIC_MODE_T>()),
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
        
        public static unsafe MMAL_PARAMETER_FPS_RANGE_T GetFramerateRange(this IPort port)
        {
            var str = new MMAL_PARAMETER_FPS_RANGE_T(
                    new MMAL_PARAMETER_HEADER_T(
                        MmalParametersCamera.MmalParameterFpsRange,
                        Marshal.SizeOf<MMAL_PARAMETER_FPS_RANGE_T>()), default(MMAL_RATIONAL_T), default(MMAL_RATIONAL_T));

            MmalCheck(MmalPort.GetParameter(port.Ptr, &str.Hdr), "Unable to get framerate range for port.");

            return str;
        }

        internal static unsafe void SetFramerateRange(this IPort port, MMAL_RATIONAL_T fpsLow, MMAL_RATIONAL_T fpsHigh)
        {
            var str = new MMAL_PARAMETER_FPS_RANGE_T(
                    new MMAL_PARAMETER_HEADER_T(
                        MmalParametersCamera.MmalParameterFpsRange,
                        Marshal.SizeOf<MMAL_PARAMETER_FPS_RANGE_T>()), fpsLow, fpsHigh);

            MmalCheck(MmalPort.SetParameter(port.Ptr, &str.Hdr), "Unable to set framerate range for port.");
        }
    }
}