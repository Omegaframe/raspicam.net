using System;
using System.IO;
using MMALSharp.Common;
using MMALSharp.Processing.Processors.Motion;

namespace MMALSharp.Processing.Handlers
{
    public class FrameBufferCaptureHandler : MemoryStreamCaptureHandler, IMotionCaptureHandler, IVideoCaptureHandler
    {
        FrameDiffAnalyser _motionAnalyser;
        bool _detectingMotion;
        bool _waitForFullFrame = true;
        bool _writeFrameRequested;

        public FrameBufferCaptureHandler() { }

        public FrameBufferCaptureHandler(string directory = "", string extension = "", string fileDateTimeFormat = "yyyy-MM-dd HH.mm.ss.ffff")
        {
            FileDirectory = directory.TrimEnd('/');
            FileExtension = extension;
            FileDateTimeFormat = fileDateTimeFormat;
            Directory.CreateDirectory(FileDirectory);
        }

        public string FileDirectory { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
        public string FileDateTimeFormat { get; set; } = string.Empty;
        public string MostRecentFilename { get; set; } = string.Empty;
        public string MostRecentPathname { get; set; } = string.Empty;
        public MotionType MotionType { get; set; } = MotionType.FrameDiff;

        public void WriteFrame()
        {
            if (string.IsNullOrWhiteSpace(FileDirectory) || string.IsNullOrWhiteSpace(FileDateTimeFormat))
                throw new Exception($"The {nameof(FileDirectory)} and {nameof(FileDateTimeFormat)} must be set before calling {nameof(WriteFrame)}");

            _writeFrameRequested = true;
        }

        public override void Process(ImageContext context)
        {
            // guard against partial frame data at startup
            if (_waitForFullFrame)
            {
                _waitForFullFrame = !context.IsEos;
                if (_waitForFullFrame)
                    return;
            }

            if (_detectingMotion)
                _motionAnalyser.Apply(context);

            // accumulate frame data in the underlying memory stream
            base.Process(context);

            if (!context.IsEos) 
                return;

            // write a full frame if a request is pending
            if (_writeFrameRequested)
            {
                WriteStreamToFile();
                _writeFrameRequested = false;
            }

            // reset the stream to begin the next frame
            CurrentStream.SetLength(0);
        }

        public void ConfigureMotionDetection(MotionConfig config, Action onDetect)
        {
            _motionAnalyser = new FrameDiffAnalyser(config, onDetect);
            EnableMotionDetection();
        }

        public void EnableMotionDetection()
        {
            _detectingMotion = true;
            _motionAnalyser?.ResetAnalyser();
        }

        public void DisableMotionDetection()
        {
            _detectingMotion = false;
        }

        public void Split() { } // Unused, but required to handle a video stream.

        void WriteStreamToFile()
        {
            var directory = FileDirectory.TrimEnd('/');
            var filename = DateTime.Now.ToString(FileDateTimeFormat);
            var pathname = $"{directory}/{filename}.{FileExtension}";

            using (var fs = new FileStream(pathname, FileMode.Create, FileAccess.Write))
                CurrentStream.WriteTo(fs);

            MostRecentFilename = filename;
            MostRecentPathname = pathname;
        }
    }
}
