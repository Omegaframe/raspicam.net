using System;
using System.Runtime.InteropServices;
using Raspicam.Net.Native.Pool;
using Raspicam.Net.Native.Port;
using Raspicam.Net.Native.Que;

namespace Raspicam.Net.Native.Connection
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MmalConnectionType
    {
        public IntPtr UserData;
        public IntPtr Callback;

        public uint IsEnabled;
        public uint Flags;
        public MmalPortType* Input;
        public MmalPortType* Output;
        public MmalPoolType* Pool;
        public MmalQueueType* Queue;
        public char* Name;
        public long TimeSetup;
        public long TimeEnable;
        public long TimeDisable;

        public MmalConnectionType(IntPtr userData, IntPtr callback, uint isEnabled, uint flags, MmalPortType* input, MmalPortType* output,
            MmalPoolType* pool, MmalQueueType* queue, char* name, long timeSetup, long timeEnable, long timeDisable)
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