using System.Runtime.InteropServices;
using MMALSharp.Native.Util;

namespace MMALSharp.Native.Parameters
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