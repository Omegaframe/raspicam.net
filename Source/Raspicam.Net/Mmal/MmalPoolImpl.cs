using System;
using Microsoft.Extensions.Logging;
using Raspicam.Net.Mmal.Ports;
using Raspicam.Net.Native.Pool;
using Raspicam.Net.Native.Util;
using Raspicam.Net.Utility;
using static Raspicam.Net.MmalNativeExceptionHelper;

namespace Raspicam.Net.Mmal
{
    unsafe class MmalPoolImpl : MmalObject, IBufferPool
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
