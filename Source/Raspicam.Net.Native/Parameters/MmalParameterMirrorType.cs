using System.Runtime.InteropServices;

namespace Raspicam.Net.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterMirrorType
    {
        public MmalParameterHeaderType Hdr;
        public MmalParamMirrorType Value;

        public MmalParameterMirrorType(MmalParameterHeaderType hdr, MmalParamMirrorType value)
        {
            Hdr = hdr;
            Value = value;
        }
    }
}