using System.Runtime.InteropServices;
using MMALSharp.Native.Util;

namespace MMALSharp.Native.Parameters
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