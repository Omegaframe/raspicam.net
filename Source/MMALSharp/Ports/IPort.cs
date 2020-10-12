using System;
using System.Drawing;
using System.Threading.Tasks;
using MMALSharp.Common;
using MMALSharp.Common.Utility;
using MMALSharp.Components;
using MMALSharp.Native;
using MMALSharp.Native.Format;
using MMALSharp.Native.Port;
using MMALSharp.Native.Util;

namespace MMALSharp.Ports
{
    public interface IPort : IMmalObject
    {
        unsafe MmalPortType* Ptr { get; }
        PortType PortType { get; }
        IComponent ComponentReference { get; }
        IConnection ConnectedReference { get; }
        IBufferPool BufferPool { get; }
        Guid Guid { get; }
        MmalEncoding EncodingType { get; }
        MmalEncoding PixelFormat { get; }
        IMmalPortConfig PortConfig { get; }
        string Name { get; }
        bool Enabled { get; }
        int BufferNumMin { get; }
        int BufferSizeMin { get; }
        int BufferAlignmentMin { get; }
        int BufferNumRecommended { get; }
        int BufferSizeRecommended { get; }
        int BufferNum { get; }
        int BufferSize { get; }
        MmalEsFormat Format { get; }
        Resolution Resolution { get; }
        Rectangle Crop { get; }
        double FrameRate { get; }
        MmalRational FrameRateRational { get; }
        MmalEncoding VideoColorSpace { get; }
        int CropWidth { get; }
        int CropHeight { get; }
        MmalFormat.MmalEsTypeT FormatType { get; }
        int NativeEncodingType { get; }
        int NativeEncodingSubformat { get; }
        int Bitrate { get; }
        MmalRational Par { get; }
        bool ZeroCopy { get; set; }
        TaskCompletionSource<bool> Trigger { get; }
        void EnablePort(IntPtr callback);

        void DisablePort();
        void Commit();
        void ShallowCopy(IPort destination);

        void ShallowCopy(IBufferEvent eventFormatSource);
        void FullCopy(IPort destination);
        void FullCopy(IBufferEvent eventFormatSource);
        void Flush();
        void SendBuffer(IBuffer buffer);
        void SendAllBuffers();
        void SendAllBuffers(IBufferPool pool);
        void DestroyPortPool();
        void InitialiseBufferPool();
        void ExtraDataAlloc(int size);
        void CloseConnection();
    }
}
