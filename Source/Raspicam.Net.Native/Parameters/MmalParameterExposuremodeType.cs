using System.Runtime.InteropServices;

namespace Raspicam.Net.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterExposuremodeType
    {
        public MmalParameterHeaderType Hdr;
        public MmalParamExposuremodeType Value;

        public MmalParameterExposuremodeType(MmalParameterHeaderType hdr, MmalParamExposuremodeType value)
        {
            Hdr = hdr;
            Value = value;
        }
    }
}