using System.Runtime.InteropServices;
using Raspicam.Net.Native.Util;

namespace Raspicam.Net.Native.Format
{
    public static class MmalFormat
    {
        public enum MmalEsTypeT
        {
            MmalEsTypeUnknown,
            MmalEsTypeControl,
            MmalEsTypeAudio,
            MmalEsTypeVideo,
            MmalEsTypeSubpicture
        }

        [DllImport("libmmal.so", EntryPoint = "mmal_format_extradata_alloc", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum AllocExtradata(MmalEsFormat* format, uint extradata_size);

        [DllImport("libmmal.so", EntryPoint = "mmal_format_copy", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void CopyFormat(MmalEsFormat* fmt_dst, MmalEsFormat* fmt_src);

        [DllImport("libmmal.so", EntryPoint = "mmal_format_full_copy", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum CopyFull(MmalEsFormat* fmt_dst, MmalEsFormat* fmt_src);
    }
}