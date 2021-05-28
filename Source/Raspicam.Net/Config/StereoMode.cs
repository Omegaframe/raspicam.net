using Raspicam.Net.Native.Parameters;

namespace Raspicam.Net.Config
{
    public class StereoMode
    {
        public MmalStereoscopicModeType Mode { get; set; } = MmalStereoscopicModeType.MmalStereoscopicModeNone;

        public int Decimate { get; set; }
        public int SwapEyes { get; set; }
    }
}
