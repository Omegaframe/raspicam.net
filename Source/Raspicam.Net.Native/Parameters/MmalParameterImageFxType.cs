using System.Runtime.InteropServices;

namespace MMALSharp.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterImageFxType
    {
        public MmalParameterHeaderType Hdr;
        public MmalParamImagefxType Value;

        public MmalParameterImageFxType(MmalParameterHeaderType hdr, MmalParamImagefxType value)
        {
            Hdr = hdr;
            Value = value;
        }
    }
}