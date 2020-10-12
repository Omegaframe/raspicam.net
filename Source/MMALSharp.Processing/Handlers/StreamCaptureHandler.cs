using System;
using System.IO;
using Microsoft.Extensions.Logging;
using MMALSharp.Common;
using MMALSharp.Common.Utility;
using MMALSharp.Processors;

namespace MMALSharp.Handlers
{
    /// <summary>
    /// Processes the image data to a stream.
    /// </summary>
    /// <typeparam name="T">The <see cref="Stream"/> type.</typeparam>
    public abstract class StreamCaptureHandler<T> : OutputCaptureHandler
        where T : Stream
    {
        /// <summary>
        /// A Stream instance that we can process image data to.
        /// </summary>
        public T CurrentStream { get; protected set; }
        
        /// <inheritdoc />
        public override void Process(ImageContext context)
        {
            Processed += context.Data.Length;
                        
            if (CurrentStream.CanWrite)
                CurrentStream.Write(context.Data, 0, context.Data.Length);
            else
                throw new IOException("Stream not writable.");

            base.Process(context);
        }

        /// <inheritdoc />
        public override void PostProcess()
        {
            try
            {
                if (CurrentStream != null && CurrentStream.CanRead && CurrentStream.Length > 0)
                {
                    if (OnManipulate != null && ImageContext != null)
                    {
                        byte[] arr = null;

                        using (var ms = new MemoryStream())
                        {
                            CurrentStream.Position = 0;
                            CurrentStream.CopyTo(ms);

                            arr = ms.ToArray();

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
                }
            }
            catch(Exception e)
            {
                MmalLog.Logger.LogWarning($"Something went wrong while processing stream: {e.Message}. {e.InnerException?.Message}. {e.StackTrace}");
            }
        }
        
        /// <inheritdoc />
        public override string TotalProcessed()
        {
            return $"{Helpers.ConvertBytesToMegabytes(Processed)}";
        }

        /// <summary>
        /// Releases the underlying stream.
        /// </summary>
        public override void Dispose()
        {
            MmalLog.Logger.LogInformation($"Successfully processed {Helpers.ConvertBytesToMegabytes(Processed)}.");
            CurrentStream?.Dispose();
        }
    }
}
