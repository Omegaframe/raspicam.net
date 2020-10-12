using System.Runtime.InteropServices;

namespace MMALSharp.Native.Clock
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalClockUpdateThreshold
    {
        public long ThresholdLower;
        public long ThresholdUpper;

        public MmalClockUpdateThreshold(long thresholdLower, long thresholdUpper)
        {
            ThresholdLower = thresholdLower;
            ThresholdUpper = thresholdUpper;
        }
    }
}
