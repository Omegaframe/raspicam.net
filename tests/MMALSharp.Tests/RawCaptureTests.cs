// <copyright file="RawCaptureTests.cs" company="Techyian">
// Copyright (c) Ian Auty and contributors. All rights reserved.
// Licensed under the MIT License. Please see LICENSE.txt for License info.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using MMALSharp.Common;
using MMALSharp.Components;
using MMALSharp.Ports;
using MMALSharp.Ports.Outputs;
using MMALSharp.Processing.Handlers;
using MMALSharp.Tests.Data;
using Xunit;

namespace MMALSharp.Tests
{
    public class RawCaptureTests : TestBase
    {
        public RawCaptureTests(MMALFixture fixture)
            : base(fixture)
        {
        }

        [Fact]
        public async Task RecordVideoDirectlyFromResizer()
        {
            TestHelper.BeginTest("RecordVideoDirectlyFromResizer");
            TestHelper.SetConfigurationDefaults();
            TestHelper.CleanDirectory("/home/pi/videos/tests");

            using var vidCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/tests", "raw");
            using var preview = new MmalVideoRenderer();
            using var resizer = new MmalResizerComponent();

            Fixture.MalCamera.ConfigureCameraSettings();

            // Use the resizer to resize 1080p to 640x480.
            var portConfig = new MmalPortConfig(MmalEncoding.I420, MmalEncoding.I420, width: 640, height: 480);

            resizer.ConfigureOutputPort<VideoPort>(0, portConfig, vidCaptureHandler);

            // Create our component pipeline.         
            Fixture.MalCamera.Camera.VideoPort
                .ConnectTo(resizer);
            Fixture.MalCamera.Camera.PreviewPort
                .ConnectTo(preview);

            // Camera warm up time
            await Task.Delay(2000);

            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            // Record video for 20 seconds
            await Fixture.MalCamera.ProcessAsync(Fixture.MalCamera.Camera.VideoPort, cts.Token);

            Fixture.CheckAndAssertFilepath(vidCaptureHandler.GetFilepath());
        }

        [Fact]
        public async Task RecordVideoDirectlyFromSplitter()
        {
            TestHelper.BeginTest("RecordVideoDirectlyFromSplitter");
            TestHelper.SetConfigurationDefaults();
            TestHelper.CleanDirectory("/home/pi/videos/tests");

            // I am only using a single output here because due to the disk IO performance on the Pi you ideally need to be
            // using a faster storage medium such as the ramdisk to output to multiple files.
            using var vidCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/tests", "raw");
            using var preview = new MmalVideoRenderer();
            using var splitter = new MmalSplitterComponent();

            Fixture.MalCamera.ConfigureCameraSettings();

            var splitterPortConfig = new MmalPortConfig(MmalEncoding.I420, MmalEncoding.I420);

            // Create our component pipeline.         
            splitter.ConfigureInputPort(new MmalPortConfig(MmalEncoding.Opaque, MmalEncoding.I420, 0), Fixture.MalCamera.Camera.VideoPort, null);
            splitter.ConfigureOutputPort(0, splitterPortConfig, vidCaptureHandler);
               
            // Create our component pipeline.         
            Fixture.MalCamera.Camera.VideoPort
                .ConnectTo(splitter);
            Fixture.MalCamera.Camera.PreviewPort
                .ConnectTo(preview);

            // Camera warm up time
            await Task.Delay(2000);

            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            // Record video for 20 seconds
            await Fixture.MalCamera.ProcessAsync(Fixture.MalCamera.Camera.VideoPort, cts.Token);

            Fixture.CheckAndAssertFilepath(vidCaptureHandler.GetFilepath());
        }

        [Fact]
        public async Task RecordVideoDirectlyFromResizerWithSplitterComponent()
        {
            TestHelper.BeginTest("RecordVideoDirectlyFromResizerWithSplitterComponent");
            TestHelper.SetConfigurationDefaults();
            TestHelper.CleanDirectory("/home/pi/videos/tests");

            // I am only using a single output here because due to the disk IO performance on the Pi you ideally need to be
            // using a faster storage medium such as the ramdisk to output to multiple files.
            using var vidCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/tests", "h264");
            using var preview = new MmalVideoRenderer();
            using var splitter = new MmalSplitterComponent();
            using var resizer = new MmalResizerComponent();

            Fixture.MalCamera.ConfigureCameraSettings();

            var splitterPortConfig = new MmalPortConfig(MmalEncoding.Opaque, MmalEncoding.I420);
            var resizerPortConfig = new MmalPortConfig(MmalEncoding.I420, MmalEncoding.I420, width: 1024, height: 768, timeout: DateTime.Now.AddSeconds(15));

            // Create our component pipeline.         
            splitter.ConfigureInputPort(new MmalPortConfig(MmalEncoding.Opaque, MmalEncoding.I420), Fixture.MalCamera.Camera.VideoPort, null);
            splitter.ConfigureOutputPort(0, splitterPortConfig, null);
                
            resizer.ConfigureOutputPort<VideoPort>(0, resizerPortConfig, vidCaptureHandler);
                
            // Create our component pipeline.         
            Fixture.MalCamera.Camera.VideoPort
                .ConnectTo(splitter);

            splitter.Outputs[0].ConnectTo(resizer);

            Fixture.MalCamera.Camera.PreviewPort
                .ConnectTo(preview);

            // Camera warm up time
            await Task.Delay(2000);

            await Fixture.MalCamera.ProcessAsync(Fixture.MalCamera.Camera.VideoPort);

            Fixture.CheckAndAssertFilepath(vidCaptureHandler.GetFilepath());
        }
    }
}
