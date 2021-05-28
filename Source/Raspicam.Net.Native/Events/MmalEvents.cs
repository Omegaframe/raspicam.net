using Raspicam.Net.Native.Internal;

namespace Raspicam.Net.Native.Events
{
    public static class MmalEvents
    {
        public static int MmalEventError = "ERRO".ToFourCc();
        public static int MmalEventEos = "EEOS".ToFourCc();
        public static int MmalEventFormatChanged = "EFCH".ToFourCc();
        public static int MmalEventParameterChanged = "EPCH".ToFourCc();
    }
}
