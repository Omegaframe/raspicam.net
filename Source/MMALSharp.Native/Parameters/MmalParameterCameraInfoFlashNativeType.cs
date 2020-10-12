using System.Runtime.InteropServices;

namespace MMALSharp.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterCameraInfoFlashNativeType
    {
        public MmalParameterCameraInfoFlashType FlashType;

        public MmalParameterCameraInfoFlashNativeType(MmalParameterCameraInfoFlashType flashType)
        {
            FlashType = flashType;
        }
    }
}