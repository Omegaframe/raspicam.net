using System.Runtime.InteropServices;

namespace Raspicam.Net.Native.Format
{
    [StructLayout(LayoutKind.Explicit)]
    public struct MmalEsSpecificFormat
    {
        [FieldOffset(0)]
        public MmalAudioFormat Audio;

        [FieldOffset(0)]
        public MmalVideoFormat Video;

        [FieldOffset(0)]
        public MmalSubpictureFormat Subpicture;

        public MmalEsSpecificFormat(MmalAudioFormat audio, MmalVideoFormat video, MmalSubpictureFormat subpicture)
        {
            Audio = audio;
            Video = video;
            Subpicture = subpicture;
        }
    }
}
