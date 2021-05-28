using System.Runtime.InteropServices;
using Raspicam.Net.Native.Util;

namespace Raspicam.Net.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterAwbGainsType
    {
        public MmalParameterHeaderType Hdr;
        public MmalRational RGain;
        public MmalRational BGain;

        public MmalParameterAwbGainsType(MmalParameterHeaderType hdr, MmalRational rGain, MmalRational bGain)
        {
            Hdr = hdr;
            RGain = rGain;
            BGain = bGain;
        }
    }
}