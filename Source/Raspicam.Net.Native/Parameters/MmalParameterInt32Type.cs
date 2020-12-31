using System.Runtime.InteropServices;

namespace MMALSharp.Native.Parameters
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