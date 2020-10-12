using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MMALSharp.Common;
using MMALSharp.Common.Utility;
using MMALSharp.Processing.Internal;

[assembly: InternalsVisibleTo("MMALSharp.Tests")]
namespace MMALSharp.Processing.Handlers
{
    public sealed class CircularBufferCaptureHandler : VideoStreamCaptureHandler
    {
        bool _recordToFileStream;

        internal CircularBuffer<byte> Buffer { get; private set; }

        public CircularBufferCaptureHandler(int bufferSize)
        {
            Buffer = new CircularBuffer<byte>(bufferSize);
        }

        public CircularBufferCaptureHandler(int bufferSize, string directory, string extension) : base(directory, extension)
        {
            Buffer = new CircularBuffer<byte>(bufferSize);
        }

        public CircularBufferCaptureHandler(int bufferSize, string fullPath) : base(fullPath)
        {
            Buffer = new CircularBuffer<byte>(bufferSize);
        }

        public override void Process(ImageContext context)
        {
            if (!_recordToFileStream)
            {
                foreach (var bytes in context.Data)
                    Buffer.PushBack(bytes);
            }
            else
            {
                if (Buffer.Size > 0)
                {
                    // The buffer contains data.
                    if (CurrentStream != null && CurrentStream.CanWrite)
                        CurrentStream.Write(Buffer.ToArray(), 0, Buffer.Size);

                    Processed += Buffer.Size;
                    Buffer = new CircularBuffer<byte>(Buffer.Capacity);
                }

                if (CurrentStream != null && CurrentStream.CanWrite)
                    CurrentStream.Write(context.Data, 0, context.Data.Length);

                Processed += context.Data.Length;
            }

            ImageContext = context;
        }

        public async Task StartRecording(Action initRecording = null, CancellationToken cancellationToken = default)
        {
            if (CurrentStream == null)
                throw new InvalidOperationException($"Recording unavailable, {nameof(CircularBufferCaptureHandler)} was not created with output-file arguments");

            _recordToFileStream = true;

            initRecording?.Invoke();

            if (cancellationToken == CancellationToken.None)
                return;

            try { await Task.Delay(-1, cancellationToken).ConfigureAwait(false); } catch (TaskCanceledException) {/* catch */ }

            StopRecording();
        }

        public void StopRecording()
        {
            if (CurrentStream == null)
                throw new InvalidOperationException($"Recording unavailable, {nameof(CircularBufferCaptureHandler)} was not created with output-file arguments");

            MmalLog.Logger.LogInformation("Stop recording.");

            _recordToFileStream = false;
        }

        public override void Dispose() => CurrentStream?.Dispose();
        public override string TotalProcessed() => $"{Processed}";
    }
}
