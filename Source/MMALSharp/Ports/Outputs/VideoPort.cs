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
    public unsafe class VideoPort : OutputPort, IVideoPort
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

        public VideoPort(IntPtr ptr, IComponent comp, Guid guid) : base(ptr, comp, guid) { }

        public VideoPort(IPort copyFrom) : base((IntPtr)copyFrom.Ptr, copyFrom.ComponentReference, copyFrom.Guid) { }

        /// <inheritdoc />
        public override void Configure(IMMALPortConfig config, IInputPort copyFrom, IOutputCaptureHandler handler)
        {
            base.Configure(config, copyFrom, handler);

            CallbackHandler = new VideoOutputCallbackHandler(this, (IVideoCaptureHandler)handler, config.Split, config.StoreMotionVectors);
        }

        /// <summary>
        /// The native callback MMAL passes buffer headers to.
        /// </summary>
        /// <param name="port">The port the buffer is sent to.</param>
        /// <param name="buffer">The buffer header.</param>
        internal override void NativeOutputPortCallback(MMAL_PORT_T* port, MMAL_BUFFER_HEADER_T* buffer)
        {
            if (MMALCameraConfig.Debug)
                MMALLog.Logger.LogDebug($"{Name}: In native {nameof(VideoPort)} output callback");

            var bufferImpl = new MMALBufferImpl(buffer);

            bufferImpl.PrintProperties();

            var eos = (PortConfig.Timeout.HasValue && DateTime.Now.CompareTo(PortConfig.Timeout.Value) > 0) || ComponentReference.ForceStopProcessing;

            if (bufferImpl.CheckState() && bufferImpl.Length > 0 && !eos && !Trigger.Task.IsCompleted)
                CallbackHandler.Callback(bufferImpl);

            // Ensure we release the buffer before any signalling or we will cause a memory leak due to there still being a reference count on the buffer.
            ReleaseBuffer(bufferImpl, eos);

            if (eos && !Trigger.Task.IsCompleted)
            {
                MMALLog.Logger.LogDebug($"{Name}: Timeout exceeded, triggering signal.");
                Task.Run(() => { Trigger.SetResult(true); });
            }
        }
    }
}
