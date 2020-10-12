using System;
using MMALSharp.Common;

namespace MMALSharp.Processors.Motion
{
    /// <summary>
    /// A frame analyser for use with motion vector detection.
    /// </summary>
    public class MotionVectorAnalyser : FrameAnalyser
    {
        internal Action OnDetect { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="MotionVectorAnalyser"/>.
        /// </summary>
        public MotionVectorAnalyser()
        {
        }

        /// <inheritdoc />
        public override void Apply(ImageContext context)
        {
        }
    }
}
