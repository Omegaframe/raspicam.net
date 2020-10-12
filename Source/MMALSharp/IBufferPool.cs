using MMALSharp.Native;

namespace MMALSharp
{
    public interface IBufferPool : IMmalObject
    {
        unsafe MMAL_POOL_T* Ptr { get; }
        IBufferQueue Queue { get; }
        uint HeadersNum { get; }
        void Resize(uint numHeaders, uint size);
    }
}
