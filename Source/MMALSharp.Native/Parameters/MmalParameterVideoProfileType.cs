using System.Runtime.InteropServices;

namespace MMALSharp.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterVideoProfileType
    {
        public MmalParameterHeaderType Hdr;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public MmalParameterVideoProfileS[] Profile;

        public MmalParameterVideoProfileType(MmalParameterHeaderType hdr, MmalParameterVideoProfileS[] profile)
        {
            Hdr = hdr;
            Profile = profile;
        }
    }
}