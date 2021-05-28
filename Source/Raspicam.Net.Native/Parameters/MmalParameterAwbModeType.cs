using System.Runtime.InteropServices;

namespace Raspicam.Net.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterAwbModeType
    {
        public MmalParameterHeaderType Hdr;
        public MmalParamAwbmodeType Value;

        public MmalParameterAwbModeType(MmalParameterHeaderType hdr, MmalParamAwbmodeType value)
        {
            Hdr = hdr;
            Value = value;
        }
    }
}