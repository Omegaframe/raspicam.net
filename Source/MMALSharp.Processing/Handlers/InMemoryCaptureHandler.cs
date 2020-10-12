using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MMALSharp.Common;
using MMALSharp.Common.Utility;
using MMALSharp.Processing.Processors;

namespace MMALSharp.Processing.Handlers
{
    public class InMemoryCaptureHandler : OutputCaptureHandler
    {
        public List<byte> WorkingData { get; set; }

        int _totalProcessed;

        public InMemoryCaptureHandler()
        {
            WorkingData = new List<byte>();
        }

        public override void Process(ImageContext context)
        {
            WorkingData.AddRange(context.Data);
            _totalProcessed += context.Data.Length;

            base.Process(context);
        }

        public override void PostProcess()
        {
            if (OnManipulate == null || ImageContext == null)
                return;

            ImageContext.Data = WorkingData.ToArray();
            OnManipulate(new FrameProcessingContext(ImageContext));
            WorkingData = new List<byte>(ImageContext.Data);
        }

        public override string TotalProcessed() => $"{_totalProcessed}";
        public override void Dispose() => MmalLog.Logger.LogInformation($"Successfully processed {Helpers.ConvertBytesToMegabytes(_totalProcessed)}.");
    }
}