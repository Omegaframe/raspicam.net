using System.Runtime.InteropServices;

namespace Raspicam.Net.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterInt64Type
    {
        public MmalParameterHeaderType Hdr;
        public long Value;

        public MmalParameterInt64Type(MmalParameterHeaderType hdr, long value)
        {
            Hdr = hdr;
            Value = value;
        }
    }
}