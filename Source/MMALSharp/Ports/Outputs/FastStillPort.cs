using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MMALSharp.Callbacks;
using MMALSharp.Common.Utility;
using MMALSharp.Components;
using MMALSharp.Handlers;
using MMALSharp.Native;
using MMALSharp.Ports.Inputs;

namespace MMALSharp.Ports.Outputs
{
    /// <summary>
    /// Represents a still port used specifically when capturing rapid single image frames from the camera's video port.
    /// </summary>
    public unsafe class FastStillPort : OutputPort, IVideoPort
    {
        public override Resolution Resolution
        {
            get => new Resolution(Width, Height);
            internal set
            {
                if (value.Width == 0 || value.Height == 0)
                {
                    Width = MMALCameraConfig.Resolution.Pad().Width;
                    Height = MMALCameraConfig.Resolution.Pad().Height;
                }
                else
                {
                    Width = value.Pad().Width;
                    Height = value.Pad().Height;
                }
            }
        }

        public FastStillPort(IntPtr ptr, IComponent comp, Guid guid) : base(ptr, comp, guid) { }

        public FastStillPort(IPort copyFrom) : base((IntPtr)copyFrom.Ptr, copyFrom.ComponentReference, copyFrom.Guid) { }

        public override void Configure(IMMALPortConfig config, IInputPort copyFrom, IOutputCaptureHandler handler)
        {
            base.Configure(config, copyFrom, handler);

            CallbackHandler = new FastImageOutputCallbackHandler(this, handler);
        }

        /// <summary>
        /// The native callback MMAL passes buffer headers to.
        /// </summary>
        /// <param name="port">The port the buffer is sent to.</param>
        /// <param name="buffer">The buffer header.</param>
        internal override void NativeOutputPortCallback(MMAL_PORT_T* port, MMAL_BUFFER_HEADER_T* buffer)
        {
            if (MMALCameraConfig.Debug)
                MmalLog.Logger.LogDebug($"{Name}: In native {nameof(FastStillPort)} output callback.");

            var bufferImpl = new MMALBufferImpl(buffer);

            bufferImpl.PrintProperties();

            var failed = bufferImpl.AssertProperty(MMALBufferProperties.MMAL_BUFFER_HEADER_FLAG_TRANSMISSION_FAILED);

            if ((bufferImpl.CheckState() && bufferImpl.Length > 0 && !ComponentReference.ForceStopProcessing && !failed && !Trigger.Task.IsCompleted) ||
                (ComponentReference.ForceStopProcessing && !Trigger.Task.IsCompleted))
            {
                CallbackHandler.Callback(bufferImpl);
            }

            // Ensure we release the buffer before any signalling or we will cause a memory leak due to there still being a reference count on the buffer.
            ReleaseBuffer(bufferImpl, ComponentReference.ForceStopProcessing || failed);

            // If this buffer signals the end of data stream, allow waiting thread to continue.
            if (ComponentReference.ForceStopProcessing || failed)
            {
                MmalLog.Logger.LogDebug($"{Name}: Signaling completion of continuous still frame capture...");
                Task.Run(() => { Trigger.SetResult(true); });
            }
        }
    }
}