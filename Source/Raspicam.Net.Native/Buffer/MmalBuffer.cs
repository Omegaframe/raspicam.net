using System.Runtime.InteropServices;
using MMALSharp.Native.Util;

namespace MMALSharp.Native.Buffer
{
    public static class MmalBuffer
    {
        [DllImport("libmmal.so", EntryPoint = "mmal_buffer_header_acquire", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void HeaderAcquire(MmalBufferHeader* header);

        [DllImport("libmmal.so", EntryPoint = "mmal_buffer_header_reset", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void HeaderReset(MmalBufferHeader* header);

        [DllImport("libmmal.so", EntryPoint = "mmal_buffer_header_release", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void HeaderRelease(MmalBufferHeader* header);

        [DllImport("libmmal.so", EntryPoint = "mmal_buffer_header_mem_lock", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum MemLock(MmalBufferHeader* header);

        [DllImport("libmmal.so", EntryPoint = "mmal_buffer_header_mem_unlock", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void MemUnlock(MmalBufferHeader* header);
    }
}