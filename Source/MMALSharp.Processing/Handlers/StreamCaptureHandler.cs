using System;
using System.IO;
using Microsoft.Extensions.Logging;
using MMALSharp.Common;
using MMALSharp.Common.Utility;
using MMALSharp.Processing.Processors;

namespace MMALSharp.Processing.Handlers
{
    public abstract class StreamCaptureHandler<T> : OutputCaptureHandler where T : Stream
    {
        public T CurrentStream { get; protected set; }

        public override void Process(ImageContext context)
        {
            Processed += context.Data.Length;

            if (CurrentStream.CanWrite)
                CurrentStream.Write(context.Data, 0, context.Data.Length);
            else
                throw new IOException("Stream not writable.");

            base.Process(context);
        }

        public override void PostProcess()
        {
            try
            {
                if (CurrentStream == null || !CurrentStream.CanRead || CurrentStream.Length <= 0)
                    return;

                if (OnManipulate == null || ImageContext == null)
                    return;

                using (var ms = new MemoryStream())
                {
                    CurrentStream.Position = 0;
                    CurrentStream.CopyTo(ms);

                    var arr = ms.ToArray();

                    ImageContext.Data = arr;
                    ImageContext.StoreFormat = StoreFormat;

                    MmalLog.Logger.LogDebug("Applying image processing.");

                    OnManipulate(new FrameProcessingContext(ImageContext));
                }

                using (var ms = new MemoryStream(ImageContext.Data))
                {
                    CurrentStream.SetLength(0);
                    CurrentStream.Position = 0;
                    ms.CopyTo(CurrentStream);
                }
            }
            catch (Exception e)
            {
                MmalLog.Logger.LogWarning($"Something went wrong while processing stream: {e.Message}. {e.InnerException?.Message}. {e.StackTrace}");
            }
        }

        public override string TotalProcessed() => $"{Helpers.ConvertBytesToMegabytes(Processed)}";

        public override void Dispose()
        {
            MmalLog.Logger.LogInformation($"Successfully processed {Helpers.ConvertBytesToMegabytes(Processed)}.");
            CurrentStream?.Dispose();
        }
    }
}
