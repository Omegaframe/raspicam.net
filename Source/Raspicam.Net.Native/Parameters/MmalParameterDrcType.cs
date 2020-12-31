using System.Runtime.InteropServices;

namespace MMALSharp.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterDrcType
    {
        public MmalParameterHeaderType Hdr;
        public MmalParameterDrcStrengthType Strength;

        public MmalParameterDrcType(MmalParameterHeaderType hdr, MmalParameterDrcStrengthType strength)
        {
            Hdr = hdr;
            Strength = strength;
        }
    }
}