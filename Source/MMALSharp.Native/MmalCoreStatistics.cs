using System.Runtime.InteropServices;

namespace MMALSharp.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalCoreStatistics
    {
        public uint BufferCount;
        public uint FirstBufferTime;
        public uint LastBufferTime;
        public uint MaxDelay;

        public MmalCoreStatistics(uint bufferCount, uint firstBufferTime, uint lastBufferTime, uint maxDelay)
        {
            BufferCount = bufferCount;
            FirstBufferTime = firstBufferTime;
            LastBufferTime = lastBufferTime;
            MaxDelay = maxDelay;
        }
    }
}
