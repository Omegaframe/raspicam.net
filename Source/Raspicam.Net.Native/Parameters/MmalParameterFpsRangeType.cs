using System.Runtime.InteropServices;
using Raspicam.Net.Native.Util;

namespace Raspicam.Net.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterFpsRangeType
    {
        public MmalParameterHeaderType Hdr;
        public MmalRational FpsLow;
        public MmalRational FpsHigh;

        public MmalParameterFpsRangeType(MmalParameterHeaderType hdr, MmalRational fpsLow, MmalRational fpsHigh)
        {
            Hdr = hdr;
            FpsLow = fpsLow;
            FpsHigh = fpsHigh;
        }
    }
}