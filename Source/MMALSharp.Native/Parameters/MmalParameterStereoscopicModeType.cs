using System.Runtime.InteropServices;

namespace MMALSharp.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterStereoscopicModeType
    {
        public MmalParameterHeaderType Hdr;
        public MmalStereoscopicModeType Mode;
        public int Decimate;
        public int SwapEyes;

        public MmalParameterStereoscopicModeType(MmalParameterHeaderType hdr, MmalStereoscopicModeType mode,
            int decimate, int swapEyes)
        {
            Hdr = hdr;
            Mode = mode;
            Decimate = decimate;
            SwapEyes = swapEyes;
        }
    }
}