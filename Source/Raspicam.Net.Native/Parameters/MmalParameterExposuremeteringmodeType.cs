using System.Runtime.InteropServices;

namespace Raspicam.Net.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterExposuremeteringmodeType
    {
        public MmalParameterHeaderType Hdr;
        public MmalParamExposuremeteringmodeType Value;

        public MmalParameterExposuremeteringmodeType(MmalParameterHeaderType hdr, MmalParamExposuremeteringmodeType value)
        {
            Hdr = hdr;
            Value = value;
        }
    }
}