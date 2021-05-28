using System.Runtime.InteropServices;

namespace Raspicam.Net.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterChangeEventRequestType
    {
        public MmalParameterHeaderType Hdr;
        public int ChangeId;
        public int Enable;

        public MmalParameterChangeEventRequestType(MmalParameterHeaderType hdr, int changeId, int enable)
        {
            Hdr = hdr;
            ChangeId = changeId;
            Enable = enable;
        }
    }
}