using System.Runtime.InteropServices;

namespace Raspicam.Net.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterVideoProfileS
    {
        public MmalParametersVideo.MmalVideoProfileT Profile;
        public MmalParametersVideo.MmalVideoLevelT Level;

        public MmalParameterVideoProfileS(MmalParametersVideo.MmalVideoProfileT profile, MmalParametersVideo.MmalVideoLevelT level)
        {
            Profile = profile;
            Level = level;
        }
    }
}