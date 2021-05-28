using System;
using System.Runtime.InteropServices;

namespace Raspicam.Net.Native.Buffer
{

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MmalBufferHeader
    {
        public MmalBufferHeader* Next;
        public IntPtr Priv;
        public uint Cmd;
        public byte* Data;
        public uint AllocSize, Length, Offset, Flags;
        public long Pts, Dts;

        public IntPtr Type, UserData;

        public MmalBufferHeader(MmalBufferHeader* next, IntPtr priv, uint cmd, byte* data, uint allocSize, uint length, uint offset, uint flags, long pts, long dts, IntPtr type, IntPtr userData)
        {
            Next = next;
            Priv = priv;
            Cmd = cmd;
            Data = data;
            AllocSize = allocSize;
            Length = length;
            Offset = offset;
            Flags = flags;
            Pts = pts;
            Dts = dts;
            Type = type;
            UserData = userData;
        }
    }
}
