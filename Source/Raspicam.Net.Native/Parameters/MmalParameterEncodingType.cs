using System.Runtime.InteropServices;

namespace MMALSharp.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterEncodingType
    {
        public MmalParameterHeaderType Hdr;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public int[] Value;

        public MmalParameterEncodingType(MmalParameterHeaderType hdr, int[] value)
        {
            Hdr = hdr;
            Value = value;
        }
    }
}