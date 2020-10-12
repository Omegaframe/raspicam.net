using System.IO;
using MMALSharp.Common.Utility;

namespace MMALSharp.Processing.Handlers
{
    public class InputCaptureHandler : IInputCaptureHandler
    {
        public Stream CurrentStream { get; }
        public int Processed { get; protected set; }

        public InputCaptureHandler(Stream inputStream)
        {
            CurrentStream = inputStream;
        }

        public virtual ProcessResult Process(uint allocSize)
        {
            var buffer = new byte[allocSize];

            var read = CurrentStream.Read(buffer, 0, (int)allocSize);

            Processed += read;

            return read == 0 ?
                new ProcessResult { Success = true, BufferFeed = buffer, IsEof = true, DataLength = read } :
                new ProcessResult { Success = true, BufferFeed = buffer, DataLength = read };
        }

        public void Dispose() => CurrentStream?.Dispose();
        public string TotalProcessed() => $"{Helpers.ConvertBytesToMegabytes(Processed)}";
    }
}
