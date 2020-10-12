namespace MMALSharp.Processing.Handlers
{
    public class ProcessedFileResult
    {
        public string Directory { get; set; }
        public string Filename { get; set; }
        public string Extension { get; set; }

        public ProcessedFileResult(string directory, string filename, string extension)
        {
            Directory = directory;
            Filename = filename;
            Extension = extension;
        }
    }
}
