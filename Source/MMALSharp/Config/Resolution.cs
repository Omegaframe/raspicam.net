using System;

namespace MMALSharp.Config
{
    public readonly struct Resolution : IComparable<Resolution>
    {
        public int Width { get; }
        public int Height { get; }

        public Resolution(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public static Resolution As8MPixel => new Resolution(3264, 2448);
        public static Resolution As7MPixel => new Resolution(3072, 2304);
        public static Resolution As6MPixel => new Resolution(3032, 2008);
        public static Resolution As5MPixel => new Resolution(2560, 1920);
        public static Resolution As4MPixel => new Resolution(2240, 1680);
        public static Resolution As3MPixel => new Resolution(2048, 1536);
        public static Resolution As2MPixel => new Resolution(1600, 1200);
        public static Resolution As1MPixel => new Resolution(1280, 960);
        public static Resolution As03MPixel => new Resolution(640, 480);

        public static Resolution As720p => new Resolution(1280, 720);
        public static Resolution As1080p => new Resolution(1920, 1080);
        public static Resolution As1440p => new Resolution(2560, 1440);

        public int CompareTo(Resolution res)
        {
            if (Width == res.Width && Height == res.Height)
                return 0;

            if (Width == res.Width && Height > res.Height)
                return 1;

            if (Width == res.Width && Height < res.Height)
                return -1;

            if (Width > res.Width)
                return 1;

            return -1;
        }

        public Resolution Pad(int width = 32, int height = 16) => new Resolution(VCosAlignUp(Width, width), VCosAlignUp(Height, height));

        static int VCosAlignUp(int value, int roundTo) => (int)(Math.Ceiling(value / Convert.ToDouble(roundTo)) * roundTo);
    }
}