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
using MMALSharp.Native.Component;
using MMALSharp.Native.Format;
using static MMALSharp.MmalNativeExceptionHelper;

namespace MMALSharp.Ports
{
    public abstract unsafe class PortBase<TCallback> : MmalObject, IPort where TCallback : ICallbackHandler
    {
        public TCallback CallbackHandler { get; internal set; }
        public PortType PortType { get; }
        public IComponent ComponentReference { get; }
        public IConnection ConnectedReference { get; internal set; }
        public IBufferPool BufferPool { get; internal set; }
        public Guid Guid { get; }
        public MmalEncoding EncodingType { get; internal set; }
        public MmalEncoding PixelFormat { get; internal set; }
        public IMmalPortConfig PortConfig { get; internal set; }
        public bool ZeroCopy { get; set; }
        public TaskCompletionSource<bool> Trigger { get; internal set; }
        public MMAL_PORT_T* Ptr { get; }
        public string Name => Marshal.PtrToStringAnsi((IntPtr)Ptr->Name);
        public bool Enabled => Ptr->IsEnabled == 1;
        public int BufferNumMin => Ptr->BufferNumMin;
        public int BufferSizeMin => Ptr->BufferSizeMin;
        public int BufferAlignmentMin => Ptr->BufferAlignmentMin;
        public int BufferNumRecommended => Ptr->BufferNumRecommended;
        public int BufferSizeRecommended => Ptr->BufferSizeRecommended;

        public int BufferNum
        {
            get => Ptr->BufferNum;
            internal set => Ptr->BufferNum = value;
        }

        public int BufferSize
        {
            get => Ptr->BufferSize;
            internal set => Ptr->BufferSize = value;
        }

        public MmalEsFormat Format => *Ptr->Format;

        public abstract Resolution Resolution { get; internal set; }

        public Rectangle Crop
        {
            get => new Rectangle(Ptr->Format->Es->Video.Crop.X, Ptr->Format->Es->Video.Crop.Y, Ptr->Format->Es->Video.Crop.Width, Ptr->Format->Es->Video.Crop.Height);
            internal set => Ptr->Format->Es->Video.Crop = new MMAL_RECT_T(value.X, value.Y, value.Width, value.Height);
        }

        public double FrameRate
        {
            get => Ptr->Format->Es->Video.FrameRate.Num;
            internal set => Ptr->Format->Es->Video.FrameRate = new MMAL_RATIONAL_T(value);
        }

        public MMAL_RATIONAL_T FrameRateRational
        {
            get => Ptr->Format->Es->Video.FrameRate;
        }

        public MmalEncoding VideoColorSpace
        {
            get => Ptr->Format->Es->Video.ColorSpace.ParseEncoding();
            internal set => Ptr->Format->Es->Video.ColorSpace = value.EncodingVal;
        }

        public int CropWidth => Ptr->Format->Es->Video.Crop.Width;

        public int CropHeight => Ptr->Format->Es->Video.Crop.Height;

        public MmalFormat.MmalEsTypeT FormatType
        {
            get => Ptr->Format->Type;
            internal set => Ptr->Format->Type = value;
        }

        public int NativeEncodingType
        {
            get => Ptr->Format->Encoding;
            internal set => Ptr->Format->Encoding = value;
        }

        public int NativeEncodingSubformat
        {
            get => Ptr->Format->EncodingVariant;
            internal set => Ptr->Format->EncodingVariant = value;
        }

        public int Bitrate
        {
            get => Ptr->Format->Bitrate;
            internal set => Ptr->Format->Bitrate = value;
        }

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

        internal MmalComponentType* Comp { get; }

        internal IntPtr PtrCallback { get; set; }

        internal MmalPort.MMAL_PORT_BH_CB_T NativeCallback { get; set; }

        public override bool CheckState()
        {
            return Ptr != null && (IntPtr)Ptr != IntPtr.Zero;
        }

        protected PortBase(IntPtr ptr, IComponent comp, PortType type, Guid guid)
        {
            Ptr = (MMAL_PORT_T*)ptr;
            Comp = ((MMAL_PORT_T*)ptr)->Component;
            ComponentReference = comp;
            PortType = type;
            Guid = guid;
        }

        public void EnablePort(IntPtr callback)
        {
            MmalLog.Logger.LogDebug($"{Name}: Enabling port.");
            MmalCheck(MmalPort.mmal_port_enable(Ptr, callback), $"{Name}: Unable to enable port.");
        }

        public void DisablePort()
        {
            if (!Enabled)
                return;

            MmalLog.Logger.LogDebug($"{Name}: Disabling port.");
            MmalCheck(MmalPort.mmal_port_disable(Ptr), $"{Name}: Unable to disable port.");
        }

        public void Commit()
        {
            MmalLog.Logger.LogDebug($"{Name}: Committing port format changes.");
            MmalCheck(MmalPort.mmal_port_format_commit(Ptr), $"{Name}: Unable to commit port changes.");
        }

        public void ShallowCopy(IPort destination)
        {
            MmalLog.Logger.LogDebug($"{Name}: Shallow copy port format.");
            MmalFormat.CopyFormat(destination.Ptr->Format, Ptr->Format);
        }

        public void ShallowCopy(IBufferEvent eventFormatSource)
        {
            MmalLog.Logger.LogDebug($"{Name}: Shallow copy event format.");
            MmalFormat.CopyFormat(Ptr->Format, eventFormatSource.Ptr);
        }

        public void FullCopy(IPort destination)
        {
            MmalLog.Logger.LogDebug($"{Name}: Full copy port format.");
            MmalFormat.CopyFull(destination.Ptr->Format, Ptr->Format);
        }

        public void FullCopy(IBufferEvent eventFormatSource)
        {
            MmalLog.Logger.LogDebug($"{Name}: Full copy event format.");
            MmalFormat.CopyFull(Ptr->Format, eventFormatSource.Ptr);
        }

        public void Flush()
        {
            MmalLog.Logger.LogDebug($"{Name}: Flushing port buffers");
            MmalCheck(MmalPort.mmal_port_flush(Ptr), $"{Name}: Unable to flush port.");
        }

        public void SendBuffer(IBuffer buffer)
        {
            if (!Enabled)
                return;

            if (MmalCameraConfig.Debug)
                MmalLog.Logger.LogDebug($"{Name}: Sending buffer start.");

            MmalCheck(MmalPort.mmal_port_send_buffer(Ptr, buffer.Ptr), $"{Name}: Unable to send buffer header.");

            if (MmalCameraConfig.Debug)
                MmalLog.Logger.LogDebug($"{Name}: Sending buffer complete.");
        }

        public void SendAllBuffers()
        {
            InitialiseBufferPool();

            var length = BufferPool.Queue.QueueLength();

            for (var i = 0; i < length; i++)
            {
                var buffer = BufferPool.Queue.GetBuffer();

                MmalLog.Logger.LogDebug($"{Name}: Sending buffer to output port: Length {buffer.Length}.");

                SendBuffer(buffer);
            }
        }

        public void SendAllBuffers(IBufferPool pool)
        {
            var length = pool.Queue.QueueLength();

            for (var i = 0; i < length; i++)
            {
                var buffer = pool.Queue.GetBuffer();

                MmalLog.Logger.LogDebug($"{Name}: Sending buffer to output port: Length {buffer.Length}.");

                SendBuffer(buffer);
            }
        }

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

        public void InitialiseBufferPool()
        {
            MmalLog.Logger.LogDebug($"{Name}: Initialising buffer pool.");
            BufferPool = new MmalPoolImpl(this);
        }

        public void CloseConnection()
        {
            ConnectedReference = null;
        }

        public void ExtraDataAlloc(int size)
        {
            MmalCheck(MmalFormat.AllocExtradata(Ptr->Format, (uint)size), $"{Name}: Unable to alloc extradata.");
        }
    }
}
