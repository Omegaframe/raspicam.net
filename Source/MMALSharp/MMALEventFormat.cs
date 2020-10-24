using System;
using System.Runtime.InteropServices;
using MMALSharp.Common;
using MMALSharp.Native.Events;
using MMALSharp.Native.Format;

namespace MMALSharp
{
    unsafe class MmalEventFormat : IBufferEvent
    {
        public MmalEsFormat* Ptr { get; }

        public string FourCC => MmalEncodingHelpers.ParseEncoding(Format.Encoding).EncodingName;

        public int Bitrate => Format.Bitrate;

        public int Width => Format.Es->Video.Width;

        public int Height => Format.Es->Video.Height;

        public int CropX => Format.Es->Video.Crop.X;

        public int CropY => Format.Es->Video.Crop.Y;

        public int CropWidth => Format.Es->Video.Crop.Width;

        public int CropHeight => Format.Es->Video.Crop.Height;

        public int ParNum => Format.Es->Video.Par.Num;

        public int ParDen => Format.Es->Video.Par.Den;

        public int FramerateNum => Format.Es->Video.FrameRate.Num;

        public int FramerateDen => Format.Es->Video.FrameRate.Den;

        MmalEsFormat Format { get; }

        public MmalEventFormat(MmalEsFormat format)
        {
            Format = format;
        }

        public MmalEventFormat(MmalEsFormat format, MmalEsFormat* ptr)
        {
            Format = format;
            Ptr = ptr;
        }

        internal static MmalEventFormat GetEventFormat(IBuffer buffer)
        {
            var ev = MmalEvents.GetChanged(buffer.Ptr);
            return new MmalEventFormat(Marshal.PtrToStructure<MmalEsFormat>((IntPtr)ev->Format), ev->Format);
        }
    }
}
