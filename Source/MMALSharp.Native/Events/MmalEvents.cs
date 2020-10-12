using System.Runtime.InteropServices;
using MMALSharp.Native.Buffer;
using MMALSharp.Native.Internal;

namespace MMALSharp.Native.Events
{
    public static class MmalEvents
    {
        public static int MmalEventError = "ERRO".ToFourCc();
        public static int MmalEventEos = "EEOS".ToFourCc();
        public static int MmalEventFormatChanged = "EFCH".ToFourCc();
        public static int MmalEventParameterChanged = "EPCH".ToFourCc();

        [DllImport("libmmal.so", EntryPoint = "mmal_event_format_changed_get", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalEventFormatChanged* GetChanged(MmalBufferHeader* buffer);
    }
}
