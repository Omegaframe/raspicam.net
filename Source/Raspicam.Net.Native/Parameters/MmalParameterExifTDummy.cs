using System.Runtime.InteropServices;

namespace Raspicam.Net.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MmalParameterExifTDummy
    {
        public MmalParameterHeaderType Hdr;
        public int Keylen;
        public int ValueOffset;
        public int ValueLen;
        public byte Data;

        public MmalParameterExifTDummy(MmalParameterHeaderType hdr, int keylen, int valueOffset, int valueLen, byte data)
        {
            Hdr = hdr;
            Keylen = keylen;
            ValueOffset = valueOffset;
            ValueLen = valueLen;
            Data = data;
        }
    }
}