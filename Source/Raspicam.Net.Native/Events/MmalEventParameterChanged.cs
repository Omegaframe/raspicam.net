using System.Runtime.InteropServices;
using Raspicam.Net.Native.Parameters;

namespace Raspicam.Net.Native.Events
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
