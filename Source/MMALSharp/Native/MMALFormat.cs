using System;
using System.Runtime.InteropServices;

namespace MMALSharp.Native
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

        public static int MmalEsFormatFlagFramed = 0x1;

        public static int MmalEsFormatCompareFlagType = 0x01;
        public static int MmalEsFormatCompareFlagEncoding = 0x02;
        public static int MmalEsFormatCompareFlagBitrate = 0x04;
        public static int MmalEsFormatCompareFlagFlags = 0x08;
        public static int MmalEsFormatCompareFlagExtradata = 0x10;
        public static int MmalEsFormatCompareFlagVideoResolution = 0x0100;
        public static int MmalEsFormatCompareFlagVideoCropping = 0x0200;
        public static int MmalEsFormatCompareFlagVideoFrameRate = 0x0400;
        public static int MmalEsFormatCompareFlagVideoAspectRatio = 0x0800;
        public static int MmalEsFormatCompareFlagVideoColorSpace = 0x1000;

        public static int MmalEsFormatCompareFlagEsOther = 0x10000000;

        [DllImport("libmmal.so", EntryPoint = "mmal_format_alloc", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MMAL_ES_FORMAT_T* mmal_format_alloc();

        [DllImport("libmmal.so", EntryPoint = "mmal_format_free", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void mmal_format_free(MMAL_ES_FORMAT_T* format);

        [DllImport("libmmal.so", EntryPoint = "mmal_format_extradata_alloc", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalUtil.MmalStatusT mmal_format_extradata_alloc(MMAL_ES_FORMAT_T* format, uint extradata_size);

        [DllImport("libmmal.so", EntryPoint = "mmal_format_copy", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void mmal_format_copy(MMAL_ES_FORMAT_T* fmt_dst, MMAL_ES_FORMAT_T* fmt_src);

        [DllImport("libmmal.so", EntryPoint = "mmal_format_full_copy", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalUtil.MmalStatusT mmal_format_full_copy(MMAL_ES_FORMAT_T* fmt_dst, MMAL_ES_FORMAT_T* fmt_src);

        [DllImport("libmmal.so", EntryPoint = "mmal_format_compare", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe uint mmal_format_compare(MMAL_ES_FORMAT_T* ptr, MMAL_ES_FORMAT_T* ptr2);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_VIDEO_FORMAT_T
    {
        public int Width, Height;
        public MMAL_RECT_T Crop;
        public MMAL_RATIONAL_T FrameRate, Par;
        public int ColorSpace;

        public MMAL_VIDEO_FORMAT_T(int width, int height, MMAL_RECT_T crop, MMAL_RATIONAL_T frameRate,
                                    MMAL_RATIONAL_T par, int colorSpace)
        {
            this.Width = width;
            this.Height = height;
            this.Crop = crop;
            this.FrameRate = frameRate;
            this.Par = par;
            this.ColorSpace = colorSpace;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_AUDIO_FORMAT_T
    {
        uint channels, sampleRate, bitsPerSample, blockAlign;

        public uint Channels => channels;
        public uint SampleRate => sampleRate;
        public uint BitsPerSample => bitsPerSample;
        public uint BlockAlign => blockAlign;

        public MMAL_AUDIO_FORMAT_T(uint channels, uint sampleRate, uint bitsPerSample, uint blockAlign)
        {
            this.channels = channels;
            this.sampleRate = sampleRate;
            this.bitsPerSample = bitsPerSample;
            this.blockAlign = blockAlign;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_SUBPICTURE_FORMAT_T
    {
        uint xOffset, yOffset;

        public uint XOffset => xOffset;
        public uint YOffset => yOffset;

        public MMAL_SUBPICTURE_FORMAT_T(uint xOffset, uint yOffset)
        {
            this.xOffset = xOffset;
            this.yOffset = yOffset;
        }
    }

    // Union type.
    [StructLayout(LayoutKind.Explicit)]
    public struct MMAL_ES_SPECIFIC_FORMAT_T
    {
        [FieldOffset(0)]
        public MMAL_AUDIO_FORMAT_T Audio;
        [FieldOffset(0)]
        public MMAL_VIDEO_FORMAT_T Video;
        [FieldOffset(0)]
        public MMAL_SUBPICTURE_FORMAT_T Subpicture;

        public MMAL_ES_SPECIFIC_FORMAT_T(MMAL_AUDIO_FORMAT_T audio, MMAL_VIDEO_FORMAT_T video, MMAL_SUBPICTURE_FORMAT_T subpicture)
        {
            this.Audio = audio;
            this.Video = video;
            this.Subpicture = subpicture;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MMAL_ES_FORMAT_T
    {
        public MmalFormat.MmalEsTypeT Type;
        public int Encoding, EncodingVariant;
        public MMAL_ES_SPECIFIC_FORMAT_T* Es;
        public int Bitrate, Flags, ExtraDataSize;

        // byte*
        public IntPtr ExtraData;

        public MMAL_ES_FORMAT_T(MmalFormat.MmalEsTypeT type, int encoding, int encodingVariant,
                                MMAL_ES_SPECIFIC_FORMAT_T* es, int bitrate, int flags, int extraDataSize,
                                IntPtr extraData)
        {
            this.Type = type;
            this.Encoding = encoding;
            this.EncodingVariant = encodingVariant;
            this.Es = es;
            this.Bitrate = bitrate;
            this.Flags = flags;
            this.ExtraDataSize = extraDataSize;
            this.ExtraData = extraData;
        }
    }
}