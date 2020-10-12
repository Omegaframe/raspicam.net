using System;
using Microsoft.Extensions.Logging;
using MMALSharp.Common.Utility;
using MMALSharp.Native;
using MMALSharp.Ports;
using static MMALSharp.MmalNativeExceptionHelper;

namespace MMALSharp
{
    public unsafe class MmalPoolImpl : MmalObject, IBufferPool
    {
        public MMAL_POOL_T* Ptr { get; }

        public IBufferQueue Queue { get; }
        public uint HeadersNum => Ptr->HeadersNum;

        public MmalPoolImpl(IPort port)
        {
            MmalLog.Logger.LogDebug($"Creating buffer pool with {port.BufferNum} buffers of size {port.BufferSize}");

            Ptr = MMALUtil.mmal_port_pool_create(port.Ptr, port.BufferNum, port.BufferSize);
            Queue = new MmalQueueImpl((*Ptr).Queue);
        }

        public MmalPoolImpl(MMAL_POOL_T* ptr)
        {
            MmalLog.Logger.LogDebug($"Creating buffer pool from existing instance.");

            Ptr = ptr;
            Queue = new MmalQueueImpl((*Ptr).Queue);
        }

        public override bool CheckState() => Ptr != null && (IntPtr)Ptr != IntPtr.Zero;

        public void Resize(uint numHeaders, uint size) => MmalCheck(MMALPool.mmal_pool_resize(Ptr, numHeaders, size), "Unable to resize pool");

        public override void Dispose()
        {
            MmalLog.Logger.LogDebug("Disposing buffer pool.");
            Destroy();
            base.Dispose();
        }

        void Destroy() => MMALPool.mmal_pool_destroy(Ptr);
    }
}
