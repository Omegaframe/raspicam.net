using MMALSharp.Native;

namespace MMALSharp
{
    public interface IBufferQueue : IMmalObject
    {
        unsafe MMAL_QUEUE_T* Ptr { get; }

        IBuffer GetBuffer();
        uint QueueLength();
        IBuffer Wait();
        IBuffer TimedWait(int waitMs);
        void Put(IBuffer buffer);
    }
}
