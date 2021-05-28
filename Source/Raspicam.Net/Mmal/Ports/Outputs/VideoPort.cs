using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Raspicam.Net.Config;
using Raspicam.Net.Mmal.Callbacks;
using Raspicam.Net.Mmal.Components;
using Raspicam.Net.Mmal.Handlers;
using Raspicam.Net.Mmal.Ports.Inputs;
using Raspicam.Net.Native.Buffer;
using Raspicam.Net.Native.Port;
using Raspicam.Net.Utility;

namespace Raspicam.Net.Mmal.Ports.Outputs
{
    unsafe class VideoPort : OutputPort, IVideoPort
    {
        public override Resolution Resolution
        {
            get => new Resolution(Width, Height);
            internal set
            {
                if (value.Width == 0 || value.Height == 0)
                {
                    Width = CameraConfig.Resolution.Pad().Width;
                    Height = CameraConfig.Resolution.Pad().Height;
                }
                else
                {
                    Width = value.Pad().Width;
                    Height = value.Pad().Height;
                }
            }
        }

        public VideoPort(IntPtr ptr, IComponent comp, Guid guid) : base(ptr, comp, guid) { }

        public override void Configure(IMmalPortConfig config, IInputPort copyFrom, ICaptureHandler handler)
        {
            base.Configure(config, copyFrom, handler);

            CallbackHandler = new VideoOutputCallbackHandler(this, handler);
        }

        internal override void NativeOutputPortCallback(MmalPortType* port, MmalBufferHeader* buffer)
        {
            var bufferImpl = new MmalBuffer(buffer);

            var eos = (PortConfig.Timeout.HasValue && DateTime.Now.CompareTo(PortConfig.Timeout.Value) > 0) || ComponentReference.ForceStopProcessing;

            if (bufferImpl.CheckState() && bufferImpl.Length > 0 && !eos && !Trigger.Task.IsCompleted)
                CallbackHandler.Callback(bufferImpl);

            // Ensure we release the buffer before any signalling or we will cause a memory leak due to there still being a reference count on the buffer.
            ReleaseBuffer(bufferImpl, eos);

            if (eos && !Trigger.Task.IsCompleted)
            {
                MmalLog.Logger.LogDebug($"{Name}: Timeout exceeded, triggering signal.");
                Task.Run(() => { Trigger.SetResult(true); });
            }
        }
    }
}
