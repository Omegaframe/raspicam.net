using System.Runtime.InteropServices;
using Raspicam.Net.Native.Util;

namespace Raspicam.Net.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalDisplayRegionType
    {
        public MmalParameterHeaderType Hdr;
        public uint Set;
        public uint DisplayNum;
        public int Fullscreen;
        public MmalParametersVideo.MmalDisplaytransformT Transform;
        public MmalRect DestRect;
        public MmalRect SrcRect;
        public int NoAspect;
        public MmalParametersVideo.MmalDisplaymodeT Mode;
        public int PixelX;
        public int PixelY;
        public int Layer;
        public int CopyrightRequired;
        public int Alpha;

        public MmalDisplayRegionType(MmalParameterHeaderType hdr, uint set, uint displayNum, int fullscreen,
            MmalParametersVideo.MmalDisplaytransformT transform, MmalRect destRect, MmalRect srcRect,
            int noAspect, MmalParametersVideo.MmalDisplaymodeT mode, int pixelX, int pixelY,
            int layer, int copyrightRequired, int alpha)
        {
            Hdr = hdr;
            Set = set;
            DisplayNum = displayNum;
            Fullscreen = fullscreen;
            Transform = transform;
            DestRect = destRect;
            SrcRect = srcRect;
            NoAspect = noAspect;
            Mode = mode;
            PixelX = pixelX;
            PixelY = pixelY;
            Layer = layer;
            CopyrightRequired = copyrightRequired;
            Alpha = alpha;
        }
    }
}