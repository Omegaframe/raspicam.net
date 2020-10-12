using System.Runtime.InteropServices;

namespace MMALSharp.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterUint64Type
    {
        public MmalParameterHeaderType Hdr;
        public ulong Value;

        public MmalParameterUint64Type(MmalParameterHeaderType hdr, ulong value)
        {
            Hdr = hdr;
            Value = value;
        }
    }
}