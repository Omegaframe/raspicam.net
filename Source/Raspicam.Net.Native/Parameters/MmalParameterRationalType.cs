using System.Runtime.InteropServices;
using Raspicam.Net.Native.Util;

namespace Raspicam.Net.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterRationalType
    {
        public MmalParameterHeaderType Hdr;
        public MmalRational Value;

        public MmalParameterRationalType(MmalParameterHeaderType hdr, MmalRational value)
        {
            Hdr = hdr;
            Value = value;
        }
    }
}