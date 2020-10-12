using System.Runtime.InteropServices;
using MMALSharp.Native.Buffer;

namespace MMALSharp.Native
{
    public static class MmalQueue
    {
        // MMAL_QUEUE_T*
        [DllImport("libmmal.so", EntryPoint = "mmal_queue_create", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MMAL_QUEUE_T* mmal_queue_create();

        [DllImport("libmmal.so", EntryPoint = "mmal_queue_put", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void mmal_queue_put(MMAL_QUEUE_T* ptr, MmalBufferHeader* header);

        [DllImport("libmmal.so", EntryPoint = "mmal_queue_put_back", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void mmal_queue_put_back(MMAL_QUEUE_T* ptr, MmalBufferHeader* header);

        // MMAL_QUEUE_T*
        [DllImport("libmmal.so", EntryPoint = "mmal_queue_get", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalBufferHeader* mmal_queue_get(MMAL_QUEUE_T* ptr);

        // MMAL_QUEUE_T*
        [DllImport("libmmal.so", EntryPoint = "mmal_queue_wait", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalBufferHeader* mmal_queue_wait(MMAL_QUEUE_T* ptr);

        // MMAL_QUEUE_T*
        [DllImport("libmmal.so", EntryPoint = "mmal_queue_timedwait", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalBufferHeader* mmal_queue_timedwait(MMAL_QUEUE_T* ptr, int waitms);
        
        [DllImport("libmmal.so", EntryPoint = "mmal_queue_length", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe uint mmal_queue_length(MMAL_QUEUE_T* ptr);

        [DllImport("libmmal.so", EntryPoint = "mmal_queue_destroy", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void mmal_queue_destroy(MMAL_QUEUE_T* ptr);
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MMAL_QUEUE_T
    {
        uint length;
        MmalBufferHeader* first;
        MmalBufferHeader** last;

        public uint Length => length;
        public MmalBufferHeader* First => first;
        public MmalBufferHeader** Last => last;

        public MMAL_QUEUE_T(uint length, MmalBufferHeader* first, MmalBufferHeader** last)
        {
            this.length = length;
            this.first = first;
            this.last = last;
        }
    }
}
