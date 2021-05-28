using System.Runtime.InteropServices;

namespace Raspicam.Net.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterStringType
    {
        public MmalParameterHeaderType Hdr;
        public string Value;

        public MmalParameterStringType(MmalParameterHeaderType hdr, string value)
        {
            Hdr = hdr;
            Value = value;
        }
    }
}