using System;
using System.Drawing.Imaging;
using MMALSharp.Common;
using MMALSharp.Processing.Processors;

namespace MMALSharp.Processing.Handlers
{
    public abstract class OutputCaptureHandler : IOutputCaptureHandler
    {
        protected Action<IFrameProcessingContext> OnManipulate { get; set; }
        protected Func<IFrameProcessingContext, IFrameAnalyser> OnAnalyse { get; set; }
        protected ImageContext ImageContext { get; set; }
        protected ImageFormat StoreFormat { get; set; }
        protected int Processed { get; set; }

        public abstract string TotalProcessed();
        public virtual void PostProcess() { }
        public virtual void Process(ImageContext context)
        {
            ImageContext = context;
        }

        public void Manipulate(Action<IFrameProcessingContext> context, ImageFormat storeFormat)
        {
            OnManipulate = context;
            StoreFormat = storeFormat;
        }

        public abstract void Dispose();
    }
}