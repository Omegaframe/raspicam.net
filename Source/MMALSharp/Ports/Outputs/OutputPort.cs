using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MMALSharp.Callbacks;
using MMALSharp.Components;
using MMALSharp.Config;
using MMALSharp.Extensions;
using MMALSharp.Handlers;
using MMALSharp.Native.Buffer;
using MMALSharp.Native.Parameters;
using MMALSharp.Native.Port;
using MMALSharp.Native.Util;
using MMALSharp.Ports.Inputs;
using MMALSharp.Utility;

namespace MMALSharp.Ports.Outputs
{
    unsafe class OutputPort : PortBase<IOutputCallbackHandler>, IOutputPort
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

        public OutputPort(IntPtr ptr, IComponent comp, Guid guid) : base(ptr, comp, PortType.Output, guid) { }

        public virtual void Configure(IMmalPortConfig config, IInputPort copyFrom, ICaptureHandler handler)
        {
            if (config != null)
            {
                PortConfig = config;

                copyFrom?.ShallowCopy(this);

                if (config.EncodingType != null)
                    NativeEncodingType = config.EncodingType.EncodingVal;

                if (config.PixelFormat != null)
                    NativeEncodingSubformat = config.PixelFormat.EncodingVal;

                Par = new MmalRational(1, 1);

                var tempVid = Ptr->Format->Es->Video;

                try
                {
                    Commit();
                }
                catch
                {
                    // If commit fails using new settings, attempt to reset using old temp MMAL_VIDEO_FORMAT_T.
                    MmalLog.Logger.LogWarning($"{Name}: Commit of output port failed. Attempting to reset values.");
                    Ptr->Format->Es->Video = tempVid;
                    Commit();
                }

                if (config.ZeroCopy)
                {
                    ZeroCopy = true;
                    this.SetParameter(MmalParametersCommon.MmalParameterZeroCopy, true);
                }

                if (MmalCameraConfig.VideoColorSpace != null && MmalCameraConfig.VideoColorSpace.EncType == MmalEncoding.EncodingType.ColorSpace)
                    VideoColorSpace = MmalCameraConfig.VideoColorSpace;

                if (config.Framerate > 0)
                    FrameRate = config.Framerate;

                if (config.Bitrate > 0)
                    Bitrate = config.Bitrate;

                EncodingType = config.EncodingType;
                PixelFormat = config.PixelFormat;

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

                BufferNum = Math.Max(BufferNumMin, config.BufferNum > 0 ? config.BufferNum : BufferNumRecommended);
                BufferSize = Math.Max(BufferSizeMin, config.BufferSize > 0 ? config.BufferSize : BufferSizeRecommended);

                // It is important to re-commit changes to width and height.
                Commit();
            }

            CallbackHandler = new DefaultOutputPortCallbackHandler(this, handler);
        }

        public virtual IConnection ConnectTo(IDownstreamComponent destinationComponent, int inputPort = 0, bool useCallback = false)
        {
            if (ConnectedReference != null)
            {
                MmalLog.Logger.LogWarning($"{Name}: A connection has already been established on this port");
                return ConnectedReference;
            }

            var connection = MmalConnectionImpl.CreateConnection(this, destinationComponent.Inputs[inputPort], destinationComponent, useCallback);
            ConnectedReference = connection;

            destinationComponent.Inputs[inputPort].ConnectTo(this, connection);

            return connection;
        }

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
                if (MmalCameraConfig.Debug)
                {
                    if (!Enabled)
                        MmalLog.Logger.LogDebug($"{Name}: Port not enabled.");

                    if (BufferPool == null)
                        MmalLog.Logger.LogDebug($"{Name}: Buffer pool null.");
                }

                if (Enabled && BufferPool != null)
                {
                    newBuffer = BufferPool.Queue.GetBuffer();

                    if (newBuffer.CheckState())
                        SendBuffer(newBuffer);
                    else
                        MmalLog.Logger.LogWarning($"{Name}: Buffer null. Continuing.");
                }
            }
            catch (Exception e)
            {
                if (newBuffer != null && newBuffer.CheckState())
                    newBuffer.Release();

                MmalLog.Logger.LogWarning($"{Name}: Unable to send buffer header. {e.Message}");
            }
        }

        public void RegisterCallbackHandler(IOutputCallbackHandler callbackHandler)
        {
            CallbackHandler = callbackHandler;
        }

        public virtual void Enable()
        {
            if (Enabled)
                return;

            NativeCallback = NativeOutputPortCallback;

            var ptrCallback = Marshal.GetFunctionPointerForDelegate(NativeCallback);
            PtrCallback = ptrCallback;

            if (CallbackHandler == null)
            {
                MmalLog.Logger.LogWarning($"{Name}: Callback null");

                EnablePort(IntPtr.Zero);
            }
            else
            {
                EnablePort(ptrCallback);
            }

            if (CallbackHandler != null)
                SendAllBuffers();

            if (!Enabled)
            {
                throw new PiCameraError($"{Name}: Unknown error occurred whilst enabling port");
            }
        }

        public void Start()
        {
            MmalLog.Logger.LogDebug($"{Name}: Starting output port.");
            Trigger = new TaskCompletionSource<bool>();
            Enable();
        }

        internal virtual void NativeOutputPortCallback(MmalPortType* port, MmalBufferHeader* buffer)
        {
            if (MmalCameraConfig.Debug)
                MmalLog.Logger.LogDebug($"{Name}: In native output callback");

            var bufferImpl = new MmalBuffer(buffer);

            bufferImpl.PrintProperties();

            var failed = bufferImpl.AssertProperty(MmalBufferProperties.MmalBufferHeaderFlagTransmissionFailed);

            var eos = bufferImpl.AssertProperty(MmalBufferProperties.MmalBufferHeaderFlagFrameEnd) ||
                      bufferImpl.AssertProperty(MmalBufferProperties.MmalBufferHeaderFlagEos) ||
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
                MmalLog.Logger.LogDebug($"{Name}: End of stream. Signaling completion...");

                Task.Run(() => { Trigger.SetResult(true); });
            }
        }
    }
}