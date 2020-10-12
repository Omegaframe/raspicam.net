using System;
using Microsoft.Extensions.Logging;
using MMALSharp.Common.Utility;
using MMALSharp.Native;
using MMALSharp.Native.Pool;
using MMALSharp.Native.Util;
using MMALSharp.Ports;
using static MMALSharp.MmalNativeExceptionHelper;

namespace MMALSharp
{
    public unsafe class MmalPoolImpl : MmalObject, IBufferPool
    {
        public MmalPoolType* Ptr { get; }

        public IBufferQueue Queue { get; }
        public uint HeadersNum => Ptr->HeadersNum;

        public MmalPoolImpl(IPort port)
        {
            MmalLog.Logger.LogDebug($"Creating buffer pool with {port.BufferNum} buffers of size {port.BufferSize}");

            Ptr = MmalUtil.PoolCreate(port.Ptr, port.BufferNum, port.BufferSize);
            Queue = new MmalQueueImpl((*Ptr).Queue);
        }

        public MmalPoolImpl(MmalPoolType* ptr)
        {
            MmalLog.Logger.LogDebug($"Creating buffer pool from existing instance.");

            Ptr = ptr;
            Queue = new MmalQueueImpl((*Ptr).Queue);
        }

        public override bool CheckState() => Ptr != null && (IntPtr)Ptr != IntPtr.Zero;

        public void Resize(uint numHeaders, uint size) => MmalCheck(MmalPool.Resize(Ptr, numHeaders, size), "Unable to resize pool");

        public override void Dispose()
        {
            MmalLog.Logger.LogDebug("Disposing buffer pool.");
            Destroy();
            base.Dispose();
        }

        void Destroy() => MmalPool.Destroy(Ptr);
    }
}
