using System.Runtime.InteropServices;
using MMALSharp.Native.Buffer;

namespace MMALSharp.Native.Clock
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MmalClockEvent
    {
        public uint Id;
        public uint Magic;
        public MmalBufferHeader* Buffer;
        public uint Padding0;
        public MmalClockEventData Data;
        public long Padding1;

        public MmalClockEvent(uint id, uint magic, MmalBufferHeader* buffer, uint padding0, MmalClockEventData data, long padding1)
        {
            Id = id;
            Magic = magic;
            Buffer = buffer;
            Padding0 = padding0;
            Data = data;
            Padding1 = padding1;
        }
    }
}
