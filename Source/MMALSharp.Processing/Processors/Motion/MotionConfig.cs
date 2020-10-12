using System;

namespace MMALSharp.Processing.Processors.Motion
{
    public class MotionConfig
    {
        public int Threshold { get; set; }
        public TimeSpan TestFrameInterval { get; set; }
        public string MotionMaskPathname { get; set; }

        public MotionConfig(int threshold = 130, TimeSpan testFrameInterval = default, string motionMaskPathname = null)
        {
            Threshold = threshold;
            TestFrameInterval = testFrameInterval.Equals(TimeSpan.Zero) ? TimeSpan.FromSeconds(10) : testFrameInterval;
            MotionMaskPathname = motionMaskPathname;
        }
    }
}
