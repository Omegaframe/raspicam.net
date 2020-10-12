using MMALSharp.Native;

namespace MMALSharp.Config
{
    public class StereoMode
    {
        public MmalStereoscopicModeT Mode { get; set; } = MmalStereoscopicModeT.MmalStereoscopicModeNone;

        public int Decimate { get; set; }
        public int SwapEyes { get; set; }
    }
}
