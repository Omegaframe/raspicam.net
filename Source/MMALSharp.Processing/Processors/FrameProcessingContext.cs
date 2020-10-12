using MMALSharp.Common;

namespace MMALSharp.Processing.Processors
{
    public class FrameProcessingContext : IFrameProcessingContext
    {
        readonly ImageContext _context;

        public FrameProcessingContext(ImageContext context)
        {
            _context = context;
        }

        public IFrameProcessingContext Apply(IFrameProcessor processor)
        {
            processor.Apply(_context);

            return this;
        }
    }
}
