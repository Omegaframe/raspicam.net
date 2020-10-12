using MMALSharp.Common;
using MMALSharp.Common.Utility;

namespace MMALSharp.Config
{
    public class PreviewOverlayConfiguration : PreviewConfiguration
    {
        public Resolution Resolution { get; set; }
        public MmalEncoding Encoding { get; set; }
    }
}
