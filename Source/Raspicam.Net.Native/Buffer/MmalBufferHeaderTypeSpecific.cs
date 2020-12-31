using System.Runtime.InteropServices;

namespace MMALSharp.Native.Buffer
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
