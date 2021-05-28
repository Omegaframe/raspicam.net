using System.Runtime.InteropServices;

namespace Raspicam.Net.Native.Format
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
