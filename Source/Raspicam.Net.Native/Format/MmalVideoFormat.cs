using System.Runtime.InteropServices;
using Raspicam.Net.Native.Util;

namespace Raspicam.Net.Native.Format
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalVideoFormat
    {
        public int Width, Height;
        public MmalRect Crop;
        public MmalRational FrameRate, Par;
        public int ColorSpace;

        public MmalVideoFormat(int width, int height, MmalRect crop, MmalRational frameRate,
            MmalRational par, int colorSpace)
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
