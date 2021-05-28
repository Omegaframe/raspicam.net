using System.Runtime.InteropServices;

namespace Raspicam.Net.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterCameraInfoType
    {
        public MmalParameterHeaderType Hdr;
        public int NumCameras;
        public int NumFlashes;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public MmalParameterCameraInfoCameraType[] Cameras;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public MmalParameterCameraInfoFlashNativeType[] Flashes;

        public MmalParameterCameraInfoType(MmalParameterHeaderType hdr, int numCameras, int numFlashes, MmalParameterCameraInfoCameraType[] cameras,
            MmalParameterCameraInfoFlashNativeType[] flashes)
        {
            Hdr = hdr;
            NumCameras = numCameras;
            NumFlashes = numFlashes;
            Cameras = cameras;
            Flashes = flashes;
        }
    }
}