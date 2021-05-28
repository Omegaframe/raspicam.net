using System.Runtime.InteropServices;
using Raspicam.Net.Native.Util;

namespace Raspicam.Net.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterInputCropType
    {
        public MmalParameterHeaderType Hdr;
        public MmalRect Rect;

        public MmalParameterInputCropType(MmalParameterHeaderType hdr, MmalRect rect)
        {
            Hdr = hdr;
            Rect = rect;
        }
    }
}