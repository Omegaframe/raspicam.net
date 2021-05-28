using System.Drawing;

namespace Raspicam.Net.Config
{
    public struct ColorEffects
    {
        public bool Enable { get; }
        public Color Color { get; }

        public ColorEffects(bool enable, Color color)
        {
            Enable = enable;
            Color = color;
        }
    }
}
