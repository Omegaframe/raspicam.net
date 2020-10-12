using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using MMALSharp.Common.Utility;

namespace MMALSharp.Processing.Handlers
{
    public class FileStreamCaptureHandler : StreamCaptureHandler<FileStream>, IFileStreamCaptureHandler
    {
        public List<ProcessedFileResult> ProcessedFiles { get; set; } = new List<ProcessedFileResult>();
        public string Directory { get; set; }
        public string Extension { get; set; }
        public string CurrentFilename { get; set; }

        readonly bool _customFilename;
        int _increment;

        public FileStreamCaptureHandler()
        {
            MmalLog.Logger.LogDebug($"{nameof(FileStreamCaptureHandler)} empty ctor invoked, no file will be written");
        }

        public FileStreamCaptureHandler(string directory, string extension)
        {
            Directory = directory.TrimEnd('/');
            Extension = extension.TrimStart('.');

            MmalLog.Logger.LogDebug($"{nameof(FileStreamCaptureHandler)} created for directory {Directory} and extension {Extension}");

            System.IO.Directory.CreateDirectory(Directory);

            var now = DateTime.Now.ToString("dd-MMM-yy HH-mm-ss");

            var i = 1;
            var fileName = $"{Directory}/{now}.{Extension}";

            while (File.Exists(fileName))
            {
                fileName = $"{Directory}/{now} {i}.{Extension}";
                i++;
            }

            var fileInfo = new FileInfo(fileName);

            CurrentFilename = Path.GetFileNameWithoutExtension(fileInfo.Name);
            CurrentStream = File.Create(fileName);
        }

        public FileStreamCaptureHandler(string fullPath)
        {
            var fileInfo = new FileInfo(fullPath);

            Directory = fileInfo.DirectoryName;
            CurrentFilename = Path.GetFileNameWithoutExtension(fileInfo.Name);

            var ext = fullPath.Split('.').LastOrDefault();

            if (string.IsNullOrEmpty(ext))
                throw new ArgumentNullException(nameof(ext), "Could not get file extension from path string.");

            Extension = ext;

            MmalLog.Logger.LogDebug($"{nameof(FileStreamCaptureHandler)} created for directory {Directory} and extension {Extension}");

            _customFilename = true;

            System.IO.Directory.CreateDirectory(Directory);

            CurrentStream = File.Create(fullPath);
        }

        public string GetFilename() => (CurrentStream != null) ? Path.GetFileNameWithoutExtension(CurrentStream.Name) : string.Empty;
        public string GetFilepath() => CurrentStream?.Name ?? string.Empty;

        public virtual void NewFile()
        {
            if (CurrentStream == null)
                return;

            CurrentStream?.Dispose();

            string newFilename;
            if (_customFilename)
            {
                // If we're taking photos from video port, we don't want to be hammering File.Exists as this is added I/O overhead. Camera can take multiple photos per second
                // so we can't do this when filename uses the current DateTime.
                _increment++;
                newFilename = $"{Directory}/{CurrentFilename} {_increment}.{Extension}";
            }
            else
            {
                var tempFilename = DateTime.Now.ToString("dd-MMM-yy HH-mm-ss");
                var i = 1;

                newFilename = $"{Directory}/{tempFilename}.{Extension}";

                while (File.Exists(newFilename))
                {
                    newFilename = $"{Directory}/{tempFilename} {i}.{Extension}";
                    i++;
                }
            }

            CurrentStream = File.Create(newFilename);
        }

        public override void PostProcess()
        {
            if (CurrentStream == null)
                return;

            ProcessedFiles.Add(new ProcessedFileResult(Directory, GetFilename(), Extension));
            base.PostProcess();
        }

        public override string TotalProcessed() => $"{Processed}";
    }
}
