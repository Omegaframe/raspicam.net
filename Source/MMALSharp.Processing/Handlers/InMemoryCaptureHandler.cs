using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MMALSharp.Common;
using MMALSharp.Common.Utility;
using MMALSharp.Processors;

namespace MMALSharp.Handlers
{
    /// <summary>
    /// A capture handler which stores its data to memory.
    /// </summary>
    public class InMemoryCaptureHandler : OutputCaptureHandler
    {
        private int _totalProcessed;

        /// <summary>
        /// The working data store.
        /// </summary>
        public List<byte> WorkingData { get; set; }
               
        /// <summary>
        /// Creates a new instance of <see cref="InMemoryCaptureHandler"/>.
        /// </summary>
        public InMemoryCaptureHandler()
        {
            WorkingData = new List<byte>();
        }
        
        /// <inheritdoc />
        public override void Dispose()
        {
            MMALLog.Logger.LogInformation($"Successfully processed {Helpers.ConvertBytesToMegabytes(_totalProcessed)}.");
        }
        
        /// <inheritdoc />
        public override void Process(ImageContext context)
        {
            WorkingData.AddRange(context.Data);
            _totalProcessed += context.Data.Length;
            base.Process(context);
        }

        /// <summary>
        /// Allows us to do any further processing once the capture method has completed. Note: It is the user's responsibility to 
        /// clear the WorkingData list after processing is complete.
        /// </summary>
        public override void PostProcess()
        {
            if (OnManipulate != null && ImageContext != null)
            {
                ImageContext.Data = WorkingData.ToArray();
                OnManipulate(new FrameProcessingContext(ImageContext));
                WorkingData = new List<byte>(ImageContext.Data);     
            }
        }

        /// <inheritdoc />
        public override string TotalProcessed()
        {
            return $"{_totalProcessed}";
        }
    }
}