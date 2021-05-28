using System.Runtime.InteropServices;

namespace Raspicam.Net.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterBooleanType
    {
        public MmalParameterHeaderType Hdr;
        public int Value;

        public MmalParameterBooleanType(MmalParameterHeaderType hdr, int value)
        {
            Hdr = hdr;
            Value = value;
        }
    }
}