using System.Runtime.InteropServices;

namespace MMALSharp.Native.Events
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalEventParameterChanged
    {
        public MMAL_PARAMETER_HEADER_T Hdr;

        public MmalEventParameterChanged(MMAL_PARAMETER_HEADER_T hdr)
        {
            Hdr = hdr;
        }
    }
}
