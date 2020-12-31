using System;
using System.Runtime.InteropServices;
using MMALSharp.Native.Port;

namespace MMALSharp.Native.Component
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MmalComponentType
    {
        public IntPtr Priv;

        public IntPtr UserData;

        public char* Name;

        public uint IsEnabled;

        public MmalPortType* Control;

        public uint InputNum;

        public MmalPortType** Input;

        public uint OutputNum;

        public MmalPortType** Output;

        public uint ClockNum;

        public MmalPortType** Clock;

        public uint PortNum;

        public MmalPortType** Port;

        public uint Id;

        public MmalComponentType(
            IntPtr priv,
            IntPtr userData,
            char* name,
            uint isEnabled,
            MmalPortType* control,
            uint inputNum,
            MmalPortType** input,
            uint outputNum,
            MmalPortType** output,
            uint clockNum,
            MmalPortType** clock,
            uint portNum,
            MmalPortType** port,
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
