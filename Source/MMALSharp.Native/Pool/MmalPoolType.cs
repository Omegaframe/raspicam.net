using System;
using System.Runtime.InteropServices;

namespace MMALSharp.Native.Pool
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MmalPoolType
    {
        public MMAL_QUEUE_T* Queue;
        public uint HeadersNum;
        public IntPtr Header;

        public MmalPoolType(MMAL_QUEUE_T* queue, uint headersNum, IntPtr header)
        {
            Queue = queue;
            HeadersNum = headersNum;
            Header = header;
        }
    }
}
