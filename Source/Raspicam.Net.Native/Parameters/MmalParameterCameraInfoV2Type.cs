using System.Runtime.InteropServices;

namespace Raspicam.Net.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterCameraInfoV2Type
    {
        public MmalParameterHeaderType Hdr;
        public int NumCameras;
        public int NumFlashes;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public MmalParameterCameraInfoCameraV2Type[] Cameras;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public MmalParameterCameraInfoFlashNativeType[] Flashes;

        public MmalParameterCameraInfoV2Type(MmalParameterHeaderType hdr, int numCameras, int numFlashes, MmalParameterCameraInfoCameraV2Type[] cameras,
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