using System.Runtime.InteropServices;

namespace MMALSharp.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterCameraInfoCameraType
    {
        public int PortId;
        public int MaxWidth;
        public int MaxHeight;
        public int LensPresent;

        public MmalParameterCameraInfoCameraType(int portId, int maxWidth, int maxHeight, int lensPresent)
        {
            PortId = portId;
            MaxWidth = maxWidth;
            MaxHeight = maxHeight;
            LensPresent = lensPresent;
        }
    }
}