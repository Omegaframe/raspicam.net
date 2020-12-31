using System.Runtime.InteropServices;
using MMALSharp.Native.Buffer;

namespace MMALSharp.Native.Que
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
