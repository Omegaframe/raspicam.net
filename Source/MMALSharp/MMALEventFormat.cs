using MMALSharp.Native;
using System;
using System.Runtime.InteropServices;
using MMALSharp.Common;

namespace MMALSharp
{
    public unsafe class MmalEventFormat : IBufferEvent
    {
        public MMAL_ES_FORMAT_T* Ptr { get; }

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

        MMAL_ES_FORMAT_T Format { get; }

        public MmalEventFormat(MMAL_ES_FORMAT_T format)
        {
            Format = format;
        }

        public MmalEventFormat(MMAL_ES_FORMAT_T format, MMAL_ES_FORMAT_T* ptr)
        {
            Format = format;
            Ptr = ptr;
        }

        internal static MmalEventFormat GetEventFormat(IBuffer buffer)
        {
            var ev = MmalEvents.mmal_event_format_changed_get(buffer.Ptr);
            return new MmalEventFormat(Marshal.PtrToStructure<MMAL_ES_FORMAT_T>((IntPtr)ev->Format), ev->Format);
        }
    }
}
