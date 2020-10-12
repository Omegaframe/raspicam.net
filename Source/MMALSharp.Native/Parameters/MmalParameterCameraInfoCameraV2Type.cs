using System.Runtime.InteropServices;

namespace MMALSharp.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct MmalParameterCameraInfoCameraV2Type
    {
        public int PortId;
        public int MaxWidth;
        public int MaxHeight;
        public int LensPresent;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string CameraName;

        public MmalParameterCameraInfoCameraV2Type(int portId, int maxWidth, int maxHeight, int lensPresent, string cameraName)
        {
            PortId = portId;
            MaxWidth = maxWidth;
            MaxHeight = maxHeight;
            LensPresent = lensPresent;
            CameraName = cameraName;
        }
    }
}