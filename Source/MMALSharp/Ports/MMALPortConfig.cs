using System;
using System.Drawing;
using MMALSharp.Config;

namespace MMALSharp.Ports
{
    class MmalPortConfig : IMmalPortConfig
    {
        public MmalEncoding EncodingType { get; }
        public MmalEncoding PixelFormat { get; }
        public int Width { get; }
        public int Height { get; }
        public double Framerate { get; }
        public int Quality { get; }
        public int Bitrate { get; }
        public bool ZeroCopy { get; }
        public DateTime? Timeout { get; }
        public int BufferNum { get; }
        public int BufferSize { get; }
        public Rectangle? Crop { get; }
        public Split Split { get; }
        public bool StoreMotionVectors { get; }

        public MmalPortConfig(
            MmalEncoding encodingType,
            MmalEncoding pixelFormat,
            int quality = 0,
            int bitrate = 0,
            DateTime? timeout = null,
            Split split = null,
            bool storeMotionVectors = false,
            int width = 0,
            int height = 0,
            double framerate = 0,
            bool zeroCopy = false,
            int bufferNum = 0,
            int bufferSize = 0,
            Rectangle? crop = null)
        {
            EncodingType = encodingType;
            PixelFormat = pixelFormat;
            Width = width;
            Height = height;
            Framerate = framerate;
            Quality = quality;
            Bitrate = bitrate;
            ZeroCopy = zeroCopy;
            Timeout = timeout;
            Split = split;
            BufferNum = bufferNum;
            BufferSize = bufferSize;
            Crop = crop;
            StoreMotionVectors = storeMotionVectors;
        }
    }
}
