using System.Runtime.InteropServices;

namespace MMALSharp.Native.Format
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
        public static extern unsafe MmalUtil.MmalStatusT AllocExtradata(MmalEsFormat* format, uint extradata_size);

        [DllImport("libmmal.so", EntryPoint = "mmal_format_copy", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void CopyFormat(MmalEsFormat* fmt_dst, MmalEsFormat* fmt_src);

        [DllImport("libmmal.so", EntryPoint = "mmal_format_full_copy", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalUtil.MmalStatusT CopyFull(MmalEsFormat* fmt_dst, MmalEsFormat* fmt_src);
    }
}