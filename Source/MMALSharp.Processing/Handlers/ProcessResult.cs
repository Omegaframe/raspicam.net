namespace MMALSharp.Processing.Handlers
{
    public class ProcessResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public bool IsEof { get; set; }
        public byte[] BufferFeed { get; set; }
        public int DataLength { get; set; }
        public int AllocSize { get; set; }
    }
}
