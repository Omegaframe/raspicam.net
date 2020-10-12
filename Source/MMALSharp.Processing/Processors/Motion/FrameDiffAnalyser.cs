using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MMALSharp.Common;
using MMALSharp.Common.Utility;

namespace MMALSharp.Processing.Processors.Motion
{
    public class FrameDiffAnalyser : FrameAnalyser
    {
        public int CellDivisor { get; set; } = 32;

        internal Action OnDetect { get; set; }

        protected byte[] TestFrame { get; set; }
        protected bool FullTestFrame { get; set; }
        protected MotionConfig MotionConfig { get; set; }
        protected ImageContext ImageContext { get; set; }

        bool _firstFrame = true;

        int _frameWidth;
        int _frameHeight;
        int _frameStride;
        int _frameBpp;

        byte[] _mask;
        readonly Stopwatch _testFrameAge;

        int[] _cellDiff;
        Rectangle[] _cellRect;
        byte[] _workingData;

        public FrameDiffAnalyser(MotionConfig config, Action onDetect)
        {
            MotionConfig = config;
            OnDetect = onDetect;

            _testFrameAge = new Stopwatch();
        }

        public override void Apply(ImageContext context)
        {
            ImageContext = context;

            base.Apply(context);

            if (!FullTestFrame)
            {
                if (!context.IsEos)
                    return;

                FullTestFrame = true;
                PrepareTestFrame();
                MmalLog.Logger.LogDebug("EOS reached for test frame.");
            }
            else
            {
                MmalLog.Logger.LogDebug("Have full test frame.");

                if (!FullFrame || TestFrameExpired())
                    return;

                MmalLog.Logger.LogDebug("Have full frame, checking for changes.");
                CheckForChanges(OnDetect);
            }
        }

        public void ResetAnalyser()
        {
            TestFrame = null;
            WorkingData = new List<byte>();
            FullFrame = false;
            FullTestFrame = false;

            _testFrameAge.Reset();
        }

        void PrepareTestFrame()
        {
            if (_firstFrame)
            {
                // one-time collection of basic frame dimensions
                _frameWidth = ImageContext.Resolution.Width;
                _frameHeight = ImageContext.Resolution.Height;
                _frameBpp = GetBpp() / 8;
                _frameStride = ImageContext.Stride;

                // one-time setup of the diff cell parameters and arrays
                var indices = (int)Math.Pow(CellDivisor, 2);
                var cellWidth = _frameWidth / CellDivisor;
                var cellHeight = _frameHeight / CellDivisor;
                var i = 0;

                _cellRect = new Rectangle[indices];
                _cellDiff = new int[indices];

                for (var row = 0; row < CellDivisor; row++)
                {
                    var y = row * cellHeight;

                    for (var col = 0; col < CellDivisor; col++)
                    {
                        var x = col * cellWidth;
                        _cellRect[i] = new Rectangle(x, y, cellWidth, cellHeight);
                        i++;
                    }
                }

                TestFrame = WorkingData.ToArray();

                if (!string.IsNullOrWhiteSpace(MotionConfig.MotionMaskPathname))
                    PrepareMask();

                _firstFrame = false;
            }
            else
            {
                TestFrame = WorkingData.ToArray();
            }

            if (MotionConfig.TestFrameInterval != TimeSpan.Zero)
                _testFrameAge.Restart();
        }

        int GetBpp()
        {
            const PixelFormat format = default;

            // RGB16 doesn't appear to be supported by GDI?
            if (ImageContext.PixelFormat == MmalEncoding.Rgb24)
                return 24;

            if (ImageContext.PixelFormat == MmalEncoding.Rgb32 || ImageContext.PixelFormat == MmalEncoding.Rgba)
                return 32;

            if (format == default)
                throw new Exception("Unsupported pixel format.");
        }

        void PrepareMask()
        {
            using var fs = new FileStream(MotionConfig.MotionMaskPathname, FileMode.Open, FileAccess.Read);
            using var mask = new Bitmap(fs);

            // Verify it matches our frame dimensions
            var maskBpp = Image.GetPixelFormatSize(mask.PixelFormat) / 8;
            if (mask.Width != _frameWidth || mask.Height != _frameHeight || maskBpp != _frameBpp)
                throw new Exception("Motion-detection mask must match raw stream width, height, and format (bits per pixel)");

            // Store the byte array
            BitmapData bmpData = null;
            try
            {
                bmpData = mask.LockBits(new Rectangle(0, 0, mask.Width, mask.Height), ImageLockMode.ReadOnly, mask.PixelFormat);
                var pNative = bmpData.Scan0;
                var size = bmpData.Stride * mask.Height;
                _mask = new byte[size];
                Marshal.Copy(pNative, _mask, 0, size);
            }
            finally
            {
                mask.UnlockBits(bmpData);
            }
        }

        bool TestFrameExpired()
        {
            if (MotionConfig.TestFrameInterval == TimeSpan.Zero || _testFrameAge.Elapsed < MotionConfig.TestFrameInterval)
                return false;

            MmalLog.Logger.LogDebug("Have full frame, updating test frame.");
            PrepareTestFrame();

            return true;
        }

        void CheckForChanges(Action onDetect)
        {
            var diff = Analyse();

            if (diff >= MotionConfig.Threshold)
            {
                MmalLog.Logger.LogInformation($"Motion detected! Frame difference {diff}.");
                onDetect();
            }
        }

        int Analyse()
        {
            _workingData = WorkingData.ToArray();

            var result = Parallel.ForEach(_cellDiff, (cell, loopState, loopIndex) => CheckDiff(loopIndex, loopState));

            if (!result.IsCompleted && !result.LowestBreakIteration.HasValue)
                return int.MaxValue; // loop was stopped, so return a large diff

            return _cellDiff.Sum();
        }

        void CheckDiff(long cellIndex, ParallelLoopState loopState)
        {
            var diff = 0;
            var rect = _cellRect[cellIndex];

            for (var col = rect.X; col < rect.X + rect.Width; col++)
            {
                for (var row = rect.Y; row < rect.Y + rect.Height; row++)
                {
                    var index = (col * _frameBpp) + (row * _frameStride);

                    if (_mask != null)
                    {
                        var rgbMask = _mask[index] + _mask[index + 1] + _mask[index + 2];

                        if (rgbMask == 0)
                            continue;
                    }

                    var rgb1 = TestFrame[index] + TestFrame[index + 1] + TestFrame[index + 2];
                    var rgb2 = _workingData[index] + _workingData[index + 1] + _workingData[index + 2];

                    if (rgb2 - rgb1 > MotionConfig.Threshold)
                        diff++;

                    // If the threshold has been exceeded, exit immediately and preempt any CheckDiff calls not yet started.
                    if (diff <= MotionConfig.Threshold) 
                        continue;

                    _cellDiff[cellIndex] = diff;
                    loopState.Stop();

                    return;
                }

                if (diff <= MotionConfig.Threshold) 
                    continue;

                _cellDiff[cellIndex] = diff;
                loopState.Stop();

                return;
            }

            _cellDiff[cellIndex] = diff;
        }
    }
}
