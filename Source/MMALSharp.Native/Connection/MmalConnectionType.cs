using System;
using System.Runtime.InteropServices;
using MMALSharp.Native.Pool;

namespace MMALSharp.Native.Connection
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MmalConnectionType
    {
        public IntPtr UserData;
        public IntPtr Callback;

        public uint IsEnabled;
        public uint Flags;
        public MMAL_PORT_T* Input;
        public MMAL_PORT_T* Output;
        public MmalPoolType* Pool;
        public MMAL_QUEUE_T* Queue;
        public char* Name;
        public long TimeSetup;
        public long TimeEnable;
        public long TimeDisable;

        public MmalConnectionType(IntPtr userData, IntPtr callback, uint isEnabled, uint flags, MMAL_PORT_T* input, MMAL_PORT_T* output,
            MmalPoolType* pool, MMAL_QUEUE_T* queue, char* name, long timeSetup, long timeEnable, long timeDisable)
        {
            UserData = userData;
            Callback = callback;
            IsEnabled = isEnabled;
            Flags = flags;
            Input = input;
            Output = output;
            Pool = pool;
            Queue = queue;
            Name = name;
            TimeSetup = timeSetup;
            TimeEnable = timeEnable;
            TimeDisable = timeDisable;
        }
    }
}