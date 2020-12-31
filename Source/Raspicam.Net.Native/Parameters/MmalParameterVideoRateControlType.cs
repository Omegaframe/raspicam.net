using System.Runtime.InteropServices;

namespace MMALSharp.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterVideoRateControlType
    {
        public MmalParameterHeaderType Hdr;
        public MmalParametersVideo.MmalVideoRatecontrolT Control;

        public unsafe MmalParameterHeaderType* HdrPtr
        {
            get
            {
                fixed (MmalParameterHeaderType* ptr = &Hdr)
                {
                    return ptr;
                }
            }
        }

        public MmalParameterVideoRateControlType(MmalParameterHeaderType hdr, MmalParametersVideo.MmalVideoRatecontrolT control)
        {
            Hdr = hdr;
            Control = control;
        }
    }
}