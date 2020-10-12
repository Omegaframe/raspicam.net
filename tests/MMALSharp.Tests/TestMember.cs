using MMALSharp.Common;
using MMALSharp.Native;

namespace MMALSharp.Tests
{
    public class TestMember
    {
        public string Extension { get; set; }
        public MmalEncoding EncodingType { get; set; }
        public MmalEncoding PixelFormat { get; set; }

        public TestMember(string extension, MmalEncoding encodingType, MmalEncoding pixelFormat)
        {
            this.Extension = extension;
            this.EncodingType = encodingType;
            this.PixelFormat = pixelFormat;
        }
    }
}