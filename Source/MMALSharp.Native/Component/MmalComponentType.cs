using System;
using System.Runtime.InteropServices;

namespace MMALSharp.Native.Component
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MmalComponentType
    {
        public IntPtr Priv;

        public IntPtr UserData;

        public char* Name;

        public uint IsEnabled;

        public MMAL_PORT_T* Control;

        public uint InputNum;

        public MMAL_PORT_T** Input;

        public uint OutputNum;

        public MMAL_PORT_T** Output;

        public uint ClockNum;

        public MMAL_PORT_T** Clock;

        public uint PortNum;

        public MMAL_PORT_T** Port;

        public uint Id;

        public MmalComponentType(
            IntPtr priv,
            IntPtr userData,
            char* name,
            uint isEnabled,
            MMAL_PORT_T* control,
            uint inputNum,
            MMAL_PORT_T** input,
            uint outputNum,
            MMAL_PORT_T** output,
            uint clockNum,
            MMAL_PORT_T** clock,
            uint portNum,
            MMAL_PORT_T** port,
            uint id)
        {
            Priv = priv;
            UserData = userData;
            Name = name;
            IsEnabled = isEnabled;
            Control = control;
            InputNum = inputNum;
            Input = input;
            OutputNum = outputNum;
            Output = output;
            ClockNum = clockNum;
            Clock = clock;
            PortNum = portNum;
            Port = port;
            Id = id;
        }
    }
}
