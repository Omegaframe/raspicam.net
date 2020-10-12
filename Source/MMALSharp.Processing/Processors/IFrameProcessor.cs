using MMALSharp.Common;

namespace MMALSharp.Processors
{
    /// <summary>
    /// A processor to apply image processing techniques on image frame data.
    /// </summary>
    public interface IFrameProcessor
    {
        /// <summary>
        /// Apply the convolution.
        /// </summary>
        /// <param name="context">The image's metadata.</param>
        void Apply(ImageContext context);
    }
}
