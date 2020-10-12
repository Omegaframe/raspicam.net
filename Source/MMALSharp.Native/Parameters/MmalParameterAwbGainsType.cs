using System.Runtime.InteropServices;
using MMALSharp.Native.Util;

namespace MMALSharp.Native.Parameters
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