using System.Runtime.InteropServices;
using MMALSharp.Common.Utility;

namespace MMALSharp.Native
{
    public static class MmalEvents
    {
        public static int MmalEventError = Helpers.FourCcFromString("ERRO");
        public static int MmalEventEos = Helpers.FourCcFromString("EEOS");
        public static int MmalEventFormatChanged = Helpers.FourCcFromString("EFCH");
        public static int MmalEventParameterChanged = Helpers.FourCcFromString("EPCH");

        [DllImport("libmmal.so", EntryPoint = "mmal_event_format_changed_get", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MMAL_EVENT_FORMAT_CHANGED_T* mmal_event_format_changed_get(MMAL_BUFFER_HEADER_T* buffer);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_EVENT_END_OF_STREAM_T
    {
        MmalPort.MmalPortTypeT portType;
        uint portIndex;

        public MmalPort.MmalPortTypeT PortType => portType;

        public uint PortIndex => portIndex;

        public MMAL_EVENT_END_OF_STREAM_T(MmalPort.MmalPortTypeT portType, uint portIndex)
        {
            this.portType = portType;
            this.portIndex = portIndex;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MMAL_EVENT_FORMAT_CHANGED_T
    {
        uint bufferSizeMin, bufferNumMin, bufferSizeRecommended, bufferNumRecommended;
        MMAL_ES_FORMAT_T* format;

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
