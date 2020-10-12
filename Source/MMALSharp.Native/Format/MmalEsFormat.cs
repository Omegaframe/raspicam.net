using System;
using System.Runtime.InteropServices;

namespace MMALSharp.Native.Format
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MmalEsFormat
    {
        public MmalFormat.MmalEsTypeT Type;
        public int Encoding, EncodingVariant;
        public MmalEsSpecificFormat* Es;
        public int Bitrate, Flags, ExtraDataSize;

        // byte*
        public IntPtr ExtraData;

        public MmalEsFormat(MmalFormat.MmalEsTypeT type, int encoding, int encodingVariant,
            MmalEsSpecificFormat* es, int bitrate, int flags, int extraDataSize,
            IntPtr extraData)
        {
            Type = type;
            Encoding = encoding;
            EncodingVariant = encodingVariant;
            Es = es;
            Bitrate = bitrate;
            Flags = flags;
            ExtraDataSize = extraDataSize;
            ExtraData = extraData;
        }
    }
}
