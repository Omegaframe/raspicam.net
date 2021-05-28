using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Raspicam.Net.Utility;

namespace Raspicam.Net.Mmal
{
    public static class MmalEncodingHelpers
    {
        static IReadOnlyCollection<MmalEncoding> EncodingList { get; } = new ReadOnlyCollection<MmalEncoding>(new List<MmalEncoding>
        {
            MmalEncoding.H264,
            MmalEncoding.Mvc,
            MmalEncoding.H263,
            MmalEncoding.Mp4V,
            MmalEncoding.Mp2V,
            MmalEncoding.Mp1V,
            MmalEncoding.Wmv3,
            MmalEncoding.Wmv2,
            MmalEncoding.Wmv1,
            MmalEncoding.Wvc1,
            MmalEncoding.Vp8,
            MmalEncoding.Vp7,
            MmalEncoding.Vp6,
            MmalEncoding.Theora,
            MmalEncoding.Spark,
            MmalEncoding.MJpeg,
            MmalEncoding.Jpeg,
            MmalEncoding.Gif,
            MmalEncoding.Png,
            MmalEncoding.Ppm,
            MmalEncoding.Tga,
            MmalEncoding.Bmp,
            MmalEncoding.I420,
            MmalEncoding.I420Slice,
            MmalEncoding.Yv12,
            MmalEncoding.I422,
            MmalEncoding.I422Slice,
            MmalEncoding.YUyv,
            MmalEncoding.YVyu,
            MmalEncoding.UYvy,
            MmalEncoding.VYuy,
            MmalEncoding.Nv12,
            MmalEncoding.Nv21,
            MmalEncoding.ARgb,
            MmalEncoding.Rgba,
            MmalEncoding.ABgr,
            MmalEncoding.BGra,
            MmalEncoding.Rgb16,
            MmalEncoding.Rgb24,
            MmalEncoding.Rgb32,
            MmalEncoding.Bgr16,
            MmalEncoding.Bgr24,
            MmalEncoding.Bgr32,
            MmalEncoding.BayerSbggr10P,
            MmalEncoding.BayerSbggr8,
            MmalEncoding.BayerSbggr12P,
            MmalEncoding.BayerSbggr16,
            MmalEncoding.BayerSbggr10Dpcm8,
            MmalEncoding.Yuvuv128,
            MmalEncoding.Yuv10Col,
            MmalEncoding.Opaque,
            MmalEncoding.EglImage,
            MmalEncoding.PcmUnsignedBe,
            MmalEncoding.PcmUnsignedLe,
            MmalEncoding.PcmSignedBe,
            MmalEncoding.PcmSignedLe,
            MmalEncoding.PcmFloatBe,
            MmalEncoding.PcmFloatLe,
            MmalEncoding.PcmUnsigned,
            MmalEncoding.PcmSigned,
            MmalEncoding.PcmFloat,
            MmalEncoding.Mp4A,
            MmalEncoding.MpgA,
            MmalEncoding.Alaw,
            MmalEncoding.Mulaw,
            MmalEncoding.AdpcmMs,
            MmalEncoding.AdpcmImaMs,
            MmalEncoding.AdpcmSwf,
            MmalEncoding.Wma1,
            MmalEncoding.Wma2,
            MmalEncoding.Wmap,
            MmalEncoding.Wmal,
            MmalEncoding.Wmav,
            MmalEncoding.Amrnb,
            MmalEncoding.Amrwb,
            MmalEncoding.Amrwbp,
            MmalEncoding.Ac3,
            MmalEncoding.Eac3,
            MmalEncoding.Dts,
            MmalEncoding.Mlp,
            MmalEncoding.Flac,
            MmalEncoding.Vorbis,
            MmalEncoding.Speex,
            MmalEncoding.Atrac3,
            MmalEncoding.Atracx,
            MmalEncoding.Atracl,
            MmalEncoding.Midi,
            MmalEncoding.Evrc,
            MmalEncoding.Nellymoser,
            MmalEncoding.Qcelp,
            MmalEncoding.Mp4VDivxDrm,
            MmalEncoding.VariantH264Default,
            MmalEncoding.VariantH264Avc1,
            MmalEncoding.VariantH264Raw,
            MmalEncoding.VariantMp4ADefault,
            MmalEncoding.VariantMp4AAdts,
            MmalEncoding.MmalColorSpaceUnknown,
            MmalEncoding.MmalColorSpaceIturBt601,
            MmalEncoding.MmalColorSpaceIturBt709,
            MmalEncoding.MmalColorSpaceJpegJfif,
            MmalEncoding.MmalColorSpaceFcc,
            MmalEncoding.MmalColorSpaceSmpte240M,
            MmalEncoding.MmalColorSpaceBt4702M,
            MmalEncoding.MmalColorSpaceBt4702Bg,
            MmalEncoding.MmalColorSpaceJfifY16255,
            MmalEncoding.MmalColorSpaceRec2020
        });

        public static MmalEncoding ParseEncoding(this int encodingType) => EncodingList.FirstOrDefault(c => c.EncodingVal == encodingType);
    }

    public class MmalEncoding
    {
        public enum EncodingType
        {
            Image,
            Video,
            Audio,
            PixelFormat,
            ColorSpace,
            Internal
        }

        public int EncodingVal { get; }
        public string EncodingName { get; }
        public EncodingType EncType { get; }

        public override string ToString()
        {
            var type = EncType switch
            {
                EncodingType.Audio => "Audio",
                EncodingType.Image => "Image",
                EncodingType.Internal => "Internal",
                EncodingType.PixelFormat => "Pixel Format",
                EncodingType.Video => "Video",
                _ => string.Empty
            };

            return $"Name: {EncodingName}. FourCC: {EncodingVal}. Type: {type}";
        }

        MmalEncoding(string s, EncodingType type)
        {
            EncodingVal = Helpers.FourCcFromString(s);
            EncodingName = s;
            EncType = type;
        }

        MmalEncoding(int val, string name, EncodingType type)
        {
            EncodingVal = val;
            EncodingName = name;
            EncType = type;
        }

        public static readonly MmalEncoding H264 = new MmalEncoding("H264", EncodingType.Video);
        public static readonly MmalEncoding Mvc = new MmalEncoding("MVC ", EncodingType.Video);
        public static readonly MmalEncoding H263 = new MmalEncoding("H263", EncodingType.Video);
        public static readonly MmalEncoding Mp4V = new MmalEncoding("MP4V", EncodingType.Video);
        public static readonly MmalEncoding Mp2V = new MmalEncoding("MP2V", EncodingType.Video);
        public static readonly MmalEncoding Mp1V = new MmalEncoding("MP1V", EncodingType.Video);
        public static readonly MmalEncoding Wmv3 = new MmalEncoding("WMV3", EncodingType.Video);
        public static readonly MmalEncoding Wmv2 = new MmalEncoding("WMV2", EncodingType.Video);
        public static readonly MmalEncoding Wmv1 = new MmalEncoding("WMV1", EncodingType.Video);
        public static readonly MmalEncoding Wvc1 = new MmalEncoding("WVC1", EncodingType.Video);
        public static readonly MmalEncoding Vp8 = new MmalEncoding("VP8 ", EncodingType.Video);
        public static readonly MmalEncoding Vp7 = new MmalEncoding("VP7 ", EncodingType.Video);
        public static readonly MmalEncoding Vp6 = new MmalEncoding("VP6 ", EncodingType.Video);
        public static readonly MmalEncoding Theora = new MmalEncoding("THEO", EncodingType.Video);
        public static readonly MmalEncoding Spark = new MmalEncoding("SPRK", EncodingType.Video);
        public static readonly MmalEncoding MJpeg = new MmalEncoding("MJPG", EncodingType.Video);
        public static readonly MmalEncoding Jpeg = new MmalEncoding("JPEG", EncodingType.Image);
        public static readonly MmalEncoding Gif = new MmalEncoding("GIF ", EncodingType.Image);
        public static readonly MmalEncoding Png = new MmalEncoding("PNG ", EncodingType.Image);
        public static readonly MmalEncoding Ppm = new MmalEncoding("PPM ", EncodingType.Image);
        public static readonly MmalEncoding Tga = new MmalEncoding("TGA ", EncodingType.Image);
        public static readonly MmalEncoding Bmp = new MmalEncoding("BMP ", EncodingType.Image);
        public static readonly MmalEncoding I420 = new MmalEncoding("I420", EncodingType.PixelFormat);
        public static readonly MmalEncoding I420Slice = new MmalEncoding("S420", EncodingType.PixelFormat);
        public static readonly MmalEncoding Yv12 = new MmalEncoding("YV12", EncodingType.PixelFormat);
        public static readonly MmalEncoding I422 = new MmalEncoding("I422", EncodingType.PixelFormat);
        public static readonly MmalEncoding I422Slice = new MmalEncoding("S422", EncodingType.PixelFormat);
        public static readonly MmalEncoding YUyv = new MmalEncoding("YUYV", EncodingType.PixelFormat);
        public static readonly MmalEncoding YVyu = new MmalEncoding("YVYU", EncodingType.PixelFormat);
        public static readonly MmalEncoding UYvy = new MmalEncoding("UYVY", EncodingType.PixelFormat);
        public static readonly MmalEncoding VYuy = new MmalEncoding("VYUY", EncodingType.PixelFormat);
        public static readonly MmalEncoding Nv12 = new MmalEncoding("NV12", EncodingType.PixelFormat);
        public static readonly MmalEncoding Nv21 = new MmalEncoding("NV21", EncodingType.PixelFormat);
        public static readonly MmalEncoding ARgb = new MmalEncoding("ARGB", EncodingType.PixelFormat);
        public static readonly MmalEncoding Rgba = new MmalEncoding("RGBA", EncodingType.PixelFormat);
        public static readonly MmalEncoding ABgr = new MmalEncoding("ABGR", EncodingType.PixelFormat);
        public static readonly MmalEncoding BGra = new MmalEncoding("BGRA", EncodingType.PixelFormat);
        public static readonly MmalEncoding Rgb16 = new MmalEncoding("RGB2", EncodingType.PixelFormat);
        public static readonly MmalEncoding Rgb24 = new MmalEncoding("RGB3", EncodingType.PixelFormat);
        public static readonly MmalEncoding Rgb32 = new MmalEncoding("RGB4", EncodingType.PixelFormat);
        public static readonly MmalEncoding Bgr16 = new MmalEncoding("BGR2", EncodingType.PixelFormat);
        public static readonly MmalEncoding Bgr24 = new MmalEncoding("BGR3", EncodingType.PixelFormat);
        public static readonly MmalEncoding Bgr32 = new MmalEncoding("BGR4", EncodingType.PixelFormat);
        public static readonly MmalEncoding BayerSbggr10P = new MmalEncoding("pBAA", EncodingType.PixelFormat);
        public static readonly MmalEncoding BayerSbggr8 = new MmalEncoding("BA81", EncodingType.PixelFormat);
        public static readonly MmalEncoding BayerSbggr12P = new MmalEncoding("BY12", EncodingType.PixelFormat);
        public static readonly MmalEncoding BayerSbggr16 = new MmalEncoding("BYR2", EncodingType.PixelFormat);
        public static readonly MmalEncoding BayerSbggr10Dpcm8 = new MmalEncoding("bBA8", EncodingType.PixelFormat);
        public static readonly MmalEncoding Yuvuv128 = new MmalEncoding("SAND", EncodingType.PixelFormat);
        public static readonly MmalEncoding Yuv10Col = new MmalEncoding("Y10C", EncodingType.PixelFormat);

        public static readonly MmalEncoding Opaque = new MmalEncoding("OPQV", EncodingType.Internal);
        public static readonly MmalEncoding EglImage = new MmalEncoding("EGLI", EncodingType.PixelFormat);
        public static readonly MmalEncoding PcmUnsignedBe = new MmalEncoding("PCMU", EncodingType.PixelFormat);
        public static readonly MmalEncoding PcmUnsignedLe = new MmalEncoding("pcmu", EncodingType.PixelFormat);
        public static readonly MmalEncoding PcmSignedBe = new MmalEncoding("PCMS", EncodingType.PixelFormat);
        public static readonly MmalEncoding PcmSignedLe = new MmalEncoding("pcms", EncodingType.PixelFormat);
        public static readonly MmalEncoding PcmFloatBe = new MmalEncoding("PCMF", EncodingType.PixelFormat);
        public static readonly MmalEncoding PcmFloatLe = new MmalEncoding("pcmf", EncodingType.PixelFormat);
        public static readonly MmalEncoding PcmUnsigned = PcmUnsignedLe;
        public static readonly MmalEncoding PcmSigned = PcmSignedLe;
        public static readonly MmalEncoding PcmFloat = PcmFloatLe;
        public static readonly MmalEncoding Mp4A = new MmalEncoding("MP4A", EncodingType.Audio);
        public static readonly MmalEncoding MpgA = new MmalEncoding("MPGA", EncodingType.Audio);
        public static readonly MmalEncoding Alaw = new MmalEncoding("ALAW", EncodingType.Audio);
        public static readonly MmalEncoding Mulaw = new MmalEncoding("ULAW", EncodingType.Audio);
        public static readonly MmalEncoding AdpcmMs = new MmalEncoding("MS\x00\x02", EncodingType.Audio);
        public static readonly MmalEncoding AdpcmImaMs = new MmalEncoding("MS\x00\x01", EncodingType.Audio);
        public static readonly MmalEncoding AdpcmSwf = new MmalEncoding("ASWF", EncodingType.Audio);
        public static readonly MmalEncoding Wma1 = new MmalEncoding("WMA1", EncodingType.Audio);
        public static readonly MmalEncoding Wma2 = new MmalEncoding("WMA2", EncodingType.Audio);
        public static readonly MmalEncoding Wmap = new MmalEncoding("WMAP", EncodingType.Audio);
        public static readonly MmalEncoding Wmal = new MmalEncoding("WMAL", EncodingType.Audio);
        public static readonly MmalEncoding Wmav = new MmalEncoding("WMAV", EncodingType.Audio);
        public static readonly MmalEncoding Amrnb = new MmalEncoding("AMRN", EncodingType.Audio);
        public static readonly MmalEncoding Amrwb = new MmalEncoding("AMRW", EncodingType.Audio);
        public static readonly MmalEncoding Amrwbp = new MmalEncoding("AMRP", EncodingType.Audio);
        public static readonly MmalEncoding Ac3 = new MmalEncoding("AC3 ", EncodingType.Audio);
        public static readonly MmalEncoding Eac3 = new MmalEncoding("EAC3", EncodingType.Audio);
        public static readonly MmalEncoding Dts = new MmalEncoding("DTS ", EncodingType.Audio);
        public static readonly MmalEncoding Mlp = new MmalEncoding("MLP ", EncodingType.Audio);
        public static readonly MmalEncoding Flac = new MmalEncoding("FLAC", EncodingType.Audio);
        public static readonly MmalEncoding Vorbis = new MmalEncoding("VORB", EncodingType.Audio);
        public static readonly MmalEncoding Speex = new MmalEncoding("SPX ", EncodingType.Audio);
        public static readonly MmalEncoding Atrac3 = new MmalEncoding("ATR3", EncodingType.Audio);
        public static readonly MmalEncoding Atracx = new MmalEncoding("ATRX", EncodingType.Audio);
        public static readonly MmalEncoding Atracl = new MmalEncoding("ATRL", EncodingType.Audio);
        public static readonly MmalEncoding Midi = new MmalEncoding("MIDI", EncodingType.Audio);
        public static readonly MmalEncoding Evrc = new MmalEncoding("EVRC", EncodingType.Audio);
        public static readonly MmalEncoding Nellymoser = new MmalEncoding("NELY", EncodingType.Audio);
        public static readonly MmalEncoding Qcelp = new MmalEncoding("QCEL", EncodingType.Audio);
        public static readonly MmalEncoding Mp4VDivxDrm = new MmalEncoding("M4VD", EncodingType.Video);
        public static readonly MmalEncoding VariantH264Default = new MmalEncoding(0, "VARIANT_H264_DEFAULT", EncodingType.Video);
        public static readonly MmalEncoding VariantH264Avc1 = new MmalEncoding("AVC1", EncodingType.Video);
        public static readonly MmalEncoding VariantH264Raw = new MmalEncoding("RAW ", EncodingType.Video);
        public static readonly MmalEncoding VariantMp4ADefault = new MmalEncoding(0, "VARIANT_MP4A_DEFAULT", EncodingType.Video);
        public static readonly MmalEncoding VariantMp4AAdts = new MmalEncoding("ADTS", EncodingType.Video);
        public static readonly MmalEncoding MmalColorSpaceUnknown = new MmalEncoding(0, "MMAL_COLOR_SPACE_UNKNOWN", EncodingType.ColorSpace);
        public static readonly MmalEncoding MmalColorSpaceIturBt601 = new MmalEncoding("Y601", EncodingType.ColorSpace);
        public static readonly MmalEncoding MmalColorSpaceIturBt709 = new MmalEncoding("Y709", EncodingType.ColorSpace);
        public static readonly MmalEncoding MmalColorSpaceJpegJfif = new MmalEncoding("YJFI", EncodingType.ColorSpace);
        public static readonly MmalEncoding MmalColorSpaceFcc = new MmalEncoding("YFCC", EncodingType.ColorSpace);
        public static readonly MmalEncoding MmalColorSpaceSmpte240M = new MmalEncoding("Y240", EncodingType.ColorSpace);
        public static readonly MmalEncoding MmalColorSpaceBt4702M = new MmalEncoding("Y__M", EncodingType.ColorSpace);
        public static readonly MmalEncoding MmalColorSpaceBt4702Bg = new MmalEncoding("Y_BG", EncodingType.ColorSpace);
        public static readonly MmalEncoding MmalColorSpaceJfifY16255 = new MmalEncoding("YY16", EncodingType.ColorSpace);
        public static readonly MmalEncoding MmalColorSpaceRec2020 = new MmalEncoding("2020", EncodingType.ColorSpace);
    }
}
