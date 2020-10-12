using System.Runtime.InteropServices;
using MMALSharp.Native.Util;

namespace MMALSharp.Native.Clock
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalClockEventData
    {
        public int Enable;
        public MmalRational Scale;
        public MmalClockUpdateThreshold UpdateThreshold;
        public MmalClockDiscontThreshold DiscontThreshold;
        public MmalClockRequestThreshold RequestThreshold;
        public MmalClockBufferInfo Buffer;
        public MmalClockLatency Latency;

        public MmalClockEventData(int enable, MmalRational scale, MmalClockUpdateThreshold updateThreshold,
            MmalClockDiscontThreshold discontThreshold, MmalClockRequestThreshold requestThreshold,
            MmalClockBufferInfo buffer, MmalClockLatency latency)
        {
            Enable = enable;
            Scale = scale;
            UpdateThreshold = updateThreshold;
            DiscontThreshold = discontThreshold;
            RequestThreshold = requestThreshold;
            Buffer = buffer;
            Latency = latency;
        }
    }
}
