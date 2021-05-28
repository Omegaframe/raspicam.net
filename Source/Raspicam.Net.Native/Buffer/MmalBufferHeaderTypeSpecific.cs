using System.Runtime.InteropServices;

namespace Raspicam.Net.Native.Buffer
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalBufferHeaderTypeSpecific
    {
        public MmalBufferHeaderVideoSpecific Video;

        public MmalBufferHeaderTypeSpecific(MmalBufferHeaderVideoSpecific video)
        {
            Video = video;
        }
    }
}
