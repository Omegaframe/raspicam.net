// <copyright file="VideoEncoderTests.cs" company="Techyian">
// Copyright (c) Ian Auty and contributors. All rights reserved.
// Licensed under the MIT License. Please see LICENSE.txt for License info.
// </copyright>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MMALSharp.Common;
using MMALSharp.Components;
using MMALSharp.Config;
using MMALSharp.Extensions;
using MMALSharp.Native;
using MMALSharp.Ports;
using MMALSharp.Ports.Outputs;
using MMALSharp.Processing.Handlers;
using MMALSharp.Tests.Data;
using Xunit;

namespace MMALSharp.Tests
{
    public class VideoEncoderTests : TestBase
    {
        public VideoEncoderTests(MMALFixture fixture)
           : base(fixture)
        {
        }

        #region Configuration tests

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [MMALTestsAttribute]
        public void SetThenGetVideoStabilisation(bool vstab)
        {
            MmalCameraConfig.VideoStabilisation = vstab;
            Fixture.MalCamera.ConfigureCameraSettings();
            Assert.True(Fixture.MalCamera.Camera.GetVideoStabilisation() == vstab);
        }

        #endregion

        [Theory]
        [MemberData(nameof(VideoData.Data), MemberType = typeof(VideoData))]
        public async Task TakeVideo(string extension, MmalEncoding encodingType, MmalEncoding pixelFormat)
        {
            TestHelper.BeginTest("TakeVideo", encodingType.EncodingName, pixelFormat.EncodingName);
            TestHelper.SetConfigurationDefaults();
            TestHelper.CleanDirectory("/home/pi/videos/tests");
                
            using (var vidCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/tests", extension))
            using (var preview = new MMALVideoRenderer())
            using (var vidEncoder = new MMALVideoEncoder())
            {
                Fixture.MalCamera.ConfigureCameraSettings();

                var portConfig = new MMALPortConfig(encodingType, pixelFormat, quality: 10, bitrate: 25000000);

                vidEncoder.ConfigureOutputPort(portConfig, vidCaptureHandler);

                // Create our component pipeline.         
                Fixture.MalCamera.Camera.VideoPort
                    .ConnectTo(vidEncoder);
                Fixture.MalCamera.Camera.PreviewPort
                    .ConnectTo(preview);
                    
                // Camera warm up time
                await Task.Delay(2000);

                CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));

                // Record video for 15 seconds
                await Fixture.MalCamera.ProcessAsync(Fixture.MalCamera.Camera.VideoPort, cts.Token);

                Fixture.CheckAndAssertFilepath(vidCaptureHandler.GetFilepath());
            }
        }

        [Fact]
        public async Task TakeVideoSplit()
        {
            TestHelper.BeginTest("TakeVideoSplit");
            TestHelper.SetConfigurationDefaults();
            TestHelper.CleanDirectory("/home/pi/videos/tests/split_test");

            MmalCameraConfig.InlineHeaders = true;
            
            using (var vidCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/tests/split_test", "h264"))
            using (var preview = new MMALVideoRenderer())
            using (var vidEncoder = new MMALVideoEncoder())
            {
                Fixture.MalCamera.ConfigureCameraSettings();
                                
                var split = new Split
                {
                    Mode = TimelapseMode.Second,
                    Value = 15
                };

                var portConfig = new MMALPortConfig(MmalEncoding.H264, MmalEncoding.I420, quality: 10, bitrate: 25000000, split: split);

                vidEncoder.ConfigureOutputPort(portConfig, vidCaptureHandler);

                // Create our component pipeline.         
                Fixture.MalCamera.Camera.VideoPort
                    .ConnectTo(vidEncoder);
                Fixture.MalCamera.Camera.PreviewPort
                    .ConnectTo(preview);

                // Camera warm up time
                await Task.Delay(2000);

                CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

                // 2 files should be created from this test. 
                await Fixture.MalCamera.ProcessAsync(Fixture.MalCamera.Camera.VideoPort, cts.Token);
                    
                Assert.True(Directory.GetFiles("/home/pi/videos/tests/split_test").Length == 2);
            }
        }

        [Fact]
        public async Task ChangeEncoderType()
        {
            TestHelper.BeginTest("Video - ChangeEncoderType");
            TestHelper.SetConfigurationDefaults();
            TestHelper.CleanDirectory("/home/pi/videos/tests");
                
            using (var vidCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/tests", "h264"))
            using (var preview = new MMALVideoRenderer())
            using (var vidEncoder = new MMALVideoEncoder())
            {
                Fixture.MalCamera.ConfigureCameraSettings();

                var portConfig = new MMALPortConfig(MmalEncoding.MJpeg, MmalEncoding.I420, quality: 10, bitrate: 25000000);

                vidEncoder.ConfigureOutputPort(portConfig, vidCaptureHandler);

                // Create our component pipeline.         
                Fixture.MalCamera.Camera.VideoPort
                    .ConnectTo(vidEncoder);
                Fixture.MalCamera.Camera.PreviewPort
                    .ConnectTo(preview);

                // Camera warm up time
                await Task.Delay(2000);

                CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));

                // Record video for 20 seconds
                await Fixture.MalCamera.ProcessAsync(Fixture.MalCamera.Camera.VideoPort, cts.Token);

                Fixture.CheckAndAssertFilepath(vidCaptureHandler.GetFilepath());
            }
                
            using (var vidCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/tests", "mjpeg"))
            using (var preview = new MMALVideoRenderer())
            using (var vidEncoder = new MMALVideoEncoder())
            {
                Fixture.MalCamera.ConfigureCameraSettings();

                var portConfig = new MMALPortConfig(MmalEncoding.MJpeg, MmalEncoding.I420, quality: 10, bitrate: 25000000);

                vidEncoder.ConfigureOutputPort(portConfig, vidCaptureHandler);

                // Create our component pipeline.         
                Fixture.MalCamera.Camera.VideoPort
                    .ConnectTo(vidEncoder);
                Fixture.MalCamera.Camera.PreviewPort
                    .ConnectTo(preview);

                CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));

                // Record video for 20 seconds
                await Fixture.MalCamera.ProcessAsync(Fixture.MalCamera.Camera.VideoPort, cts.Token);

                Fixture.CheckAndAssertFilepath(vidCaptureHandler.GetFilepath());
            }
        }

        [Fact]
        public async Task VideoSplitterComponent()
        {
            TestHelper.BeginTest("VideoSplitterComponent");
            TestHelper.SetConfigurationDefaults();
            TestHelper.CleanDirectory("/home/pi/videos/tests");

            using (var handler = new VideoStreamCaptureHandler("/home/pi/videos/tests", "h264"))
            using (var handler2 = new VideoStreamCaptureHandler("/home/pi/videos/tests", "h264"))
            using (var handler3 = new VideoStreamCaptureHandler("/home/pi/videos/tests", "h264"))
            using (var handler4 = new VideoStreamCaptureHandler("/home/pi/videos/tests", "h264"))
            using (var splitter = new MMALSplitterComponent())
            using (var vidEncoder = new MMALVideoEncoder())
            using (var vidEncoder2 = new MMALVideoEncoder())
            using (var vidEncoder3 = new MMALVideoEncoder())
            using (var vidEncoder4 = new MMALVideoEncoder())
            using (var renderer = new MMALNullSinkComponent())
            {
                Fixture.MalCamera.ConfigureCameraSettings();

                var splitterPortConfig = new MMALPortConfig(MmalEncoding.Opaque, MmalEncoding.I420, quality: 0, bitrate: 13000000);
                var portConfig1 = new MMALPortConfig(MmalEncoding.H264, MmalEncoding.I420, quality: 10, bitrate: 13000000, timeout: DateTime.Now.AddSeconds(20));
                var portConfig2 = new MMALPortConfig(MmalEncoding.H264, MmalEncoding.I420, quality: 20, bitrate: 13000000, timeout: DateTime.Now.AddSeconds(15));
                var portConfig3 = new MMALPortConfig(MmalEncoding.H264, MmalEncoding.I420, quality: 30, bitrate: 13000000, timeout: DateTime.Now.AddSeconds(10));
                var portConfig4 = new MMALPortConfig(MmalEncoding.H264, MmalEncoding.I420, quality: 40, bitrate: 13000000, timeout: DateTime.Now.AddSeconds(10));

                // Create our component pipeline.         
                splitter.ConfigureInputPort(new MMALPortConfig(MmalEncoding.Opaque, MmalEncoding.I420), Fixture.MalCamera.Camera.VideoPort, null);
                splitter.ConfigureOutputPort(0, splitterPortConfig, null);
                splitter.ConfigureOutputPort(1, splitterPortConfig, null);
                splitter.ConfigureOutputPort(2, splitterPortConfig, null);
                splitter.ConfigureOutputPort(3, splitterPortConfig, null);

                vidEncoder.ConfigureInputPort(new MMALPortConfig(MmalEncoding.Opaque, MmalEncoding.I420), splitter.Outputs[0], null);
                vidEncoder.ConfigureOutputPort(0, portConfig1, handler);

                vidEncoder2.ConfigureInputPort(new MMALPortConfig(MmalEncoding.Opaque, MmalEncoding.I420), splitter.Outputs[1], null);
                vidEncoder2.ConfigureOutputPort(0, portConfig2, handler2);
                vidEncoder3.ConfigureInputPort(new MMALPortConfig(MmalEncoding.Opaque, MmalEncoding.I420), splitter.Outputs[2], null);
                vidEncoder3.ConfigureOutputPort(0, portConfig3, handler3);

                vidEncoder4.ConfigureInputPort(new MMALPortConfig(MmalEncoding.Opaque, MmalEncoding.I420), splitter.Outputs[3], null);
                vidEncoder4.ConfigureOutputPort(0, portConfig4, handler4);

                Fixture.MalCamera.Camera.VideoPort.ConnectTo(splitter);

                splitter.Outputs[0].ConnectTo(vidEncoder);
                splitter.Outputs[1].ConnectTo(vidEncoder2);
                splitter.Outputs[2].ConnectTo(vidEncoder3);
                splitter.Outputs[3].ConnectTo(vidEncoder4);

                Fixture.MalCamera.Camera.PreviewPort.ConnectTo(renderer);

                // Camera warm up time
                await Task.Delay(2000);

                await Fixture.MalCamera.ProcessAsync(Fixture.MalCamera.Camera.VideoPort);

                Fixture.CheckAndAssertFilepath(handler.GetFilepath());
                Fixture.CheckAndAssertFilepath(handler2.GetFilepath());
                Fixture.CheckAndAssertFilepath(handler3.GetFilepath());
                Fixture.CheckAndAssertFilepath(handler4.GetFilepath());
            }
        }
        
        [Fact]
        public void ChangeColorSpace()
        {
            TestHelper.BeginTest("ChangeColorSpace");
            TestHelper.SetConfigurationDefaults();
            TestHelper.CleanDirectory("/home/pi/videos/tests");

            MmalCameraConfig.VideoColorSpace = MmalEncoding.MmalColorSpaceIturBt601;
            
            using (var handler = new VideoStreamCaptureHandler("/home/pi/videos/tests", "h264"))
            using (var handler2 = new VideoStreamCaptureHandler("/home/pi/video/tests", "h264"))
            using (var handler3 = new VideoStreamCaptureHandler("/home/pi/video/tests", "h264"))
            using (var handler4 = new VideoStreamCaptureHandler("/home/pi/video/tests", "h264"))
            using (var splitter = new MMALSplitterComponent())
            using (var vidEncoder = new MMALVideoEncoder())
            using (var vidEncoder2 = new MMALVideoEncoder())
            using (var vidEncoder3 = new MMALVideoEncoder())
            using (var vidEncoder4 = new MMALVideoEncoder())
            using (var renderer = new MMALVideoRenderer())
            {
                Fixture.MalCamera.ConfigureCameraSettings();

                var splitterPortConfig = new MMALPortConfig(MmalEncoding.Opaque, MmalEncoding.I420, bitrate: 13000000);
                var portConfig1 = new MMALPortConfig(MmalEncoding.H264, MmalEncoding.I420, quality: 10, bitrate: 13000000, timeout: DateTime.Now.AddSeconds(20));
                var portConfig2 = new MMALPortConfig(MmalEncoding.H264, MmalEncoding.I420, quality: 20, bitrate: 13000000, timeout: DateTime.Now.AddSeconds(15));
                var portConfig3 = new MMALPortConfig(MmalEncoding.H264, MmalEncoding.I420, quality: 30, bitrate: 13000000, timeout: DateTime.Now.AddSeconds(10));
                var portConfig4 = new MMALPortConfig(MmalEncoding.H264, MmalEncoding.I420, quality: 40, bitrate: 13000000, timeout: DateTime.Now.AddSeconds(10));

                // Create our component pipeline.         
                splitter.ConfigureInputPort(new MMALPortConfig(MmalEncoding.Opaque, MmalEncoding.I420), Fixture.MalCamera.Camera.VideoPort, null);
                splitter.ConfigureOutputPort(0, splitterPortConfig, null);
                splitter.ConfigureOutputPort(1, splitterPortConfig, null);
                splitter.ConfigureOutputPort(2, splitterPortConfig, null);
                splitter.ConfigureOutputPort(3, splitterPortConfig, null);

                vidEncoder.ConfigureInputPort(new MMALPortConfig(MmalEncoding.Opaque, MmalEncoding.I420), splitter.Outputs[0], null);
                vidEncoder.ConfigureOutputPort(0, portConfig1, handler);

                vidEncoder2.ConfigureInputPort(new MMALPortConfig(MmalEncoding.Opaque, MmalEncoding.I420), splitter.Outputs[1], null);
                vidEncoder2.ConfigureOutputPort(0, portConfig2, handler2);
                vidEncoder3.ConfigureInputPort(new MMALPortConfig(MmalEncoding.Opaque, MmalEncoding.I420), splitter.Outputs[2], null);
                vidEncoder3.ConfigureOutputPort(0, portConfig3, handler3);

                vidEncoder4.ConfigureInputPort(new MMALPortConfig(MmalEncoding.Opaque, MmalEncoding.I420), splitter.Outputs[3], null);
                vidEncoder4.ConfigureOutputPort(0, portConfig4, handler4);

                Fixture.MalCamera.Camera.VideoPort.ConnectTo(splitter);

                splitter.Outputs[0].ConnectTo(vidEncoder);
                splitter.Outputs[1].ConnectTo(vidEncoder2);
                splitter.Outputs[2].ConnectTo(vidEncoder3);
                splitter.Outputs[3].ConnectTo(vidEncoder4);

                Fixture.MalCamera.Camera.PreviewPort.ConnectTo(renderer);

                // Assert that all output ports have the same video color space.
                Assert.True(Fixture.MalCamera.Camera.VideoPort.VideoColorSpace.EncodingVal == MmalEncoding.MmalColorSpaceIturBt601.EncodingVal);
                
                Assert.True(splitter.Outputs[0].VideoColorSpace.EncodingVal == MmalEncoding.MmalColorSpaceIturBt601.EncodingVal);
                Assert.True(splitter.Outputs[1].VideoColorSpace.EncodingVal == MmalEncoding.MmalColorSpaceIturBt601.EncodingVal);
                Assert.True(splitter.Outputs[2].VideoColorSpace.EncodingVal == MmalEncoding.MmalColorSpaceIturBt601.EncodingVal);
                Assert.True(splitter.Outputs[3].VideoColorSpace.EncodingVal == MmalEncoding.MmalColorSpaceIturBt601.EncodingVal);
                
                Assert.True(vidEncoder.Outputs[0].VideoColorSpace.EncodingVal == MmalEncoding.MmalColorSpaceIturBt601.EncodingVal);
                Assert.True(vidEncoder2.Outputs[0].VideoColorSpace.EncodingVal == MmalEncoding.MmalColorSpaceIturBt601.EncodingVal);
                Assert.True(vidEncoder3.Outputs[0].VideoColorSpace.EncodingVal == MmalEncoding.MmalColorSpaceIturBt601.EncodingVal);
                Assert.True(vidEncoder4.Outputs[0].VideoColorSpace.EncodingVal == MmalEncoding.MmalColorSpaceIturBt601.EncodingVal);
            }
        }

        [Theory]
        [MemberData(nameof(VideoData.H264Data), MemberType = typeof(VideoData))]
        public async Task TakeVideoAndStoreMotionVectors(string extension, MmalEncoding encodingType, MmalEncoding pixelFormat)
        {
            TestHelper.BeginTest("TakeVideoAndStoreMotionVectors", encodingType.EncodingName, pixelFormat.EncodingName);
            TestHelper.SetConfigurationDefaults();
            TestHelper.CleanDirectory("/home/pi/videos/tests");

            // Ensure inline motion vectors are enabled.
            MmalCameraConfig.InlineMotionVectors = true;

            using (var motionVectorStore = File.Create("/home/pi/videos/tests/motion.dat"))
            using (var vidCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/tests", extension))
            using (var preview = new MMALVideoRenderer())
            using (var vidEncoder = new MMALVideoEncoder())
            {
                Fixture.MalCamera.ConfigureCameraSettings();

                var portConfig = new MMALPortConfig(encodingType, pixelFormat, quality: 10, bitrate: 25000000, storeMotionVectors: true);

                vidEncoder.ConfigureOutputPort(portConfig, vidCaptureHandler);

                // Initialise the motion vector stream.
                vidCaptureHandler.InitialiseMotionStore(motionVectorStore);

                // Create our component pipeline.         
                Fixture.MalCamera.Camera.VideoPort
                    .ConnectTo(vidEncoder);
                Fixture.MalCamera.Camera.PreviewPort
                    .ConnectTo(preview);

                // Camera warm up time
                await Task.Delay(2000);

                CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

                // Record video for 10 seconds
                await Fixture.MalCamera.ProcessAsync(Fixture.MalCamera.Camera.VideoPort, cts.Token);

                Fixture.CheckAndAssertFilepath(vidCaptureHandler.GetFilepath());
            }
        }

        [Theory]
        [MemberData(nameof(VideoData.H264Data), MemberType = typeof(VideoData))]
        public async Task TakeVideoWithCircularBuffer(string extension, MmalEncoding encodingType, MmalEncoding pixelFormat)
        {
            TestHelper.BeginTest("TakeVideoWithCircularBuffer", encodingType.EncodingName, pixelFormat.EncodingName);
            TestHelper.SetConfigurationDefaults();
            TestHelper.CleanDirectory("/home/pi/videos/tests");
            
            using (var circularBufferHandler = new CircularBufferCaptureHandler(4096, "/home/pi/videos/tests", extension))            
            using (var preview = new MMALVideoRenderer())
            using (var vidEncoder = new MMALVideoEncoder())
            {
                Fixture.MalCamera.ConfigureCameraSettings();

                var portConfig = new MMALPortConfig(encodingType, pixelFormat, quality: 10, bitrate: 25000000, storeMotionVectors: true);

                vidEncoder.ConfigureOutputPort(portConfig, circularBufferHandler);

                // Create our component pipeline.         
                Fixture.MalCamera.Camera.VideoPort
                    .ConnectTo(vidEncoder);
                Fixture.MalCamera.Camera.PreviewPort
                    .ConnectTo(preview);

                // Camera warm up time
                await Task.Delay(2000);

                CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

                // Record video for 10 seconds
                await Fixture.MalCamera.ProcessAsync(Fixture.MalCamera.Camera.VideoPort, cts.Token);

                // Check that the circular buffer has stored some data during the recording operation.
                Assert.True(circularBufferHandler.Buffer.Size > 0);
            }
        }

        [Theory]
        [MemberData(nameof(ImageFxData.Data), MemberType = typeof(ImageFxData))]
        public async Task ImageFxComponentFromCameraVideoPort(MMAL_PARAM_IMAGEFX_T effect, bool throwsException)
        {
            TestHelper.BeginTest($"Video - ImageFxComponentFromCameraVideoPort - {effect}");
            TestHelper.SetConfigurationDefaults();
            TestHelper.CleanDirectory("/home/pi/videos/tests");

            using (var vidCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/tests", "raw"))
            using (var preview = new MMALNullSinkComponent())
            using (var imageFx = new MMALImageFxComponent())
            {
                Fixture.MalCamera.ConfigureCameraSettings();
                
                var fxConfig = new MMALPortConfig(MmalEncoding.I420, MmalEncoding.I420);

                imageFx.ConfigureOutputPort<VideoPort>(0, fxConfig, vidCaptureHandler);

                if (throwsException)
                {
                    Assert.Throws<MmalInvalidException>(() =>
                    {
                        imageFx.ImageEffect = effect;
                    });
                }
                else
                {
                    imageFx.ImageEffect = effect;
                }

                // Create our component pipeline.         
                Fixture.MalCamera.Camera.VideoPort
                    .ConnectTo(imageFx);
                Fixture.MalCamera.Camera.PreviewPort
                    .ConnectTo(preview);

                // Camera warm up time
                await Task.Delay(2000);

                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

                await Fixture.MalCamera.ProcessAsync(Fixture.MalCamera.Camera.VideoPort, cts.Token);

                Fixture.CheckAndAssertFilepath(vidCaptureHandler.GetFilepath());
            }
        }

        [Theory]
        [MemberData(nameof(ImageFxData.Data), MemberType = typeof(ImageFxData))]
        public async Task ImageFxComponentFromCameraVideoPortWithSplitterAndEncoder(MMAL_PARAM_IMAGEFX_T effect, bool throwsException)
        {
            TestHelper.BeginTest($"Video - ImageFxComponentFromCameraVideoPortWithSplitterAndEncoder - {effect}");
            TestHelper.SetConfigurationDefaults();
            TestHelper.CleanDirectory("/home/pi/videos/tests");

            using (var vidCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/tests", "h264"))
            using (var preview = new MMALNullSinkComponent())
            using (var imageFx = new MMALImageFxComponent())
            using (var splitter = new MMALSplitterComponent())
            using (var vidEncoder = new MMALVideoEncoder())
            {
                Fixture.MalCamera.ConfigureCameraSettings();

                var vidEncoderConfig = new MMALPortConfig(MmalEncoding.H264, MmalEncoding.I420);
                var splitterConfig = new MMALPortConfig(MmalEncoding.I420, MmalEncoding.I420);
                var fxConfig = new MMALPortConfig(MmalEncoding.I420, MmalEncoding.I420);

                imageFx.ConfigureOutputPort<VideoPort>(0, fxConfig, null);

                splitter.ConfigureInputPort(new MMALPortConfig(MmalEncoding.I420, MmalEncoding.I420), imageFx.Outputs[0], null);
                splitter.ConfigureOutputPort<VideoPort>(0, splitterConfig, null);

                vidEncoder.ConfigureInputPort(new MMALPortConfig(MmalEncoding.I420, MmalEncoding.I420), splitter.Outputs[0], null);
                vidEncoder.ConfigureOutputPort(0, vidEncoderConfig, vidCaptureHandler);

                if (throwsException)
                {
                    Assert.Throws<MmalInvalidException>(() =>
                    {
                        imageFx.ImageEffect = effect;
                    });
                }
                else
                {
                    imageFx.ImageEffect = effect;
                }

                // Create our component pipeline.         
                Fixture.MalCamera.Camera.VideoPort
                    .ConnectTo(imageFx);
                Fixture.MalCamera.Camera.PreviewPort
                    .ConnectTo(preview);

                imageFx.Outputs[0].ConnectTo(splitter);
                splitter.Outputs[0].ConnectTo(vidEncoder);

                // Camera warm up time
                await Task.Delay(2000);

                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

                await Fixture.MalCamera.ProcessAsync(Fixture.MalCamera.Camera.VideoPort, cts.Token);

                Fixture.CheckAndAssertFilepath(vidCaptureHandler.GetFilepath());
            }
        }

        [Fact]
        public async Task TakeVideoAndStoreTimestamps()
        {
            TestHelper.BeginTest("Video - TakeVideoAndStoreTimestamps");
            TestHelper.SetConfigurationDefaults();
            TestHelper.CleanDirectory("/home/pi/videos/tests");
            
            using (var vidCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/tests", "h264", true))
            using (var preview = new MMALVideoRenderer())
            using (var vidEncoder = new MMALVideoEncoder())
            {
                Fixture.MalCamera.ConfigureCameraSettings();

                var portConfig = new MMALPortConfig(MmalEncoding.H264, MmalEncoding.I420, bitrate: MMALVideoEncoder.MaxBitrateLevel4);

                vidEncoder.ConfigureOutputPort(portConfig, vidCaptureHandler);

                // Create our component pipeline.         
                Fixture.MalCamera.Camera.VideoPort
                    .ConnectTo(vidEncoder);
                Fixture.MalCamera.Camera.PreviewPort
                    .ConnectTo(preview);

                // Camera warm up time
                await Task.Delay(2000);

                CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));

                // Record video for 15 seconds
                await Fixture.MalCamera.ProcessAsync(Fixture.MalCamera.Camera.VideoPort, cts.Token);

                Fixture.CheckAndAssertFilepath(vidCaptureHandler.GetFilepath());
                Fixture.CheckAndAssertFilepath($"{vidCaptureHandler.Directory}/{vidCaptureHandler.CurrentFilename}.pts");
            }
        }

        [Fact]
        public async Task AnnotateVideoRefreshSeconds()
        {
            TestHelper.BeginTest("Video - AnnotateVideo");
            TestHelper.SetConfigurationDefaults();
            TestHelper.CleanDirectory("/home/pi/videos/tests");

            MmalCameraConfig.Annotate = new AnnotateImage();
            MmalCameraConfig.Annotate.RefreshRate = DateTimeTextRefreshRate.Seconds;
            MmalCameraConfig.Annotate.TimeFormat = "HH:mm:ss";

            using (var vidCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/tests", "h264"))
            using (var vidEncoder = new MMALVideoEncoder())
            using (var renderer = new MMALVideoRenderer())
            {
                Fixture.MalCamera.ConfigureCameraSettings();

                var portConfig = new MMALPortConfig(MmalEncoding.H264, MmalEncoding.I420, quality: 10, bitrate: MMALVideoEncoder.MaxBitrateLevel4);

                // Create our component pipeline. Here we are using the H.264 standard with a YUV420 pixel format. The video will be taken at 25Mb/s.
                vidEncoder.ConfigureOutputPort(portConfig, vidCaptureHandler);

                Fixture.MalCamera.Camera.VideoPort.ConnectTo(vidEncoder);
                Fixture.MalCamera.Camera.PreviewPort.ConnectTo(renderer);

                // Camera warm up time
                await Task.Delay(2000);

                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

                // Take video for 30 seconds.
                await Fixture.MalCamera.ProcessAsync(Fixture.MalCamera.Camera.VideoPort, cts.Token);

                Fixture.CheckAndAssertFilepath(vidCaptureHandler.GetFilepath());
            }
        }
    }
}
