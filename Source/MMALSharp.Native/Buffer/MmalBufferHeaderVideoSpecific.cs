using System.Runtime.InteropServices;

namespace MMALSharp.Native.Buffer
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalBufferHeaderVideoSpecific
    {
        public uint Planes;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public uint[] Offset;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public uint[] Pitch;

        public uint Flags;

        public MmalBufferHeaderVideoSpecific(uint planes, uint[] offset, uint[] pitch, uint flags)
        {
            Planes = planes;
            Offset = offset;
            Pitch = pitch;
            Flags = flags;
        }
    }
}
