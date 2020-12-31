using System.Runtime.InteropServices;

namespace MMALSharp.Native.Format
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalSubpictureFormat
    {
        public uint XOffset;
        public uint YOffset;

        public MmalSubpictureFormat(uint xOffset, uint yOffset)
        {
            XOffset = xOffset;
            YOffset = yOffset;
        }
    }
}
