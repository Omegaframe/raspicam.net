using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MMALSharp.Common;
using MMALSharp.Common.Utility;

namespace MMALSharp.Processing.Processors
{
    public abstract class FrameAnalyser : IFrameAnalyser
    {
        protected List<byte> WorkingData { get; set; }
        protected bool FullFrame { get; set; }

        protected FrameAnalyser()
        {
            WorkingData = new List<byte>();
        }

        public virtual void Apply(ImageContext context)
        {
            if (FullFrame)
            {
                MmalLog.Logger.LogDebug("Clearing frame");
                WorkingData.Clear();
                FullFrame = false;
            }

            WorkingData.AddRange(context.Data);

            if (context.IsEos)
                FullFrame = true;
        }
    }
}
