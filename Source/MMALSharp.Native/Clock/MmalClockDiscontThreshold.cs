using System.Runtime.InteropServices;

namespace MMALSharp.Native.Clock
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalClockDiscontThreshold
    {
        public long Threshold;
        public long Duration;

        public MmalClockDiscontThreshold(long threshold, long duration)
        {
            Threshold = threshold;
            Duration = duration;
        }
    }
}
