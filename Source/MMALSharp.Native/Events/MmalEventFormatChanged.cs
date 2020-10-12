using System.Runtime.InteropServices;
using MMALSharp.Native.Format;

namespace MMALSharp.Native.Events
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MmalEventFormatChanged
    {
        public uint BufferSizeMin;

        public uint BufferNumMin;

        public uint BufferSizeRecommended;

        public uint BufferNumRecommended;

        public MmalEsFormat* Format;

        public MmalEventFormatChanged(uint bufferSizeMin, uint bufferNumMin, uint bufferSizeRecommended, uint bufferNumRecommended, MmalEsFormat* format)
        {
            BufferSizeMin = bufferSizeMin;
            BufferNumMin = bufferNumMin;
            BufferSizeRecommended = bufferSizeRecommended;
            BufferNumRecommended = bufferNumRecommended;
            Format = format;
        }
    }
}
