using System;
using MMALSharp.Common;

namespace MMALSharp.Processing.Processors.Motion
{
    public class MotionVectorAnalyser : FrameAnalyser
    {
        internal Action OnDetect { get; set; }

        public MotionVectorAnalyser() { }

        public override void Apply(ImageContext context) { }
    }
}
