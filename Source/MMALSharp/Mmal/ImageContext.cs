using MMALSharp.Config;

namespace MMALSharp.Mmal
{
    class ImageContext
    {
        public byte[] Data { get; set; }
        public bool IsRaw { get; set; }
        public Resolution Resolution { get; set; }
        public MmalEncoding Encoding { get; set; }
        public MmalEncoding PixelFormat { get; set; }
        public bool IsEos { get; set; }
        public bool IsIFrame { get; set; }
        public long? Pts { get; set; }
        public int Stride { get; set; }
    }
}