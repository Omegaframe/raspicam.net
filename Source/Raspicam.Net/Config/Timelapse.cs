using System.Threading;

namespace MMALSharp.Config
{
    public class Timelapse
    {
        public TimelapseMode Mode { get; set; }
        public CancellationToken CancellationToken { get; set; }
        public int Value { get; set; }
    }
}
