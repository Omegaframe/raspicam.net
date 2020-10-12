﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MMALSharp.Common;
using MMALSharp.Components;
using MMALSharp.Components.EncoderComponents;
using MMALSharp.Ports;
using MMALSharp.Ports.Outputs;
using MMALSharp.Processing.Handlers;
using Xunit;

namespace MMALSharp.Tests
{
    [Collection("MMALCollection")]
    public class StandaloneTests
    {
        /*
         *      Please note, the camera component is only featured in these tests in order to get the image/video data we need to 
         *      test the standalone aspects of this library. If you already have the image/video files stored to disk, there is 
         *      no requirement for the camera to be connected.
         */

        private static MMALFixture _fixture;
        public static MMALFixture Fixture
        {
            get
            {
                if (_fixture == null)
                {
                    _fixture = new MMALFixture();
                }

                return _fixture;
            }
            set => _fixture = value;
        }

        public StandaloneTests(MMALFixture fixture)
        {
            Fixture = fixture;
        }

        [Fact]
        public async Task EncodeDecodePictureFromFile()
        {
            TestHelper.BeginTest("Image - EncodeDecodeFromFile");
            TestHelper.SetConfigurationDefaults();
            TestHelper.CleanDirectory("/home/pi/images/tests");

            string imageFilepath = string.Empty;
            string decodedFilepath = string.Empty;

            // First take a new JPEG picture using RGB24 encoding.
            using (var imgCaptureHandler = new ImageStreamCaptureHandler("/home/pi/images/tests", "jpg"))
            using (var preview = new MmalNullSinkComponent())
            using (var imgEncoder = new MmalImageEncoder())
            {
                Fixture.MalCamera.ConfigureCameraSettings();

                var portConfig = new MmalPortConfig(MmalEncoding.Jpeg, MmalEncoding.Rgb24, quality: 90);

                imgEncoder.ConfigureOutputPort(portConfig, imgCaptureHandler);

                // Create our component pipeline.         
                Fixture.MalCamera.Camera.StillPort
                    .ConnectTo(imgEncoder);
                Fixture.MalCamera.Camera.PreviewPort
                    .ConnectTo(preview);

                // Camera warm up time
                await Task.Delay(2000);
                await Fixture.MalCamera.ProcessAsync(Fixture.MalCamera.Camera.StillPort);

                Fixture.CheckAndAssertFilepath(imgCaptureHandler.GetFilepath());

                imageFilepath = imgCaptureHandler.GetFilepath();
            }

            // Next decode the JPEG to raw YUV420.
            using (var stream = File.OpenRead(imageFilepath))
            using (var inputCaptureHandler = new InputCaptureHandler(stream))
            using (var outputCaptureHandler = new ImageStreamCaptureHandler("/home/pi/images/", "raw"))
            using (var imgDecoder = new MmalImageDecoder())
            {
                // We do not pass the resolution to the input port. Doing so will cause a MMAL exception.
                var inputConfig = new MmalPortConfig(MmalEncoding.Jpeg, MmalEncoding.Rgb24, zeroCopy: true);
                var outputConfig = new MmalPortConfig(MmalEncoding.I420, null, width: 640, height: 480, zeroCopy: true);

                // Create our component pipeline.
                imgDecoder.ConfigureInputPort(inputConfig, inputCaptureHandler)
                    .ConfigureOutputPort<FileEncodeOutputPort>(0, outputConfig, outputCaptureHandler);

                await Fixture.MMALStandalone.ProcessAsync(imgDecoder);
                
                Fixture.CheckAndAssertFilepath(outputCaptureHandler.GetFilepath());

                decodedFilepath = outputCaptureHandler.GetFilepath();
            }

            // Finally re-encode to BMP using YUV420.
            using (var stream = File.OpenRead(decodedFilepath))
            using (var inputCaptureHandler = new InputCaptureHandler(stream))
            using (var outputCaptureHandler = new ImageStreamCaptureHandler("/home/pi/images/", "bmp"))
            using (var imgEncoder = new MmalImageEncoder())
            {
                var inputConfig = new MmalPortConfig(MmalEncoding.I420, null, width: 640, height: 480, zeroCopy: true);
                var outputConfig = new MmalPortConfig(MmalEncoding.Bmp, MmalEncoding.I420, width: 640, height: 480, zeroCopy: true);

                imgEncoder.ConfigureInputPort(inputConfig, inputCaptureHandler)
                    .ConfigureOutputPort<FileEncodeOutputPort>(0, outputConfig, outputCaptureHandler);

                await Fixture.MMALStandalone.ProcessAsync(imgEncoder);
                
                Fixture.CheckAndAssertFilepath(outputCaptureHandler.GetFilepath());
            }
        }

        [Fact]
        public async Task EncodeDecodeVideoFromFile()
        {
            // In this test, we first start by taking a H.264 video using YUV420 encoding.
            // The outputted file is then fed into a decoder component where we decode to raw YUV420.
            // The next outputted file is then fed into an encoder component where we re-encode to MJPEG using YUV420 encoding.
            // This test showcases each operation as a single unit of work.
            TestHelper.BeginTest("Video - EncodeDecodeFromFile");
            TestHelper.SetConfigurationDefaults();
            TestHelper.CleanDirectory("/home/pi/videos/tests");

            string videoFilepath = string.Empty;
            string decodedFilepath = string.Empty;

            // First take a new H.264 video using YUV420 encoding.
            using (var videoCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/tests", "h264"))
            using (var preview = new MmalNullSinkComponent())
            using (var vidEncoder = new MmalVideoEncoder())
            {
                Fixture.MalCamera.ConfigureCameraSettings();

                var portConfig = new MmalPortConfig(MmalEncoding.H264, MmalEncoding.I420, quality: 0, bitrate: MmalVideoEncoder.MaxBitrateLevel4);

                vidEncoder.ConfigureOutputPort(portConfig, videoCaptureHandler);

                // Create our component pipeline.         
                Fixture.MalCamera.Camera.VideoPort
                    .ConnectTo(vidEncoder);
                Fixture.MalCamera.Camera.PreviewPort
                    .ConnectTo(preview);

                // Camera warm up time
                await Task.Delay(2000);

                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));

                await Fixture.MalCamera.ProcessAsync(Fixture.MalCamera.Camera.VideoPort, cts.Token);

                Fixture.CheckAndAssertFilepath(videoCaptureHandler.GetFilepath());

                videoFilepath = videoCaptureHandler.GetFilepath();
            }

            // Next decode the H.264 video to raw YUV420.
            using (var stream = File.OpenRead(videoFilepath))
            using (var inputCaptureHandler = new InputCaptureHandler(stream))
            using (var outputCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/", "raw"))
            using (var vidDecoder = new MmalVideoDecoder())
            {
                // Set the input/output resolutions to match what is set in the tests defaults method.
                var inputConfig = new MmalPortConfig(MmalEncoding.H264, null, width: 640, height: 480,  framerate: 25, zeroCopy: true);
                var outputConfig = new MmalPortConfig(MmalEncoding.I420, null, width: 640, height: 480, zeroCopy: true);

                // Create our component pipeline.
                vidDecoder.ConfigureInputPort(inputConfig, inputCaptureHandler)
                          .ConfigureOutputPort<FileEncodeOutputPort>(0, outputConfig, outputCaptureHandler);

                await Fixture.MMALStandalone.ProcessAsync(vidDecoder);

                Fixture.CheckAndAssertFilepath(outputCaptureHandler.GetFilepath());

                decodedFilepath = outputCaptureHandler.GetFilepath();
            }

            // Finally re-encode to MJPEG using YUV420.
            using (var stream = File.OpenRead(decodedFilepath))
            using (var inputCaptureHandler = new InputCaptureHandler(stream))
            using (var outputCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/", "mjpeg"))
            using (var vidEncoder = new MmalVideoEncoder())
            {
                var inputConfig = new MmalPortConfig(MmalEncoding.I420, null, width: 640, height: 480, framerate: 25, zeroCopy: true);
                var outputConfig = new MmalPortConfig(MmalEncoding.MJpeg, MmalEncoding.I420, width: 640, height: 480, framerate: 25, zeroCopy: true);

                vidEncoder.ConfigureInputPort(inputConfig, inputCaptureHandler)
                    .ConfigureOutputPort<FileEncodeOutputPort>(0, outputConfig, outputCaptureHandler);

                await Fixture.MMALStandalone.ProcessAsync(vidEncoder);

                Fixture.CheckAndAssertFilepath(outputCaptureHandler.GetFilepath());
            }
        }

        [Fact]
        public async Task EncodeDecodeVideoFromFileWithSplitter()
        {
            // This test is very similar to EncodeDecodeVideoFromFile, however this
            // one features a splitter component when re-encoding the video.
            TestHelper.BeginTest("Video - EncodeDecodeFromFileWithSplitter");
            TestHelper.SetConfigurationDefaults();
            TestHelper.CleanDirectory("/home/pi/videos/tests");

            string videoFilepath = string.Empty;
            string decodedFilepath = string.Empty;

            // First take a new H.264 video using YUV420 encoding.
            using (var videoCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/tests", "h264"))
            using (var preview = new MmalNullSinkComponent())
            using (var vidEncoder = new MmalVideoEncoder())
            {
                Fixture.MalCamera.ConfigureCameraSettings();

                var portConfig = new MmalPortConfig(MmalEncoding.H264, MmalEncoding.I420, bitrate: MmalVideoEncoder.MaxBitrateLevel4);

                vidEncoder.ConfigureOutputPort(portConfig, videoCaptureHandler);

                // Create our component pipeline.         
                Fixture.MalCamera.Camera.VideoPort
                    .ConnectTo(vidEncoder);
                Fixture.MalCamera.Camera.PreviewPort
                    .ConnectTo(preview);

                // Camera warm up time
                await Task.Delay(2000);

                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));

                await Fixture.MalCamera.ProcessAsync(Fixture.MalCamera.Camera.VideoPort, cts.Token);

                Fixture.CheckAndAssertFilepath(videoCaptureHandler.GetFilepath());

                videoFilepath = videoCaptureHandler.GetFilepath();
            }

            // Next decode the H.264 video to raw YUV420.
            using (var stream = File.OpenRead(videoFilepath))
            using (var inputCaptureHandler = new InputCaptureHandler(stream))
            using (var outputCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/", "raw"))
            using (var vidDecoder = new MmalVideoDecoder())
            {
                // Set the input/output resolutions to match what is set in the tests defaults method.
                var inputConfig = new MmalPortConfig(MmalEncoding.H264, null, width: 640, height: 480, framerate: 25, zeroCopy: true);
                var outputConfig = new MmalPortConfig(MmalEncoding.I420, null, width: 640, height: 480, zeroCopy: true);

                // Create our component pipeline.
                vidDecoder.ConfigureInputPort(inputConfig, inputCaptureHandler)
                          .ConfigureOutputPort<FileEncodeOutputPort>(0, outputConfig, outputCaptureHandler);

                await Fixture.MMALStandalone.ProcessAsync(vidDecoder);

                Fixture.CheckAndAssertFilepath(outputCaptureHandler.GetFilepath());

                decodedFilepath = outputCaptureHandler.GetFilepath();
            }

            // Finally re-encode to MJPEG using YUV420. We are using the splitter component and
            // creating 4 output streams from the single input stream.
            using (var stream = File.OpenRead(decodedFilepath))
            using (var inputCaptureHandler = new InputCaptureHandler(stream))
            using (var outputCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/", "mjpeg"))
            using (var outputCaptureHandler2 = new VideoStreamCaptureHandler("/home/pi/videos/", "mjpeg"))
            using (var outputCaptureHandler3 = new VideoStreamCaptureHandler("/home/pi/videos/", "mjpeg"))
            using (var outputCaptureHandler4 = new VideoStreamCaptureHandler("/home/pi/videos/", "mjpeg"))
            using (var splitter = new MmalSplitterComponent())
            using (var vidEncoder = new MmalVideoEncoder())
            using (var vidEncoder2 = new MmalVideoEncoder())
            using (var vidEncoder3 = new MmalVideoEncoder())
            using (var vidEncoder4 = new MmalVideoEncoder())
            {
                // You will notice here that we are setting the resolution against the splitter's input port. This is necessary to
                // tell this component the resolution of the data that it's going to be receiving.
                var splitterInputConfig = new MmalPortConfig(MmalEncoding.I420, null, width: 640, height: 480, framerate: 25, zeroCopy: true);
                var splitterOutputConfig = new MmalPortConfig(null, null, width: 640, height: 480, zeroCopy: true);

                var inputConfig = new MmalPortConfig(MmalEncoding.I420, null, width: 640, height: 480, framerate: 25, zeroCopy: true);
                var outputConfig = new MmalPortConfig(MmalEncoding.MJpeg, MmalEncoding.I420, width: 640, height: 480, framerate: 25, zeroCopy: true);

                splitter.ConfigureInputPort(splitterInputConfig, inputCaptureHandler)
                        .ConfigureOutputPort(0, splitterOutputConfig, null);

                vidEncoder.ConfigureInputPort(inputConfig, splitter.Outputs[0], null)
                    .ConfigureOutputPort<FileEncodeOutputPort>(0, outputConfig, outputCaptureHandler);

                vidEncoder2.ConfigureInputPort(inputConfig, splitter.Outputs[1], null)
                    .ConfigureOutputPort<FileEncodeOutputPort>(0, outputConfig, outputCaptureHandler2);

                vidEncoder3.ConfigureInputPort(inputConfig, splitter.Outputs[2], null)
                    .ConfigureOutputPort<FileEncodeOutputPort>(0, outputConfig, outputCaptureHandler3);

                vidEncoder4.ConfigureInputPort(inputConfig, splitter.Outputs[3], null)
                    .ConfigureOutputPort<FileEncodeOutputPort>(0, outputConfig, outputCaptureHandler4);

                splitter.Outputs[0].ConnectTo(vidEncoder);
                splitter.Outputs[1].ConnectTo(vidEncoder2);
                splitter.Outputs[2].ConnectTo(vidEncoder3);
                splitter.Outputs[3].ConnectTo(vidEncoder4);

                // The splitter is the first component in this pipeline.
                await Fixture.MMALStandalone.ProcessAsync(splitter);

                Fixture.CheckAndAssertFilepath(outputCaptureHandler.GetFilepath());
                Fixture.CheckAndAssertFilepath(outputCaptureHandler2.GetFilepath());
                Fixture.CheckAndAssertFilepath(outputCaptureHandler3.GetFilepath());
                Fixture.CheckAndAssertFilepath(outputCaptureHandler4.GetFilepath());
            }
        }

        [Fact]
        public async Task FullPipelineWithSplitter()
        {
            // In this test we are first recording a H.264 video using YUV420 encoding.
            // The output file is then fed to a decoder -> splitter -> 4 encoder components. 
            TestHelper.BeginTest("Video - FullPipelineWithSplitter");
            TestHelper.SetConfigurationDefaults();
            TestHelper.CleanDirectory("/home/pi/videos/tests");

            string videoFilepath = string.Empty;
            
            // First take a new H.264 video using YUV420 encoding.
            using (var videoCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/tests", "h264"))
            using (var preview = new MmalNullSinkComponent())
            using (var vidEncoder = new MmalVideoEncoder())
            {
                Fixture.MalCamera.ConfigureCameraSettings();

                var portConfig = new MmalPortConfig(MmalEncoding.H264, MmalEncoding.I420, 0, MmalVideoEncoder.MaxBitrateLevel4, null);

                vidEncoder.ConfigureOutputPort(portConfig, videoCaptureHandler);

                // Create our component pipeline.         
                Fixture.MalCamera.Camera.VideoPort
                    .ConnectTo(vidEncoder);
                Fixture.MalCamera.Camera.PreviewPort
                    .ConnectTo(preview);

                // Camera warm up time
                await Task.Delay(2000);

                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));

                await Fixture.MalCamera.ProcessAsync(Fixture.MalCamera.Camera.VideoPort, cts.Token);

                Fixture.CheckAndAssertFilepath(videoCaptureHandler.GetFilepath());

                videoFilepath = videoCaptureHandler.GetFilepath();
            }

            await using var stream = File.OpenRead(videoFilepath);
            using var inputCaptureHandler = new InputCaptureHandler(stream);
            using var outputCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/tests", "h264");
            using var outputCaptureHandler2 = new VideoStreamCaptureHandler("/home/pi/videos/tests", "h264");
            using var outputCaptureHandler3 = new VideoStreamCaptureHandler("/home/pi/videos/tests", "h264");
            using var outputCaptureHandler4 = new VideoStreamCaptureHandler("/home/pi/videos/tests", "h264");
            using var splitter = new MmalSplitterComponent();
            using var imgDecoder = new MmalVideoDecoder();
            using var imgEncoder = new MmalVideoEncoder();
            using var imgEncoder2 = new MmalVideoEncoder();
            using var imgEncoder3 = new MmalVideoEncoder();
            using var imgEncoder4 = new MmalVideoEncoder();
            var splitterInputConfig = new MmalPortConfig(MmalEncoding.I420, null, framerate: 25, zeroCopy: true);
            var splitterOutputConfig = new MmalPortConfig(null, null, zeroCopy: true);

            var decoderInputConfig = new MmalPortConfig(MmalEncoding.H264, null, width: 640, height: 480, framerate: 25, zeroCopy: true);
            var decoderOutputConfig = new MmalPortConfig(MmalEncoding.I420, null, width: 640, height: 480, zeroCopy: true);

            var encoderInputConfig = new MmalPortConfig(MmalEncoding.I420, null, width: 640, height: 480, zeroCopy: true);
            var encoderOutputConfig = new MmalPortConfig(MmalEncoding.H264, MmalEncoding.I420, width: 640, height: 480, framerate: 25, zeroCopy: true);

            imgDecoder.ConfigureInputPort(decoderInputConfig, inputCaptureHandler)
                .ConfigureOutputPort(0, decoderOutputConfig, null);

            splitter.ConfigureInputPort(splitterInputConfig, null)
                    .ConfigureOutputPort(0, splitterOutputConfig, null);

            imgEncoder.ConfigureInputPort(encoderInputConfig, splitter.Outputs[0], null)
                .ConfigureOutputPort<FileEncodeOutputPort>(0, encoderOutputConfig, outputCaptureHandler);

            imgEncoder2.ConfigureInputPort(encoderInputConfig, splitter.Outputs[1], null)
                .ConfigureOutputPort<FileEncodeOutputPort>(0, encoderOutputConfig, outputCaptureHandler2);

            imgEncoder3.ConfigureInputPort(encoderInputConfig, splitter.Outputs[2], null)
                .ConfigureOutputPort<FileEncodeOutputPort>(0, encoderOutputConfig, outputCaptureHandler3);

            imgEncoder4.ConfigureInputPort(encoderInputConfig, splitter.Outputs[3], null)
                .ConfigureOutputPort<FileEncodeOutputPort>(0, encoderOutputConfig, outputCaptureHandler4);

            imgDecoder.Outputs[0].ConnectTo(splitter);
            splitter.Outputs[0].ConnectTo(imgEncoder);
            splitter.Outputs[1].ConnectTo(imgEncoder2);
            splitter.Outputs[2].ConnectTo(imgEncoder3);
            splitter.Outputs[3].ConnectTo(imgEncoder4);

            Fixture.MMALStandalone.PrintPipeline(imgDecoder);

            await Fixture.MMALStandalone.ProcessAsync(imgDecoder);

            Fixture.CheckAndAssertFilepath(outputCaptureHandler.GetFilepath());
            Fixture.CheckAndAssertFilepath(outputCaptureHandler2.GetFilepath());
            Fixture.CheckAndAssertFilepath(outputCaptureHandler3.GetFilepath());
            Fixture.CheckAndAssertFilepath(outputCaptureHandler4.GetFilepath());
        }

        [Fact]
        public async Task FullPipelineWithSplitterAndResizer()
        {
            // In this test we are first recording a H.264 video using YUV420 encoding.
            // The output file is then fed to a decoder -> splitter -> (resizer) -> 4 encoder components.
            // Only one of the splitter outputs is connected to a resizer. The outputted file will be significantly smaller
            // than the rest.
            TestHelper.BeginTest("Video - FullPipelineWithSplitter");
            TestHelper.SetConfigurationDefaults();
            TestHelper.CleanDirectory("/home/pi/videos/tests");

            string videoFilepath = string.Empty;
            
            // First take a new H.264 video using YUV420 encoding.
            using (var videoCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/tests", "h264"))
            using (var preview = new MmalNullSinkComponent())
            using (var vidEncoder = new MmalVideoEncoder())
            {
                Fixture.MalCamera.ConfigureCameraSettings();

                var portConfig = new MmalPortConfig(MmalEncoding.H264, MmalEncoding.I420, 0, MmalVideoEncoder.MaxBitrateLevel4, null);

                vidEncoder.ConfigureOutputPort(portConfig, videoCaptureHandler);

                // Create our component pipeline.         
                Fixture.MalCamera.Camera.VideoPort
                    .ConnectTo(vidEncoder);
                Fixture.MalCamera.Camera.PreviewPort
                    .ConnectTo(preview);

                // Camera warm up time
                await Task.Delay(2000);

                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));

                await Fixture.MalCamera.ProcessAsync(Fixture.MalCamera.Camera.VideoPort, cts.Token);

                Fixture.CheckAndAssertFilepath(videoCaptureHandler.GetFilepath());

                videoFilepath = videoCaptureHandler.GetFilepath();
            }

            await using var stream = File.OpenRead(videoFilepath);
            using var inputCaptureHandler = new InputCaptureHandler(stream);
            using var outputCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/tests", "h264");
            using var outputCaptureHandler2 = new VideoStreamCaptureHandler("/home/pi/videos/tests", "h264");
            using var outputCaptureHandler3 = new VideoStreamCaptureHandler("/home/pi/videos/tests", "h264");
            using var outputCaptureHandler4 = new VideoStreamCaptureHandler("/home/pi/videos/tests", "h264");
            using var splitter = new MmalSplitterComponent();
            using var imgDecoder = new MmalVideoDecoder();
            using var imgEncoder = new MmalVideoEncoder();
            using var imgEncoder2 = new MmalVideoEncoder();
            using var imgEncoder3 = new MmalVideoEncoder();
            using var imgEncoder4 = new MmalVideoEncoder();
            using var resizer = new MmalResizerComponent();
            var splitterInputConfig = new MmalPortConfig(MmalEncoding.I420, null, framerate: 25, zeroCopy: true);
            var splitterOutputConfig = new MmalPortConfig(null, null, zeroCopy: true);

            // Resize from 640x480 to 320x120.
            var resizerInputConfig = new MmalPortConfig(MmalEncoding.I420, null, width: 640, height: 480, framerate: 25, zeroCopy: true);
            var resizerOutputConfig = new MmalPortConfig(null, null, width: 320, height: 120, zeroCopy: true);

            var decoderInputConfig = new MmalPortConfig(MmalEncoding.H264, null, width: 640, height: 480, framerate: 25, zeroCopy: true);
            var decoderOutputConfig = new MmalPortConfig(MmalEncoding.I420, null, width: 640, height: 480, zeroCopy: true);

            var encoderInputConfig = new MmalPortConfig(MmalEncoding.I420, null, width: 640, height: 480, zeroCopy: true);
            var encoderOutputConfig = new MmalPortConfig(MmalEncoding.H264, MmalEncoding.I420, width: 640, height: 480, framerate: 25, zeroCopy: true);

            imgDecoder.ConfigureInputPort(decoderInputConfig, inputCaptureHandler)
                .ConfigureOutputPort(0, decoderOutputConfig, null);

            splitter.ConfigureInputPort(splitterInputConfig, null)
                    .ConfigureOutputPort(0, splitterOutputConfig, null);

            imgEncoder.ConfigureInputPort(encoderInputConfig, splitter.Outputs[0], null)
                .ConfigureOutputPort<FileEncodeOutputPort>(0, encoderOutputConfig, outputCaptureHandler);

            imgEncoder2.ConfigureInputPort(encoderInputConfig, splitter.Outputs[1], null)
                .ConfigureOutputPort<FileEncodeOutputPort>(0, encoderOutputConfig, outputCaptureHandler2);

            imgEncoder3.ConfigureInputPort(encoderInputConfig, splitter.Outputs[2], null)
                .ConfigureOutputPort<FileEncodeOutputPort>(0, encoderOutputConfig, outputCaptureHandler3);

            resizer.ConfigureInputPort(resizerInputConfig, null)
                    .ConfigureOutputPort(0, resizerOutputConfig, null);

            imgEncoder4.ConfigureInputPort(encoderInputConfig, resizer.Outputs[0], null)
                .ConfigureOutputPort<FileEncodeOutputPort>(0, encoderOutputConfig, outputCaptureHandler4);

            imgDecoder.Outputs[0].ConnectTo(splitter);
            splitter.Outputs[0].ConnectTo(imgEncoder);
            splitter.Outputs[1].ConnectTo(imgEncoder2);
            splitter.Outputs[2].ConnectTo(imgEncoder3);
            splitter.Outputs[3].ConnectTo(resizer);
            resizer.Outputs[0].ConnectTo(imgEncoder4);

            Fixture.MMALStandalone.PrintPipeline(imgDecoder);

            await Fixture.MMALStandalone.ProcessAsync(imgDecoder);

            Fixture.CheckAndAssertFilepath(outputCaptureHandler.GetFilepath());
            Fixture.CheckAndAssertFilepath(outputCaptureHandler2.GetFilepath());
            Fixture.CheckAndAssertFilepath(outputCaptureHandler3.GetFilepath());
            Fixture.CheckAndAssertFilepath(outputCaptureHandler4.GetFilepath());
        }
    }
}
