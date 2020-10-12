using System.Runtime.InteropServices;

namespace MMALSharp.Native.Clock
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalClockBufferInfo
    {
        public long Timestamp;
        public uint ArrivalTime;

        public MmalClockBufferInfo(long timestamp, uint arrivalTime)
        {
            Timestamp = timestamp;
            ArrivalTime = arrivalTime;
        }
    }
}
