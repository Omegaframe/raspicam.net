using System.IO;

namespace MMALSharp.Processing.Handlers
{
    public interface IMotionVectorCaptureHandler
    {
        void InitialiseMotionStore(Stream stream);
        void ProcessMotionVectors(byte[] data);
    }
}
