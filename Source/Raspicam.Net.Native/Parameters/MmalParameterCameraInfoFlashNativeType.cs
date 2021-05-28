using System.Runtime.InteropServices;

namespace Raspicam.Net.Native.Parameters
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