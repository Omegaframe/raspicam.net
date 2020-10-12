using System.Runtime.InteropServices;

namespace MMALSharp.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MmalParameterExifType
    {
        public MmalParameterHeaderType Hdr;
        public int Keylen;
        public int ValueOffset;
        public int ValueLen;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public byte[] Data;

        public MmalParameterExifType(MmalParameterHeaderType hdr, int keylen, int valueOffset, int valueLen, byte[] data)
        {
            Hdr = hdr;
            Keylen = keylen;
            ValueOffset = valueOffset;
            ValueLen = valueLen;
            Data = data;
        }
    }
}