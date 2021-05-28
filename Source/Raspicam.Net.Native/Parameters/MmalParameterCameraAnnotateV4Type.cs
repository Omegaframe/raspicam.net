using System.Runtime.InteropServices;

namespace Raspicam.Net.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterCameraAnnotateV4Type
    {
        public MmalParameterHeaderType Hdr;
        public int Enable;
        public int ShowShutter;
        public int ShowAnalogGain;
        public int ShowLens;
        public int ShowCaf;
        public int ShowMotion;
        public int ShowFrameNum;
        public int EnableTextBackground;
        public int CustomBackgroundColor;
        public byte CustomBackgroundY;
        public byte CustomBackgroundU;
        public byte CustomBackgroundV;
        public byte Dummy1;
        public int CustomTextColor;
        public byte CustomTextY;
        public byte CustomTextU;
        public byte CustomTextV;
        public byte TextSize;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MmalParametersCamera.MmalCameraAnnotateMaxTextLenV3)]
        public byte[] Text;

        public int Justify;
        public int XOffset;
        public int YOffset;

        public MmalParameterCameraAnnotateV4Type(MmalParameterHeaderType hdr, int enable, int showShutter, int showAnalogGain, int showLens,
            int showCaf, int showMotion, int showFrameNum, int enableTextBackground, int customBackgroundColor,
            byte customBackgroundY, byte customBackgroundU, byte customBackgroundV, byte dummy1,
            int customTextColor, byte customTextY, byte customTextU, byte customTextV, byte textSize,
            byte[] text, int justify, int xOffset, int yOffset)
        {
            Hdr = hdr;

            Enable = enable;
            Text = text;
            ShowShutter = showShutter;
            ShowAnalogGain = showAnalogGain;
            ShowLens = showLens;
            ShowCaf = showCaf;
            ShowMotion = showMotion;
            ShowFrameNum = showFrameNum;
            EnableTextBackground = enableTextBackground;
            CustomBackgroundColor = customBackgroundColor;
            CustomBackgroundY = customBackgroundY;
            CustomBackgroundU = customBackgroundU;
            CustomBackgroundV = customBackgroundV;
            Dummy1 = dummy1;
            CustomTextColor = customTextColor;
            CustomTextY = customTextY;
            CustomTextU = customTextU;
            CustomTextV = customTextV;
            TextSize = textSize;
            Text = text;
            Justify = justify;
            XOffset = xOffset;
            YOffset = yOffset;
        }
    }
}