using MMALSharp.Config;

namespace MMALSharp.Mmal.Components.EncoderComponents
{
    interface IImageEncoder : IEncoder
    {
        bool RawBayer { get; }
        bool UseExif { get; }
        ExifTag[] ExifTags { get; }
        bool ContinuousCapture { get; }
        JpegThumbnail JpegThumbnailConfig { get; set; }
    }
}
