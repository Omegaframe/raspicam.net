using MMALSharp.Native;
using MMALSharp.Native.Parameters;

namespace MMALSharp.Config
{
    public class StereoMode
    {
        public MmalStereoscopicModeType Mode { get; set; } = MmalStereoscopicModeType.MmalStereoscopicModeNone;

        public int Decimate { get; set; }
        public int SwapEyes { get; set; }
    }
}
