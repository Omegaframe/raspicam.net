using System.Drawing;

namespace Raspicam.Net.Config
{
    public enum JustifyText
    {
        Centre,
        Left,
        Right
    }

    public enum DateTimeTextRefreshRate
    {
        Disabled = 0,
        Daily = 60000,
        Minutes = 1000,
        Seconds = 250,
        SubSecond = 41
    }

    public class AnnotateImage
    {
        public string CustomText { get; set; }
        public int TextSize { get; set; } = 12;
        public Color TextColour { get; set; } = Color.White;
        public Color BgColour { get; set; }
        public bool ShowShutterSettings { get; set; }
        public bool ShowGainSettings { get; set; }
        public bool ShowLensSettings { get; set; }
        public bool ShowCafSettings { get; set; }
        public bool ShowMotionSettings { get; set; }
        public bool ShowFrameNumber { get; set; }
        public bool AllowCustomBackgroundColour { get; set; }
        public bool ShowDateText { get; set; } = true;
        public bool ShowTimeText { get; set; } = true;
        public string DateFormat { get; set; } = "dd/MM/yyyy";
        public string TimeFormat { get; set; } = "HH:mm";
        public DateTimeTextRefreshRate RefreshRate { get; set; } = DateTimeTextRefreshRate.Minutes;
        public JustifyText Justify { get; set; }
        public int XOffset { get; set; }
        public int YOffset { get; set; }

        public AnnotateImage() { }

        public AnnotateImage(string customText, int textSize, Color textColour, Color bgColour,
            bool showShutterSettings, bool showGainSettings, bool showLensSettings,
            bool showCafSettings, bool showMotionSettings, bool showFrameNumber, bool allowCustomBackground,
            bool showDateText, bool showTimeText, JustifyText justify, int xOffset, int yOffset)
        {
            CustomText = customText;
            TextSize = textSize;
            TextColour = textColour;
            BgColour = bgColour;
            ShowShutterSettings = showShutterSettings;
            ShowGainSettings = showGainSettings;
            ShowLensSettings = showLensSettings;
            ShowCafSettings = showCafSettings;
            ShowMotionSettings = showMotionSettings;
            ShowFrameNumber = showFrameNumber;
            AllowCustomBackgroundColour = allowCustomBackground;
            ShowDateText = showDateText;
            ShowTimeText = showTimeText;
            Justify = justify;
            XOffset = xOffset;
            YOffset = yOffset;
        }

        public AnnotateImage(string customText, int textSize, Color textColour)
        {
            CustomText = customText;
            TextSize = textSize;
            TextColour = textColour;

            ShowDateText = true;
            ShowTimeText = true;
        }
    }
}
