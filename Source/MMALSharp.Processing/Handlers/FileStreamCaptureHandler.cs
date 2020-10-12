using Microsoft.Extensions.Logging;
using MMALSharp.Common.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MMALSharp.Handlers
{
    /// <summary>
    /// Processes image data to a <see cref="FileStream"/>.
    /// </summary>
    public class FileStreamCaptureHandler : StreamCaptureHandler<FileStream>, IFileStreamCaptureHandler
    {
        private readonly bool _customFilename;
        private int _increment;

        /// <summary>
        /// A list of files that have been processed by this capture handler.
        /// </summary>
        public List<ProcessedFileResult> ProcessedFiles { get; set; } = new List<ProcessedFileResult>();

        /// <summary>
        /// The directory to save to (if applicable).
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// The extension of the file (if applicable).
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// The name of the current file associated with the FileStream.
        /// </summary>
        public string CurrentFilename { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="FileStreamCaptureHandler"/> class without provisions for writing to a file. Supports
        /// subclasses in which file output is optional.
        /// </summary>
        public FileStreamCaptureHandler()
        {
            MmalLog.Logger.LogDebug($"{nameof(FileStreamCaptureHandler)} empty ctor invoked, no file will be written");
        }

        /// <summary>
        /// Creates a new instance of the <see cref="FileStreamCaptureHandler"/> class with the specified directory and filename extension. Filenames will be in the
        /// format "dd-MMM-yy HH-mm-ss" taken from this moment in time.
        /// </summary>
        /// <param name="directory">The directory to save captured data.</param>
        /// <param name="extension">The filename extension for saving files.</param>
        public FileStreamCaptureHandler(string directory, string extension)
        {
            Directory = directory.TrimEnd('/');
            Extension = extension.TrimStart('.');

            MmalLog.Logger.LogDebug($"{nameof(FileStreamCaptureHandler)} created for directory {Directory} and extension {Extension}");

            System.IO.Directory.CreateDirectory(Directory);

            var now = DateTime.Now.ToString("dd-MMM-yy HH-mm-ss");

            int i = 1;

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

        /// <summary>
        /// Creates a new instance of the <see cref="FileStreamCaptureHandler"/> class with the specified file path.
        /// </summary>
        /// <param name="fullPath">The absolute full path to save captured data to.</param>
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

        /// <summary>
        /// Gets the filename that a FileStream points to.
        /// </summary>
        /// <returns>The filename.</returns>
        public string GetFilename() => (CurrentStream != null) ? Path.GetFileNameWithoutExtension(CurrentStream.Name) : string.Empty;

        /// <summary>
        /// Gets the filepath that a FileStream points to.
        /// </summary>
        /// <returns>The filepath.</returns>
        public string GetFilepath() =>            CurrentStream?.Name ?? string.Empty;

        /// <summary>
        /// Creates a new File (FileStream), assigns it to the Stream instance of this class and disposes of any existing stream. 
        /// </summary>
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
                string tempFilename = DateTime.Now.ToString("dd-MMM-yy HH-mm-ss");
                int i = 1;

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
