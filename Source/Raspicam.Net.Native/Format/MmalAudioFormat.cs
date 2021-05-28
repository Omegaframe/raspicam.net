using System.Runtime.InteropServices;

namespace Raspicam.Net.Native.Format
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalAudioFormat
    {
        public uint Channels;
        public uint SampleRate;
        public uint BitsPerSample;
        public uint BlockAlign;

        public MmalAudioFormat(uint channels, uint sampleRate, uint bitsPerSample, uint blockAlign)
        {
            Channels = channels;
            SampleRate = sampleRate;
            BitsPerSample = bitsPerSample;
            BlockAlign = blockAlign;
        }
    }
}
