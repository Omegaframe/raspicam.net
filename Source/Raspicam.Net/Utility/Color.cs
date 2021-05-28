using System.Drawing;

namespace Raspicam.Net.Utility
{
    internal static class MmalColor
    {
        public static (byte y, byte u, byte v) RgbToYuvBytes(Color c)
        {
            var y = (((66 * c.R) + (129 * c.G) + (25 * c.B) + 128) >> 8) + 16;
            var u = (((-38 * c.R) - (74 * c.G) + (112 * c.B) + 128) >> 8) + 128;
            var v = (((112 * c.R) - (94 * c.G) - (18 * c.B) + 128) >> 8) + 128;

            return ((byte)y, (byte)u, (byte)v);
        }
    }
}
