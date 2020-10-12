﻿using System;
using Microsoft.Extensions.Logging;
using MMALSharp.Common.Utility;
using MMALSharp.Native;

namespace MMALSharp
{
    public unsafe class MMALQueueImpl : MMALObject, IBufferQueue
    {
        public MMAL_QUEUE_T* Ptr { get; }

        public MMALQueueImpl(MMAL_QUEUE_T* ptr)
        {
            Ptr = ptr;
        }

        public IBuffer GetBuffer()
        {
            var ptr = MMALQueue.mmal_queue_get(Ptr);

            if (!CheckState())
            {
                MMALLog.Logger.LogWarning("Buffer retrieved null.");
                return null;
            }

            return new MMALBufferImpl(ptr);
        }

        public override void Dispose()
        {
            MMALLog.Logger.LogDebug("Disposing queue.");
            Destroy();
            base.Dispose();
        }

        public override string ToString() => $"Ptr address: {(IntPtr)Ptr}";

        public override bool CheckState() => Ptr != null && (IntPtr)Ptr != IntPtr.Zero;

        public uint QueueLength() => MMALQueue.mmal_queue_length(Ptr);

        public IBuffer Wait() => new MMALBufferImpl(MMALQueue.mmal_queue_wait(Ptr));

        public IBuffer TimedWait(int waitms) => new MMALBufferImpl(MMALQueue.mmal_queue_timedwait(Ptr, waitms));

        public void Put(IBuffer buffer) => MMALQueue.mmal_queue_put(Ptr, buffer.Ptr);

        internal static MMALQueueImpl Create() => new MMALQueueImpl(MMALQueue.mmal_queue_create());

        void Destroy() => MMALQueue.mmal_queue_destroy(Ptr);
    }
}
