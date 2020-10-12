using System.Runtime.InteropServices;

namespace MMALSharp.Native.Events
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MmalEventFormatChanged
    {
        public uint BufferSizeMin;

        public uint BufferNumMin;

        public uint BufferSizeRecommended;

        public uint BufferNumRecommended;

        public MMAL_ES_FORMAT_T* Format;

        public MmalEventFormatChanged(uint bufferSizeMin, uint bufferNumMin, uint bufferSizeRecommended, uint bufferNumRecommended, MMAL_ES_FORMAT_T* format)
        {
            BufferSizeMin = bufferSizeMin;
            BufferNumMin = bufferNumMin;
            BufferSizeRecommended = bufferSizeRecommended;
            BufferNumRecommended = bufferNumRecommended;
            Format = format;
        }
    }
}
