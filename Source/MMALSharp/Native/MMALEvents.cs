using System.Runtime.InteropServices;
using MMALSharp.Common.Utility;

namespace MMALSharp.Native
{
    public static class MMALEvents
    {
        public static int MMAL_EVENT_ERROR = Helpers.FourCcFromString("ERRO");
        public static int MMAL_EVENT_EOS = Helpers.FourCcFromString("EEOS");
        public static int MMAL_EVENT_FORMAT_CHANGED = Helpers.FourCcFromString("EFCH");
        public static int MMAL_EVENT_PARAMETER_CHANGED = Helpers.FourCcFromString("EPCH");

        [DllImport("libmmal.so", EntryPoint = "mmal_event_format_changed_get", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MMAL_EVENT_FORMAT_CHANGED_T* mmal_event_format_changed_get(MMAL_BUFFER_HEADER_T* buffer);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_EVENT_END_OF_STREAM_T
    {
        private MMALPort.MMAL_PORT_TYPE_T portType;
        private uint portIndex;

        public MMALPort.MMAL_PORT_TYPE_T PortType => portType;

        public uint PortIndex => portIndex;

        public MMAL_EVENT_END_OF_STREAM_T(MMALPort.MMAL_PORT_TYPE_T portType, uint portIndex)
        {
            this.portType = portType;
            this.portIndex = portIndex;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MMAL_EVENT_FORMAT_CHANGED_T
    {
        private uint bufferSizeMin, bufferNumMin, bufferSizeRecommended, bufferNumRecommended;
        private MMAL_ES_FORMAT_T* format;

        public uint BufferSizeMin => bufferSizeMin;

        public uint BufferNumMin => bufferNumMin;

        public uint BufferSizeRecommended => bufferSizeRecommended;

        public uint BufferNumRecommended => bufferNumRecommended;

        public MMAL_ES_FORMAT_T* Format => format;

        public MMAL_EVENT_FORMAT_CHANGED_T(uint bufferSizeMin, uint bufferNumMin, uint bufferSizeRecommended, uint bufferNumRecommended,
            MMAL_ES_FORMAT_T* format)
        {
            this.bufferSizeMin = bufferSizeMin;
            this.bufferNumMin = bufferNumMin;
            this.bufferSizeRecommended = bufferSizeRecommended;
            this.bufferNumRecommended = bufferNumRecommended;
            this.format = format;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_EVENT_PARAMETER_CHANGED_T
    {
        public MMAL_PARAMETER_HEADER_T hdr;

        public MMAL_PARAMETER_HEADER_T Hdr => hdr;

        public MMAL_EVENT_PARAMETER_CHANGED_T(MMAL_PARAMETER_HEADER_T hdr)
        {
            this.hdr = hdr;
        }
    }
}
