using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MMALSharp.Callbacks;
using MMALSharp.Components;
using MMALSharp.Config;
using MMALSharp.Extensions;
using MMALSharp.Native.Buffer;
using MMALSharp.Native.Events;
using MMALSharp.Native.Parameters;
using MMALSharp.Native.Port;
using MMALSharp.Ports.Outputs;
using MMALSharp.Utility;

namespace MMALSharp.Ports.Inputs
{
    class InputPort : PortBase<IOutputCallbackHandler>, IInputPort
    {
        public override Resolution Resolution
        {
            get => new Resolution(Width, Height);
            internal set
            {
                Width = value.Pad().Width;
                Height = value.Pad().Height;
            }
        }

        public InputPort(IntPtr ptr, IComponent comp, Guid guid) : base(ptr, comp, PortType.Input, guid) { }

        public void ConnectTo(IOutputPort outputPort, IConnection connection) => ConnectedReference = connection;

        public virtual void Configure(IMmalPortConfig config, IPort copyPort)
        {
            copyPort?.ShallowCopy(this);

            if (config == null)
                return;

            PortConfig = config;

            if (config.EncodingType != null)
                NativeEncodingType = config.EncodingType.EncodingVal;

            if (config.PixelFormat != null)
                NativeEncodingSubformat = config.PixelFormat.EncodingVal;

            if (config.Width > 0 && config.Height > 0)
            {
                if (config.Crop.HasValue)
                    Crop = config.Crop.Value;
                else
                    Crop = new Rectangle(0, 0, config.Width, config.Height);

                Resolution = new Resolution(config.Width, config.Height);
            }
            else
            {
                // Use config or don't set depending on port type.
                Resolution = new Resolution(0, 0);

                // Certain resolution overrides set to global config Video/Still resolutions so check here if the width and height are greater than 0.
                if (Resolution.Width > 0 && Resolution.Height > 0)
                    Crop = new Rectangle(0, 0, Resolution.Width, Resolution.Height);
            }

            if (config.Framerate > 0)
                FrameRate = config.Framerate;

            if (config.Bitrate > 0)
                Bitrate = config.Bitrate;

            EncodingType = config.EncodingType;

            if (config.ZeroCopy)
            {
                ZeroCopy = true;
                this.SetParameter(MmalParametersCommon.MmalParameterZeroCopy, true);
            }

            BufferNum = Math.Max(BufferNumMin, config.BufferNum > 0 ? config.BufferNum : BufferNumRecommended);
            BufferSize = Math.Max(BufferSizeMin, config.BufferSize > 0 ? config.BufferSize : BufferSizeRecommended);

            Commit();
        }

        public unsafe void Enable()
        {
            if (Enabled)
                return;

            NativeCallback = NativeInputPortCallback;

            var ptrCallback = Marshal.GetFunctionPointerForDelegate(NativeCallback);

            MmalLog.Logger.LogDebug($"{Name}: Enabling input port.");

            if (CallbackHandler == null)
            {
                MmalLog.Logger.LogWarning($"{Name}: Callback null");
                EnablePort(IntPtr.Zero);
            }
            else
            {
                EnablePort(ptrCallback);
            }

            InitialiseBufferPool();

            if (!Enabled)
                throw new PiCameraError($"{Name}: Unknown error occurred whilst enabling port");
        }

        public virtual void ReleaseBuffer(IBuffer bufferImpl)
        {
            bufferImpl.Release();

            if (!Enabled || BufferPool == null || Trigger.Task.IsCompleted)
                return;

            IBuffer newBuffer;
            while (true)
            {
                newBuffer = BufferPool.Queue.GetBuffer();
                if (newBuffer != null)
                    break;
            }
        }

        public void Start()
        {
            MmalLog.Logger.LogDebug($"{Name}: Starting input port.");
            Trigger = new TaskCompletionSource<bool>();
            Enable();
        }

        internal virtual unsafe void NativeInputPortCallback(MmalPortType* port, MmalBufferHeader* buffer)
        {
            if (MmalCameraConfig.Debug)
                MmalLog.Logger.LogDebug($"{Name}: In native input callback.");

            var bufferImpl = new MmalBuffer(buffer);

            if (bufferImpl.CheckState())
            {
                if (bufferImpl.Cmd > 0)
                {
                    if (bufferImpl.Cmd == MmalEvents.MmalEventFormatChanged)
                        MmalLog.Logger.LogInformation("EVENT FORMAT CHANGED");
                }
            }

            bufferImpl.PrintProperties();

            ReleaseBuffer(bufferImpl);
        }
    }
}
