using System.Runtime.InteropServices;

namespace MMALSharp.Native.Clock
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalClockRequestThreshold
    {
        public long Threshold;
        public int ThresholdEnable;

        public MmalClockRequestThreshold(long threshold, int thresholdEnable)
        {
            Threshold = threshold;
            ThresholdEnable = thresholdEnable;
        }
    }
}
