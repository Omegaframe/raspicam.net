using MMALSharp.Native.Que;

namespace MMALSharp.Mmal
{
    interface IBufferQueue : IMmalObject
    {
        unsafe MmalQueueType* Ptr { get; }

        IBuffer GetBuffer();
        uint QueueLength();
        IBuffer Wait();
        IBuffer TimedWait(int waitMs);
        void Put(IBuffer buffer);
    }
}
