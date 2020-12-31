using System.Runtime.InteropServices;

namespace MMALSharp.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterThumbnailConfigType
    {
        public MmalParameterHeaderType Hdr;
        public int Enable;
        public int Width;
        public int Height;
        public int Quality;

        public MmalParameterThumbnailConfigType(MmalParameterHeaderType hdr, bool enable, int width, int height, int quality)
        {
            Hdr = hdr;

            Enable = enable ? 1 : 0;
            Width = width;
            Height = height;
            Quality = quality;
        }
    }
}