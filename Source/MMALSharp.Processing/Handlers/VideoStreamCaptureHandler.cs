using System;
using System.IO;
using MMALSharp.Common;
using MMALSharp.Processing.Processors.Motion;

namespace MMALSharp.Processing.Handlers
{
    public class VideoStreamCaptureHandler : FileStreamCaptureHandler, IMotionVectorCaptureHandler, IVideoCaptureHandler
    {
        public MotionType MotionType { get; set; }
        protected Stream MotionVectorStore { get; set; }

        protected bool StoreVideoTimestamps { get; }

        public VideoStreamCaptureHandler()
        {
            StoreVideoTimestamps = false;
        }

        public VideoStreamCaptureHandler(string directory, string extension, bool storeTimestamps = false) : base(directory, extension)
        {
            StoreVideoTimestamps = storeTimestamps;
        }

        public VideoStreamCaptureHandler(string fullPath, bool storeTimestamps = false) : base(fullPath)
        {
            StoreVideoTimestamps = storeTimestamps;
        }

        public override void Process(ImageContext context)
        {
            if (CurrentStream == null)
                return;

            base.Process(context);

            if (!StoreVideoTimestamps || !context.Pts.HasValue)
                return;

            var str = $"{context.Pts / 1000}.{context.Pts % 1000:000}" + Environment.NewLine;
            File.AppendAllText($"{Directory}/{CurrentFilename}.pts", str);
        }

        public virtual void Split() => NewFile();

        public void InitialiseMotionStore(Stream stream)
        {
            if (CurrentStream == null)
                return;

            MotionVectorStore = stream;
        }

        public void ProcessMotionVectors(byte[] data)
        {
            if (MotionVectorStore == null)
                return;

            if (MotionVectorStore.CanWrite)
                MotionVectorStore.Write(data, 0, data.Length);
            else
                throw new IOException("Stream not writable.");
        }
    }
}
