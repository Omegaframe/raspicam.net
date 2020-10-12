using System.Drawing;
using MMALSharp.Native;

namespace MMALSharp.Config
{
    public class PreviewConfiguration
    {
        public bool FullScreen { get; set; } = true;
        public bool NoAspect { get; set; }
        public bool CopyProtect { get; set; }
        public Rectangle PreviewWindow { get; set; } = new Rectangle(0, 0, 1024, 768);
        public int Opacity { get; set; } = 255;
        public int Layer { get; set; } = 2;
        public MmalParametersVideo.MmalDisplaytransformT DisplayTransform { get; set; }
        public MmalParametersVideo.MmalDisplaymodeT DisplayMode { get; set; }
    }
}
