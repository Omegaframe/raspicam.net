using System;
using MMALSharp.Processing.Processors.Motion;

namespace MMALSharp.Processing.Handlers
{
    public interface IMotionCaptureHandler
    {
        MotionType MotionType { get; set; }
        void ConfigureMotionDetection(MotionConfig config, Action onDetect);
        void EnableMotionDetection();
        void DisableMotionDetection();
    }
}
