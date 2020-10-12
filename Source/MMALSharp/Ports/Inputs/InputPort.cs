using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MMALSharp.Callbacks;
using MMALSharp.Common.Utility;
using MMALSharp.Components;
using MMALSharp.Extensions;
using MMALSharp.Handlers;
using MMALSharp.Native;
using MMALSharp.Ports.Outputs;

namespace MMALSharp.Ports.Inputs
{
    public class InputPort : PortBase<IInputCallbackHandler>, IInputPort
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

        /// <summary>
        /// Call to connect this input port to an output port. This method
        /// simply assigns the <see cref="IConnection"/> to the ConnectedReference property. 
        /// </summary>
        /// <param name="outputPort">The connected output port.</param>
        /// <param name="connection">The connection object.</param>
        public void ConnectTo(IOutputPort outputPort, IConnection connection) => ConnectedReference = connection;

        /// <summary>
        /// Call to configure an input port.
        /// </summary>
        /// <param name="config">The port configuration object.</param>
        /// <param name="copyPort">The port to copy from.</param>
        /// <param name="handler">The capture handler to assign to this port.</param>
        public virtual void Configure(IMMALPortConfig config, IPort copyPort, IInputCaptureHandler handler)
        {
            copyPort?.ShallowCopy(this);

            if (config == null)
                return;

            PortConfig = config;

            if (config.EncodingType != null)
            {
                NativeEncodingType = config.EncodingType.EncodingVal;
            }

            if (config.PixelFormat != null)
            {
                NativeEncodingSubformat = config.PixelFormat.EncodingVal;
            }

            if (config.Width > 0 && config.Height > 0)
            {
                if (config.Crop.HasValue)
                {
                    Crop = config.Crop.Value;
                }
                else
                {
                    Crop = new Rectangle(0, 0, config.Width, config.Height);
                }

                Resolution = new Resolution(config.Width, config.Height);
            }
            else
            {
                // Use config or don't set depending on port type.
                Resolution = new Resolution(0, 0);

                // Certain resolution overrides set to global config Video/Still resolutions so check here if the width and height are greater than 0.
                if (Resolution.Width > 0 && Resolution.Height > 0)
                {
                    Crop = new Rectangle(0, 0, Resolution.Width, Resolution.Height);
                }
            }

            if (config.Framerate > 0)
            {
                FrameRate = config.Framerate;
            }

            if (config.Bitrate > 0)
            {
                Bitrate = config.Bitrate;
            }

            EncodingType = config.EncodingType;

            if (config.ZeroCopy)
            {
                ZeroCopy = true;
                this.SetParameter(MMALParametersCommon.MMAL_PARAMETER_ZERO_COPY, true);
            }

            BufferNum = Math.Max(BufferNumMin, config.BufferNum > 0 ? config.BufferNum : BufferNumRecommended);
            BufferSize = Math.Max(BufferSizeMin, config.BufferSize > 0 ? config.BufferSize : BufferSizeRecommended);

            Commit();

            CallbackHandler = new DefaultInputPortCallbackHandler(this, handler);
        }

        /// <summary>
        /// Enables processing on an input port.
        /// </summary>
        public unsafe void Enable()
        {
            if (Enabled)
                return;

            NativeCallback = new MMALPort.MMAL_PORT_BH_CB_T(NativeInputPortCallback);

            IntPtr ptrCallback = Marshal.GetFunctionPointerForDelegate(NativeCallback);

            MMALLog.Logger.LogDebug($"{Name}: Enabling input port.");

            if (CallbackHandler == null)
            {
                MMALLog.Logger.LogWarning($"{Name}: Callback null");
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

        /// <summary>
        /// Releases an input port buffer and reads further data from user provided image data if not reached end of file.
        /// </summary>
        /// <param name="bufferImpl">A managed buffer object.</param>
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

            // Populate the new input buffer with user provided image data.
            var result = CallbackHandler.CallbackWithResult(newBuffer);

            if (result.Success)            
                newBuffer.ReadIntoBuffer(result.BufferFeed, result.DataLength, result.EOF);            

            SendBuffer(newBuffer);

            if (result.EOF || ComponentReference.ForceStopProcessing)
            {
                MMALLog.Logger.LogDebug($"{Name}: Received EOF. Releasing.");

                Task.Run(() => { Trigger.SetResult(true); });
            }
        }

        public void Start()
        {
            MMALLog.Logger.LogDebug($"{Name}: Starting input port.");
            Trigger = new TaskCompletionSource<bool>();
            Enable();
        }

        /// <summary>
        /// Registers a new input callback handler with this port.
        /// </summary>
        /// <param name="callbackHandler">The callback handler.</param>
        public void RegisterCallbackHandler(IInputCallbackHandler callbackHandler) => CallbackHandler = callbackHandler;

        internal virtual unsafe void NativeInputPortCallback(MMAL_PORT_T* port, MMAL_BUFFER_HEADER_T* buffer)
        {
            if (MMALCameraConfig.Debug)            
                MMALLog.Logger.LogDebug($"{Name}: In native input callback.");            

            var bufferImpl = new MMALBufferImpl(buffer);

            if (bufferImpl.CheckState())
            {
                if (bufferImpl.Cmd > 0)
                {
                    if (bufferImpl.Cmd == MMALEvents.MMAL_EVENT_FORMAT_CHANGED)                    
                        MMALLog.Logger.LogInformation("EVENT FORMAT CHANGED");                    
                }
            }

            bufferImpl.PrintProperties();

            ReleaseBuffer(bufferImpl);
        }
    }
}
