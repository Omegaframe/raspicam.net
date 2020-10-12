using System.Drawing;

namespace MMALSharp.Config
{
    /// <summary>
    /// Allows a user to adjust the colour of outputted frames.
    /// </summary>
    public struct ColourEffects
    {
        /// <summary>
        /// Enable the Colour Effects functionality.
        /// </summary>
        public bool Enable { get; }

        /// <summary>
        /// The <see cref="Color"/> to use.
        /// </summary>
        public Color Color { get; }

        /// <summary>
        /// Initialises a new <see cref="ColourEffects"/> struct.
        /// </summary>
        /// <param name="enable">Enable the Colour Effects functionality.</param>
        /// <param name="color">The <see cref="Color"/> to use.</param>
        public ColourEffects(bool enable, Color color)
        {
            Enable = enable;
            Color = color;
        }
    }
}
