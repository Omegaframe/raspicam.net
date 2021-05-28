using System.Runtime.InteropServices;
using Raspicam.Net.Native.Buffer;

namespace Raspicam.Net.Native.Que
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MmalQueueType
    {
        public uint Length;
        public MmalBufferHeader* First;
        public MmalBufferHeader** Last;

        public MmalQueueType(uint length, MmalBufferHeader* first, MmalBufferHeader** last)
        {
            Length = length;
            First = first;
            Last = last;
        }
    }
}
