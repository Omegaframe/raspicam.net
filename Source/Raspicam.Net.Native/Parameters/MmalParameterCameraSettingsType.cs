using System.Runtime.InteropServices;
using Raspicam.Net.Native.Util;

namespace Raspicam.Net.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterCameraSettingsType
    {
        public MmalParameterHeaderType Hdr;
        public int Exposure;
        public MmalRational AnalogGain;
        public MmalRational DigitalGain;
        public MmalRational AwbRedGain;
        public MmalRational AwbBlueGain;
        public int FocusPosition;

        public MmalParameterCameraSettingsType(MmalParameterHeaderType hdr, int exposure, MmalRational analogGain,
            MmalRational digitalGain, MmalRational awbRedGain, MmalRational awbBlueGain,
            int focusPosition)
        {
            Hdr = hdr;
            Exposure = exposure;
            AnalogGain = analogGain;
            DigitalGain = digitalGain;
            AwbRedGain = awbRedGain;
            AwbBlueGain = awbBlueGain;
            FocusPosition = focusPosition;
        }
    }
}