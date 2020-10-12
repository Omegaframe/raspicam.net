using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MMALSharp.Callbacks;
using MMALSharp.Common;
using MMALSharp.Common.Utility;
using MMALSharp.Components;
using MMALSharp.Extensions;
using MMALSharp.Handlers;
using MMALSharp.Native;
using MMALSharp.Ports.Inputs;

namespace MMALSharp.Ports.Outputs
{
    public unsafe class OutputPort : PortBase<IOutputCallbackHandler>, IOutputPort
    {
        /// <inheritdoc />
        public override Resolution Resolution
        {
            get => new Resolution(Width, Height);
            internal set
            {
                Width = value.Pad().Width;
                Height = value.Pad().Height;
            }
        }

        public OutputPort(IntPtr ptr, IComponent comp, Guid guid)             : base(ptr, comp, PortType.Output, guid)        {        }

        /// <summary>
        /// Call to configure an output port.
        /// </summary>
        /// <param name="config">The port configuration object.</param>
        /// <param name="copyFrom">The port to copy from.</param>
        /// <param name="handler">The capture handler to assign to this port.</param>
        public virtual void Configure(IMMALPortConfig config, IInputPort copyFrom, IOutputCaptureHandler handler)
        {
            if (config != null)
            {
                PortConfig = config;

                copyFrom?.ShallowCopy(this);

                if (config.EncodingType != null)
                {
                    NativeEncodingType = config.EncodingType.EncodingVal;
                }

                if (config.PixelFormat != null)
                {
                    NativeEncodingSubformat = config.PixelFormat.EncodingVal;
                }

                Par = new MMAL_RATIONAL_T(1, 1);

                MMAL_VIDEO_FORMAT_T tempVid = Ptr->Format->Es->Video;

                try
                {
                    Commit();
                }
                catch
                {
                    // If commit fails using new settings, attempt to reset using old temp MMAL_VIDEO_FORMAT_T.
                    MMALLog.Logger.LogWarning($"{Name}: Commit of output port failed. Attempting to reset values.");
                    Ptr->Format->Es->Video = tempVid;
                    Commit();
                }
                
                if (config.ZeroCopy)
                {
                    ZeroCopy = true;
                    this.SetParameter(MMALParametersCommon.MMAL_PARAMETER_ZERO_COPY, true);
                }

                if (MMALCameraConfig.VideoColorSpace != null &&
                    MMALCameraConfig.VideoColorSpace.EncType == MMALEncoding.EncodingType.ColorSpace)
                {
                    VideoColorSpace = MMALCameraConfig.VideoColorSpace;
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
                PixelFormat = config.PixelFormat;

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

                BufferNum = Math.Max(BufferNumMin, config.BufferNum > 0 ? config.BufferNum : BufferNumRecommended);
                BufferSize = Math.Max(BufferSizeMin, config.BufferSize > 0 ? config.BufferSize : BufferSizeRecommended);

                // It is important to re-commit changes to width and height.
                Commit();
            }
            
            CallbackHandler = new DefaultOutputPortCallbackHandler(this, handler);
        }

        /// <summary>
        /// Connects two components together by their input and output ports.
        /// </summary>
        /// <param name="destinationComponent">The component we want to connect to.</param>
        /// <param name="inputPort">The input port of the component we want to connect to.</param>
        /// <param name="useCallback">Flag to use connection callback (adversely affects performance).</param>
        /// <returns>The connection instance between the source output and destination input ports.</returns>
        public virtual IConnection ConnectTo(IDownstreamComponent destinationComponent, int inputPort = 0, bool useCallback = false)
        {
            if (ConnectedReference != null)
            {
                MMALLog.Logger.LogWarning($"{Name}: A connection has already been established on this port");
                return ConnectedReference;
            }

            var connection = MMALConnectionImpl.CreateConnection(this, destinationComponent.Inputs[inputPort], destinationComponent, useCallback);
            ConnectedReference = connection;

            destinationComponent.Inputs[inputPort].ConnectTo(this, connection);

            return connection;
        }

        /// <summary>
        /// Release an output port buffer, get a new one from the queue and send it for processing.
        /// </summary>
        /// <param name="bufferImpl">A managed buffer object.</param>
        /// <param name="eos">Flag that this buffer is the end of stream.</param>
        public virtual void ReleaseBuffer(IBuffer bufferImpl, bool eos)
        {
            bufferImpl.Release();
            
            if (eos)
            {
                // If we have reached the end of stream, we don't want to send a buffer to the output port again.
                return;
            }

            IBuffer newBuffer = null;

            try
            {
                if (MMALCameraConfig.Debug)
                {
                    if (!Enabled)
                    {
                        MMALLog.Logger.LogDebug($"{Name}: Port not enabled.");
                    }

                    if (BufferPool == null)
                    {
                        MMALLog.Logger.LogDebug($"{Name}: Buffer pool null.");
                    }
                }
                
                if (Enabled && BufferPool != null)
                {
                    newBuffer = BufferPool.Queue.GetBuffer();
                    
                    if (newBuffer.CheckState())
                    {
                        SendBuffer(newBuffer);
                    }
                    else
                    {
                        MMALLog.Logger.LogWarning($"{Name}: Buffer null. Continuing.");
                    }
                }
            }
            catch (Exception e)
            {
                if (newBuffer != null && newBuffer.CheckState())
                {
                    newBuffer.Release();
                }

                MMALLog.Logger.LogWarning($"{Name}: Unable to send buffer header. {e.Message}");
            }
        }

        /// <summary>
        /// Call to register a new callback handler with this port.
        /// </summary>
        /// <param name="callbackHandler">The output callback handler.</param>
        public void RegisterCallbackHandler(IOutputCallbackHandler callbackHandler)
        {
            CallbackHandler = callbackHandler;
        }

        /// <summary>
        /// Enables processing on an output port.
        /// </summary>
        public virtual void Enable()
        {            
            if (!Enabled)
            {
                NativeCallback = NativeOutputPortCallback;
                
                IntPtr ptrCallback = Marshal.GetFunctionPointerForDelegate(NativeCallback);
                PtrCallback = ptrCallback;
                
                if (CallbackHandler == null)
                {
                    MMALLog.Logger.LogWarning($"{Name}: Callback null");

                    EnablePort(IntPtr.Zero);
                }
                else
                {
                    EnablePort(ptrCallback);
                }
                
                if (CallbackHandler != null)
                {
                    SendAllBuffers();
                }
            }
            
            if (!Enabled)
            {
                throw new PiCameraError($"{Name}: Unknown error occurred whilst enabling port");
            }
        }

        /// <summary>
        /// Enable the port specified.
        /// </summary>
        public void Start()
        {
            MMALLog.Logger.LogDebug($"{Name}: Starting output port.");
            Trigger = new TaskCompletionSource<bool>();
            Enable();
        }
        
        /// <summary>
        /// The native callback MMAL passes buffer headers to.
        /// </summary>
        /// <param name="port">The port the buffer is sent to.</param>
        /// <param name="buffer">The buffer header.</param>
        internal virtual void NativeOutputPortCallback(MMAL_PORT_T* port, MMAL_BUFFER_HEADER_T* buffer)
        {
            if (MMALCameraConfig.Debug)            
                MMALLog.Logger.LogDebug($"{Name}: In native output callback");            
            
            var bufferImpl = new MMALBufferImpl(buffer);

            bufferImpl.PrintProperties();
            
            var failed = bufferImpl.AssertProperty(MMALBufferProperties.MMAL_BUFFER_HEADER_FLAG_TRANSMISSION_FAILED);

            var eos = bufferImpl.AssertProperty(MMALBufferProperties.MMAL_BUFFER_HEADER_FLAG_FRAME_END) ||
                      bufferImpl.AssertProperty(MMALBufferProperties.MMAL_BUFFER_HEADER_FLAG_EOS) ||
                      ComponentReference.ForceStopProcessing ||
                      bufferImpl.Length == 0;

            if ((bufferImpl.CheckState() && bufferImpl.Length > 0 && !eos && !failed && !Trigger.Task.IsCompleted) || (eos && !Trigger.Task.IsCompleted))
            {
                CallbackHandler.Callback(bufferImpl);
            }
            
            // Ensure we release the buffer before any signalling or we will cause a memory leak due to there still being a reference count on the buffer.
            ReleaseBuffer(bufferImpl, eos);

            // If this buffer signals the end of data stream, allow waiting thread to continue.
            if (eos || failed)
            {
                MMALLog.Logger.LogDebug($"{Name}: End of stream. Signaling completion...");
                
                Task.Run(() => { Trigger.SetResult(true); });
            }
        }
    }
}