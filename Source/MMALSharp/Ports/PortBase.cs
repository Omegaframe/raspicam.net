using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MMALSharp.Callbacks;
using MMALSharp.Common;
using MMALSharp.Common.Utility;
using MMALSharp.Components;
using MMALSharp.Native;
using static MMALSharp.MMALNativeExceptionHelper;

namespace MMALSharp.Ports
{
    public abstract unsafe class PortBase<TCallback> : MMALObject, IPort where TCallback : ICallbackHandler
    {
        /// <summary>
        /// The callback handler associated with this port.
        /// </summary>
        public TCallback CallbackHandler { get; internal set; }

        /// <summary>
        /// Specifies the type of port this is.
        /// </summary>
        public PortType PortType { get; }

        /// <summary>
        /// Managed reference to the component this port is associated with.
        /// </summary>
        public IComponent ComponentReference { get; }

        /// <summary>
        /// Managed reference to the downstream component this port is connected to.
        /// </summary>
        public IConnection ConnectedReference { get; internal set; }

        /// <summary>
        /// Managed reference to the pool of buffer headers associated with this port.
        /// </summary>
        public IBufferPool BufferPool { get; internal set; }

        /// <summary>
        /// User defined identifier given to this object.
        /// </summary>
        public Guid Guid { get; }

        /// <summary>
        /// The MMALEncoding encoding type that this port will process data in. Helpful for retrieving encoding name/FourCC value.
        /// </summary>
        public MmalEncoding EncodingType { get; internal set; }

        /// <summary>
        /// The MMALEncoding pixel format that this port will process data in. Helpful for retrieving encoding name/FourCC value.
        /// </summary>
        public MmalEncoding PixelFormat { get; internal set; }

        /// <summary>
        /// The config for this port.
        /// </summary>
        public IMMALPortConfig PortConfig { get; internal set; }

        /// <summary>
        /// Indicates whether ZeroCopy mode should be enabled on this port. When enabled, data is not copied to the ARM processor and is handled directly by the GPU. Useful when
        /// transferring large amounts of data or raw capture.
        /// See: https://www.raspberrypi.org/forums/viewtopic.php?t=170024
        /// </summary>
        public bool ZeroCopy { get; set; }

        /// <summary>
        /// Asynchronous trigger which is set when processing has completed on this port.
        /// </summary>
        public TaskCompletionSource<bool> Trigger { get; internal set; }

        /// <summary>
        /// Native pointer that represents this port.
        /// </summary>
        public MMAL_PORT_T* Ptr { get; }

        /// <summary>
        /// Native name of port.
        /// </summary>
        public string Name => Marshal.PtrToStringAnsi((IntPtr)Ptr->Name);

        /// <summary>
        /// Indicates whether this port is enabled.
        /// </summary>
        public bool Enabled => Ptr->IsEnabled == 1;

        /// <summary>
        /// Specifies minimum number of buffer headers required for this port.
        /// </summary>
        public int BufferNumMin => Ptr->BufferNumMin;

        /// <summary>
        /// Specifies minimum size of buffer headers required for this port.
        /// </summary>
        public int BufferSizeMin => Ptr->BufferSizeMin;

        /// <summary>
        /// Specifies minimum alignment value for buffer headers required for this port.
        /// </summary>
        public int BufferAlignmentMin => Ptr->BufferAlignmentMin;

        /// <summary>
        /// Specifies recommended number of buffer headers for this port.
        /// </summary>
        public int BufferNumRecommended => Ptr->BufferNumRecommended;

        /// <summary>
        /// Specifies recommended size of buffer headers for this port.
        /// </summary>
        public int BufferSizeRecommended => Ptr->BufferSizeRecommended;

        /// <summary>
        /// Indicates the currently set number of buffer headers for this port.
        /// </summary>
        public int BufferNum
        {
            get => Ptr->BufferNum;
            internal set => Ptr->BufferNum = value;
        }

        /// <summary>
        /// Indicates the currently set size of buffer headers for this port.
        /// </summary>
        public int BufferSize
        {
            get => Ptr->BufferSize;
            internal set => Ptr->BufferSize = value;
        }

        /// <summary>
        /// Accessor for the elementary stream.
        /// </summary>
        public MMAL_ES_FORMAT_T Format => *Ptr->Format;

        /// <summary>
        /// The Width/Height that this port will process data in.
        /// </summary>
        public abstract Resolution Resolution { get; internal set; }

        /// <summary>
        /// The region of interest that this port will process data in.
        /// </summary>
        public Rectangle Crop
        {
            get => new Rectangle(Ptr->Format->Es->Video.Crop.X, Ptr->Format->Es->Video.Crop.Y, Ptr->Format->Es->Video.Crop.Width, Ptr->Format->Es->Video.Crop.Height);
            internal set => Ptr->Format->Es->Video.Crop = new MMAL_RECT_T(value.X, value.Y, value.Width, value.Height);
        }

        /// <summary>
        /// The framerate we are processing data in.
        /// </summary>
        public double FrameRate
        {
            get => Ptr->Format->Es->Video.FrameRate.Num;
            internal set => Ptr->Format->Es->Video.FrameRate = new MMAL_RATIONAL_T(value);
        }

        /// <summary>
        /// The framerate represented as a <see cref="MMAL_RATIONAL_T"/>.
        /// </summary>
        public MMAL_RATIONAL_T FrameRateRational
        {
            get => Ptr->Format->Es->Video.FrameRate;
        }

        /// <summary>
        /// The working video color space, specific to video ports.
        /// </summary>
        public MmalEncoding VideoColorSpace
        {
            get => Ptr->Format->Es->Video.ColorSpace.ParseEncoding();
            internal set => Ptr->Format->Es->Video.ColorSpace = value.EncodingVal;
        }

        /// <summary>
        /// The Region of Interest width that this port will process data in.
        /// </summary>
        public int CropWidth => Ptr->Format->Es->Video.Crop.Width;

        /// <summary>
        /// The Region of Interest height that this port will process data in.
        /// </summary>
        public int CropHeight => Ptr->Format->Es->Video.Crop.Height;

        /// <summary>
        /// Query / Set the port domain type.
        /// </summary>
        public MMALFormat.MMAL_ES_TYPE_T FormatType
        {
            get => Ptr->Format->Type;
            internal set => Ptr->Format->Type = value;
        }

        /// <summary>
        /// The encoding type that this port will process data in.
        /// </summary>
        public int NativeEncodingType
        {
            get => Ptr->Format->Encoding;
            internal set => Ptr->Format->Encoding = value;
        }

        /// <summary>
        /// The pixel format that this port will process data in.
        /// </summary>
        public int NativeEncodingSubformat
        {
            get => Ptr->Format->EncodingVariant;
            internal set => Ptr->Format->EncodingVariant = value;
        }

        /// <summary>
        /// The working bitrate of this port.
        /// </summary>
        public int Bitrate
        {
            get => Ptr->Format->Bitrate;
            internal set => Ptr->Format->Bitrate = value;
        }

        /// <summary>
        /// The working aspect ratio of this port.
        /// </summary>
        public MMAL_RATIONAL_T Par
        {
            get => Ptr->Format->Es->Video.Par;
            internal set => Ptr->Format->Es->Video.Par = value;
        }

        internal int Width
        {
            get => Ptr->Format->Es->Video.Width;
            set => Ptr->Format->Es->Video.Width = value;
        }

        internal int Height
        {
            get => Ptr->Format->Es->Video.Height;
            set => Ptr->Format->Es->Video.Height = value;
        }

        /// <summary>
        /// Native pointer that represents the component this port is associated with.
        /// </summary>
        internal MMAL_COMPONENT_T* Comp { get; }

        /// <summary>
        /// Native pointer to the native callback function.
        /// </summary>
        internal IntPtr PtrCallback { get; set; }

        /// <summary>
        /// Delegate for native port callback.
        /// </summary>
        internal MMALPort.MMAL_PORT_BH_CB_T NativeCallback { get; set; }

        /// <inheritdoc />
        public override bool CheckState()
        {
            return Ptr != null && (IntPtr)Ptr != IntPtr.Zero;
        }

        /// <summary>
        /// Creates a new managed reference to a MMAL Component Port.
        /// </summary>
        /// <param name="ptr">The native pointer to the component port.</param>
        /// <param name="comp">The component this port is associated with.</param>
        /// <param name="type">The type of port this is.</param>
        /// <param name="guid">A managed unique identifier for this port.</param>
        protected PortBase(IntPtr ptr, IComponent comp, PortType type, Guid guid)
        {
            Ptr = (MMAL_PORT_T*)ptr;
            Comp = ((MMAL_PORT_T*)ptr)->Component;
            ComponentReference = comp;
            PortType = type;
            Guid = guid;
        }

        /// <summary>
        /// Enables the specified port.
        /// </summary>
        /// <param name="callback">The function pointer MMAL will call back to.</param>
        /// <exception cref="MMALException"/>
        public void EnablePort(IntPtr callback)
        {
            MmalLog.Logger.LogDebug($"{Name}: Enabling port.");
            MMALCheck(MMALPort.mmal_port_enable(Ptr, callback), $"{Name}: Unable to enable port.");
        }

        /// <summary>
        /// Disable processing on a port. Disabling a port will stop all processing on this port and return all (non-processed)
        /// buffer headers to the client. If this is a connected output port, the input port to which it is connected shall also be disabled.
        /// Any buffer pool shall be released.
        /// </summary>
        /// <exception cref="MMALException"/>
        public void DisablePort()
        {
            if (Enabled)
            {
                MmalLog.Logger.LogDebug($"{Name}: Disabling port.");
                MMALCheck(MMALPort.mmal_port_disable(Ptr), $"{Name}: Unable to disable port.");
            }
        }

        /// <summary>
        /// Commit format changes on this port.
        /// </summary>
        /// <exception cref="MMALException"/>
        public void Commit()
        {
            MmalLog.Logger.LogDebug($"{Name}: Committing port format changes.");
            MMALCheck(MMALPort.mmal_port_format_commit(Ptr), $"{Name}: Unable to commit port changes.");
        }

        /// <summary>
        /// Shallow copy a format structure. It is worth noting that the extradata buffer will not be copied in the new format.
        /// </summary>
        /// <param name="destination">The destination port we're copying to.</param>
        public void ShallowCopy(IPort destination)
        {
            MmalLog.Logger.LogDebug($"{Name}: Shallow copy port format.");
            MMALFormat.mmal_format_copy(destination.Ptr->Format, Ptr->Format);
        }

        /// <summary>
        /// Shallow copy a format structure. It is worth noting that the extradata buffer will not be copied in the new format.
        /// </summary>
        /// <param name="eventFormatSource">The source event format we're copying from.</param>
        public void ShallowCopy(IBufferEvent eventFormatSource)
        {
            MmalLog.Logger.LogDebug($"{Name}: Shallow copy event format.");
            MMALFormat.mmal_format_copy(Ptr->Format, eventFormatSource.Ptr);
        }

        /// <summary>
        /// Fully copy a format structure, including the extradata buffer.
        /// </summary>
        /// <param name="destination">The destination port we're copying to.</param>
        public void FullCopy(IPort destination)
        {
            MmalLog.Logger.LogDebug($"{Name}: Full copy port format.");
            MMALFormat.mmal_format_full_copy(destination.Ptr->Format, Ptr->Format);
        }

        /// <summary>
        /// Fully copy a format structure, including the extradata buffer.
        /// </summary>
        /// <param name="eventFormatSource">The source event format we're copying from.</param>
        public void FullCopy(IBufferEvent eventFormatSource)
        {
            MmalLog.Logger.LogDebug($"{Name}: Full copy event format.");
            MMALFormat.mmal_format_full_copy(Ptr->Format, eventFormatSource.Ptr);
        }

        /// <summary>
        /// Ask a port to release all the buffer headers it currently has. This is an asynchronous operation and the
        /// flush call will return before all the buffer headers are returned to the client.
        /// </summary>
        /// <exception cref="MMALException"/>
        public void Flush()
        {
            MmalLog.Logger.LogDebug($"{Name}: Flushing port buffers");
            MMALCheck(MMALPort.mmal_port_flush(Ptr), $"{Name}: Unable to flush port.");
        }

        /// <summary>
        /// Send a buffer header to a port.
        /// </summary>
        /// <param name="buffer">A managed buffer object.</param>
        /// <exception cref="MMALException"/>
        public void SendBuffer(IBuffer buffer)
        {
            if (!Enabled)
                return;

            if (MMALCameraConfig.Debug)
                MmalLog.Logger.LogDebug($"{Name}: Sending buffer start.");

            MMALCheck(MMALPort.mmal_port_send_buffer(Ptr, buffer.Ptr), $"{Name}: Unable to send buffer header.");

            if (MMALCameraConfig.Debug)
                MmalLog.Logger.LogDebug($"{Name}: Sending buffer complete.");
        }

        /// <summary>
        /// Attempts to send all available buffers in the queue to this port.
        /// </summary>
        public void SendAllBuffers()
        {
            InitialiseBufferPool();

            var length = BufferPool.Queue.QueueLength();

            for (int i = 0; i < length; i++)
            {
                var buffer = BufferPool.Queue.GetBuffer();

                MmalLog.Logger.LogDebug($"{Name}: Sending buffer to output port: Length {buffer.Length}.");

                SendBuffer(buffer);
            }
        }

        /// <summary>
        /// Attempts to send all available buffers in the specified pool's queue to this port.
        /// </summary>
        /// <param name="pool">The specified pool.</param>
        public void SendAllBuffers(IBufferPool pool)
        {
            var length = pool.Queue.QueueLength();

            for (int i = 0; i < length; i++)
            {
                var buffer = pool.Queue.GetBuffer();

                MmalLog.Logger.LogDebug($"{Name}: Sending buffer to output port: Length {buffer.Length}.");

                SendBuffer(buffer);
            }
        }

        /// <summary>
        /// Destroy a pool of MMAL_BUFFER_HEADER_T associated with a specific port. This will also deallocate all of the memory
        /// which was allocated when creating or resizing the pool.
        /// </summary>
        public void DestroyPortPool()
        {
            if (BufferPool != null && !BufferPool.IsDisposed)
            {
                DisablePort();

                MmalLog.Logger.LogDebug($"{Name}: Releasing active buffers.");
                while (BufferPool.Queue.QueueLength() < BufferPool.HeadersNum)
                {
                    var tempBuf = BufferPool.Queue.TimedWait(1000);

                    if (tempBuf != null)
                    {
                        tempBuf.Release();
                    }
                    else
                    {
                        MmalLog.Logger.LogWarning($"{Name}: Attempted to release buffer but retrieved null.");
                    }
                }

                BufferPool.Dispose();
            }
            else
            {
                MmalLog.Logger.LogDebug($"{Name}: Buffer pool already null or disposed of.");
            }
        }

        /// <summary>
        /// Initialises a new buffer pool.
        /// </summary>
        public void InitialiseBufferPool()
        {
            MmalLog.Logger.LogDebug($"{Name}: Initialising buffer pool.");
            BufferPool = new MMALPoolImpl(this);
        }

        /// <summary>
        /// To be called once connection has been disposed of.
        /// </summary>
        public void CloseConnection()
        {
            ConnectedReference = null;
        }

        /// <summary>
        /// Attempts to allocate the native extradata store with the given size.
        /// </summary>
        /// <param name="size">The size to allocate.</param>
        /// <exception cref="MMALException"/>
        public void ExtraDataAlloc(int size)
        {
            MMALCheck(MMALFormat.mmal_format_extradata_alloc(Ptr->Format, (uint)size), $"{Name}: Unable to alloc extradata.");
        }
    }
}
