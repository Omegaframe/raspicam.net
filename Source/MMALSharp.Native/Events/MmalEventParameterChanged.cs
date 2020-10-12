using System.Runtime.InteropServices;
using MMALSharp.Native.Parameters;

namespace MMALSharp.Native.Events
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalEventParameterChanged
    {
        public MmalParameterHeaderType Hdr;

        public MmalEventParameterChanged(MmalParameterHeaderType hdr)
        {
            Hdr = hdr;
        }
    }
}
