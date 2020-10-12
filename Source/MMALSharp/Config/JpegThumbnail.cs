namespace MMALSharp.Config
{
    public class JpegThumbnail
    {
        public bool Enable { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Quality { get; set; }

        public JpegThumbnail(bool enable, int width, int height, int quality)
        {
            Enable = enable;
            Width = width;
            Height = height;
            Quality = quality;
        }
    }
}
