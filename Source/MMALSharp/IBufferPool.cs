using MMALSharp.Native;
using MMALSharp.Native.Pool;

namespace MMALSharp
{
    public interface IBufferPool : IMmalObject
    {
        unsafe MmalPoolType* Ptr { get; }
        IBufferQueue Queue { get; }
        uint HeadersNum { get; }
        void Resize(uint numHeaders, uint size);
    }
}
