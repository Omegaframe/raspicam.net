using System.Runtime.InteropServices;
using Raspicam.Net.Native.Buffer;
using Raspicam.Net.Native.Internal;

namespace Raspicam.Net.Native.Events
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
