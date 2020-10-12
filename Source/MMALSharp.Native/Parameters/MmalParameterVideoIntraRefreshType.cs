using System.Runtime.InteropServices;

namespace MMALSharp.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterVideoIntraRefreshType
    {
        public MmalParameterHeaderType Hdr;
        public MmalParametersVideo.MmalVideoIntraRefreshT RefreshMode;
        public int AirMbs;
        public int AirRef;
        public int CirMbs;
        public int PirMbs;

        public unsafe MmalParameterHeaderType* HdrPtr
        {
            get
            {
                fixed (MmalParameterHeaderType* ptr = &Hdr)
                {
                    return ptr;
                }
            }
        }

        public MmalParameterVideoIntraRefreshType(MmalParameterHeaderType hdr, MmalParametersVideo.MmalVideoIntraRefreshT refreshMode,
            int airMbs, int airRef, int cirMbs, int pirMbs)
        {
            Hdr = hdr;
            RefreshMode = refreshMode;
            AirMbs = airMbs;
            AirRef = airRef;
            CirMbs = cirMbs;
            PirMbs = pirMbs;
        }
    }
}