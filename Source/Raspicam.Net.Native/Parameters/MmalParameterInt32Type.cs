using System.Runtime.InteropServices;

namespace Raspicam.Net.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterInt32Type
    {
        public MmalParameterHeaderType Hdr;
        public int Value;

        public MmalParameterInt32Type(MmalParameterHeaderType hdr, int value)
        {
            Hdr = hdr;
            Value = value;
        }
    }
}