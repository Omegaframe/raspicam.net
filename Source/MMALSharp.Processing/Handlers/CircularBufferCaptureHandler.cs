using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MMALSharp.Common;
using MMALSharp.Common.Utility;
using MMALSharp.Processing;

namespace MMALSharp.Handlers
{
    public sealed class CircularBufferCaptureHandler : VideoStreamCaptureHandler
    {
        private bool _recordToFileStream;
        private int _bufferSize;
        private bool _receivedIFrame;

        public CircularBuffer<byte> Buffer { get; private set; }

        public CircularBufferCaptureHandler(int bufferSize) : base()
        {
            _bufferSize = bufferSize;
            Buffer = new CircularBuffer<byte>(bufferSize);
        }

        public CircularBufferCaptureHandler(int bufferSize, string directory, string extension) : base(directory, extension)
        {
            _bufferSize = bufferSize;
            Buffer = new CircularBuffer<byte>(bufferSize);
        }

        public CircularBufferCaptureHandler(int bufferSize, string fullPath) : base(fullPath)
        {
            _bufferSize = bufferSize;
            Buffer = new CircularBuffer<byte>(bufferSize);
        }

        public override void Process(ImageContext context)
        {
            if (!_recordToFileStream)
            {
                for (var i = 0; i < context.Data.Length; i++)
                {
                    Buffer.PushBack(context.Data[i]);
                }
            }
            else
            {
                if (context.Encoding == MmalEncoding.H264)
                {
                    _receivedIFrame = context.IsIFrame;
                }

                if (Buffer.Size > 0)
                {
                    // The buffer contains data.
                    if (CurrentStream != null && CurrentStream.CanWrite)
                    {
                        CurrentStream.Write(Buffer.ToArray(), 0, Buffer.Size);
                    }

                    Processed += Buffer.Size;
                    Buffer = new CircularBuffer<byte>(Buffer.Capacity);
                }

                if (CurrentStream != null && CurrentStream.CanWrite)
                {
                    CurrentStream.Write(context.Data, 0, context.Data.Length);
                }

                Processed += context.Data.Length;
            }

            // Not calling base method to stop data being written to the stream when not recording.
            ImageContext = context;
        }

        /// <summary>
        /// Call to start recording to FileStream.
        /// </summary>
        /// <param name="initRecording">Optional Action to execute when recording starts, for example, to request an h.264 I-frame.</param>
        /// <param name="cancellationToken">When the token is canceled, <see cref="StopRecording"/> is called. If a token is not provided, the caller must stop the recording.</param>
        /// <returns>Task representing the recording process if a CancellationToken was provided, otherwise a completed Task.</returns>
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
            _receivedIFrame = false;
        }

        public override void Dispose()
        {
            CurrentStream?.Dispose();
        }

        public override string TotalProcessed()
        {
            return $"{Processed}";
        }
    }
}
