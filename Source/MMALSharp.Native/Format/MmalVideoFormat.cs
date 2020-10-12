using System.Runtime.InteropServices;

namespace MMALSharp.Native.Format
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalVideoFormat
    {
        public int Width, Height;
        public MMAL_RECT_T Crop;
        public MMAL_RATIONAL_T FrameRate, Par;
        public int ColorSpace;

        public MmalVideoFormat(int width, int height, MMAL_RECT_T crop, MMAL_RATIONAL_T frameRate,
            MMAL_RATIONAL_T par, int colorSpace)
        {
            Width = width;
            Height = height;
            Crop = crop;
            FrameRate = frameRate;
            Par = par;
            ColorSpace = colorSpace;
        }
    }
}
