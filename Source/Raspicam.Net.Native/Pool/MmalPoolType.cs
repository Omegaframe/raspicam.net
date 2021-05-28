using System;
using System.Runtime.InteropServices;
using Raspicam.Net.Native.Que;

namespace Raspicam.Net.Native.Pool
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MmalPoolType
    {
        public MmalQueueType* Queue;
        public uint HeadersNum;
        public IntPtr Header;

        public MmalPoolType(MmalQueueType* queue, uint headersNum, IntPtr header)
        {
            Queue = queue;
            HeadersNum = headersNum;
            Header = header;
        }
    }
}
