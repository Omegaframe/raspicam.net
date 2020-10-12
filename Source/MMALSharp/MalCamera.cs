using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MMALSharp.Common;
using MMALSharp.Common.Utility;
using MMALSharp.Components;
using MMALSharp.Components.EncoderComponents;
using MMALSharp.Config;
using MMALSharp.Extensions;
using MMALSharp.Native;
using MMALSharp.Ports;
using MMALSharp.Ports.Outputs;
using MMALSharp.Processing.Handlers;
using MMALSharp.Processing.Processors.Motion;

namespace MMALSharp
{
    public sealed class MalCamera
    {
        public static MalCamera Instance => Lazy.Value;

        static readonly Lazy<MalCamera> Lazy = new Lazy<MalCamera>(() => new MalCamera());

        public MmalCameraComponent Camera { get; }

        MalCamera()
        {
            BcmHost.Initialize();

            Camera = new MmalCameraComponent();
        }

        public void StartCapture(IOutputPort port)
        {
            if (port == Camera.StillPort || port == Camera.VideoPort)
                port.SetImageCapture(true);
        }

        public void StopCapture(IOutputPort port)
        {
            if (port == Camera.StillPort || port == Camera.VideoPort)
                port.SetImageCapture(false);
        }

        public void ForceStop(IOutputPort port)
        {
            Task.Run(() => port.Trigger.SetResult(true));
        }

        public async Task TakeRawVideo(IVideoCaptureHandler handler, CancellationToken cancellationToken)
        {
            using var splitter = new MmalSplitterComponent();
            using var renderer = new MmalVideoRenderer();
            ConfigureCameraSettings();

            var splitterOutputConfig = new MmalPortConfig(MmalCameraConfig.Encoding, MmalCameraConfig.EncodingSubFormat);

            // Force port type to SplitterVideoPort to prevent resolution from being set against splitter component.
            splitter.ConfigureOutputPort<SplitterVideoPort>(0, splitterOutputConfig, handler);

            // Create our component pipeline.
            Camera.VideoPort.ConnectTo(splitter);
            Camera.PreviewPort.ConnectTo(renderer);

            MmalLog.Logger.LogInformation($"Preparing to take raw video. Resolution: {Camera.VideoPort.Resolution.Width} x {Camera.VideoPort.Resolution.Height}. " +
                                          $"Encoder: {MmalCameraConfig.Encoding.EncodingName}. Pixel Format: {MmalCameraConfig.EncodingSubFormat.EncodingName}.");

            // Camera warm up time
            await Task.Delay(2000, cancellationToken).ConfigureAwait(false);
            await ProcessAsync(Camera.VideoPort, cancellationToken).ConfigureAwait(false);
        }

        public async Task TakeVideo(IVideoCaptureHandler handler, CancellationToken cancellationToken, Split split = null)
        {
            if (split != null && !MmalCameraConfig.InlineHeaders)
            {
                MmalLog.Logger.LogWarning("Inline headers not enabled. Split mode not supported when this is disabled.");
                split = null;
            }

            using var vidEncoder = new MmalVideoEncoder();
            using var renderer = new MmalVideoRenderer();
            ConfigureCameraSettings();

            var portConfig = new MmalPortConfig(MmalEncoding.H264, MmalEncoding.I420, 10, MmalVideoEncoder.MaxBitrateLevel4, split: split);

            vidEncoder.ConfigureOutputPort(portConfig, handler);

            // Create our component pipeline.
            Camera.VideoPort.ConnectTo(vidEncoder);
            Camera.PreviewPort.ConnectTo(renderer);

            MmalLog.Logger.LogInformation($"Preparing to take video. Resolution: {Camera.VideoPort.Resolution.Width} x {Camera.VideoPort.Resolution.Height}. " +
                                          $"Encoder: {vidEncoder.Outputs[0].EncodingType.EncodingName}. Pixel Format: {vidEncoder.Outputs[0].PixelFormat.EncodingName}.");

            // Camera warm up time
            await Task.Delay(2000, cancellationToken).ConfigureAwait(false);
            await ProcessAsync(Camera.VideoPort, cancellationToken).ConfigureAwait(false);
        }

        public async Task TakeRawPicture(IOutputCaptureHandler handler)
        {
            if (Camera.StillPort.ConnectedReference != null)
                throw new PiCameraError("A connection was found to the Camera still port. No encoder should be connected to the Camera's still port for raw capture.");

            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            using var renderer = new MmalNullSinkComponent();
            ConfigureCameraSettings(handler);
            Camera.PreviewPort.ConnectTo(renderer);

            MmalLog.Logger.LogInformation($"Preparing to take raw picture - Resolution: {Camera.StillPort.Resolution.Width} x {Camera.StillPort.Resolution.Height}. " +
                                          $"Encoder: {MmalCameraConfig.Encoding.EncodingName}. Pixel Format: {MmalCameraConfig.EncodingSubFormat.EncodingName}.");

            // Camera warm up time
            await Task.Delay(2000).ConfigureAwait(false);
            await ProcessAsync(Camera.StillPort).ConfigureAwait(false);
        }

        public async Task TakePicture(IOutputCaptureHandler handler, MmalEncoding encodingType, MmalEncoding pixelFormat)
        {
            using var imgEncoder = new MmalImageEncoder();
            using var renderer = new MmalNullSinkComponent();
            ConfigureCameraSettings();

            var portConfig = new MmalPortConfig(encodingType, pixelFormat, 90);

            imgEncoder.ConfigureOutputPort(portConfig, handler);

            // Create our component pipeline.
            Camera.StillPort.ConnectTo(imgEncoder);
            Camera.PreviewPort.ConnectTo(renderer);

            MmalLog.Logger.LogInformation($"Preparing to take picture. Resolution: {Camera.StillPort.Resolution.Width} x {Camera.StillPort.Resolution.Height}. " +
                                          $"Encoder: {encodingType.EncodingName}. Pixel Format: {pixelFormat.EncodingName}.");

            // Camera warm up time
            await Task.Delay(2000).ConfigureAwait(false);
            await ProcessAsync(Camera.StillPort).ConfigureAwait(false);
        }

        public async Task TakePictureTimeout(IFileStreamCaptureHandler handler, MmalEncoding encodingType, MmalEncoding pixelFormat, CancellationToken cancellationToken, bool burstMode = false)
        {
            if (burstMode)
                MmalCameraConfig.StillBurstMode = true;

            using var imgEncoder = new MmalImageEncoder();
            using var renderer = new MmalNullSinkComponent();
            ConfigureCameraSettings();

            var portConfig = new MmalPortConfig(encodingType, pixelFormat, 90);

            imgEncoder.ConfigureOutputPort(portConfig, handler);

            // Create our component pipeline.
            Camera.StillPort.ConnectTo(imgEncoder);
            Camera.PreviewPort.ConnectTo(renderer);

            // Camera warm up time
            await Task.Delay(2000, cancellationToken).ConfigureAwait(false);

            while (!cancellationToken.IsCancellationRequested)
            {
                await ProcessAsync(Camera.StillPort, cancellationToken).ConfigureAwait(false);

                if (!cancellationToken.IsCancellationRequested)
                    handler.NewFile();
            }
        }

        public async Task TakePictureTimelapse(IFileStreamCaptureHandler handler, MmalEncoding encodingType, MmalEncoding pixelFormat, Timelapse timelapse)
        {
            var interval = 0;

            if (timelapse == null)
                throw new ArgumentNullException(nameof(timelapse), "Timelapse object null. This must be initialized for Timelapse mode");

            using var imgEncoder = new MmalImageEncoder();
            using var renderer = new MmalNullSinkComponent();
            ConfigureCameraSettings();

            var portConfig = new MmalPortConfig(encodingType, pixelFormat, 90);

            imgEncoder.ConfigureOutputPort(portConfig, handler);

            // Create our component pipeline.
            Camera.StillPort.ConnectTo(imgEncoder);
            Camera.PreviewPort.ConnectTo(renderer);

            // Camera warm up time
            await Task.Delay(2000).ConfigureAwait(false);

            while (!timelapse.CancellationToken.IsCancellationRequested)
            {
                interval = timelapse.Mode switch
                {
                    TimelapseMode.Millisecond => timelapse.Value,
                    TimelapseMode.Second => timelapse.Value * 1000,
                    TimelapseMode.Minute => (timelapse.Value * 60) * 1000,
                    _ => interval
                };

                await Task.Delay(interval).ConfigureAwait(false);

                MmalLog.Logger.LogInformation($"Preparing to take picture. Resolution: {MmalCameraConfig.Resolution.Width} x {MmalCameraConfig.Resolution.Height}. " +
                                              $"Encoder: {encodingType.EncodingName}. Pixel Format: {pixelFormat.EncodingName}.");

                await ProcessAsync(Camera.StillPort).ConfigureAwait(false);

                if (!timelapse.CancellationToken.IsCancellationRequested)
                    handler.NewFile();
            }
        }

        public async Task ProcessAsync(IOutputPort cameraPort, CancellationToken cancellationToken = default)
        {
            var handlerComponents = PopulateProcessingList();

            if (handlerComponents.Count == 0)
            {
                await ProcessRawAsync(cameraPort, cancellationToken);
                return;
            }

            var tasks = new List<Task>();

            // Enable all connections associated with these components
            foreach (var component in handlerComponents)
            {
                component.ForceStopProcessing = false;

                foreach (var port in component.ProcessingPorts.Values.Where(port => port.ConnectedReference == null))
                {
                    port.Start();
                    tasks.Add(port.Trigger.Task);
                }

                component.EnableConnections();
            }

            Camera.SetShutterSpeed(MmalCameraConfig.ShutterSpeed);

            // Prepare arguments for the annotation-refresh task
            var ctsRefreshAnnotation = new CancellationTokenSource();
            var refreshInterval = (int)(MmalCameraConfig.Annotate?.RefreshRate ?? 0);

            if (!(MmalCameraConfig.Annotate?.ShowDateText ?? false) && !(MmalCameraConfig.Annotate?.ShowTimeText ?? false))
                refreshInterval = 0;

            // We now begin capturing on the camera, processing will commence based on the pipeline configured.
            StartCapture(cameraPort);

            if (cancellationToken == CancellationToken.None)
            {
                await Task.WhenAny(Task.WhenAll(tasks), RefreshAnnotations(refreshInterval, ctsRefreshAnnotation.Token)).ConfigureAwait(false);

                ctsRefreshAnnotation.Cancel();
            }
            else
            {
                await Task.WhenAny(
                    Task.WhenAll(tasks),
                    RefreshAnnotations(refreshInterval, ctsRefreshAnnotation.Token),
                    Task.Delay(-1, cancellationToken)).ConfigureAwait(false);

                ctsRefreshAnnotation.Cancel();

                foreach (var component in handlerComponents)
                    component.ForceStopProcessing = true;

                await Task.WhenAll(tasks).ConfigureAwait(false);
            }

            StopCapture(cameraPort);

            // Cleanup each connected downstream component.
            foreach (var component in handlerComponents)
            {
                foreach (var port in component.ProcessingPorts.Values.Where(port => port.ConnectedReference == null))
                    port.DisablePort();

                component.CleanPortPools();
                component.DisableConnections();
            }
        }

        public void PrintPipeline()
        {
            MmalLog.Logger.LogInformation("Current pipeline:");
            MmalLog.Logger.LogInformation(string.Empty);

            Camera.PrintComponent();

            foreach (var component in MmalBootstrapper.DownstreamComponents)
                component.PrintComponent();
        }

        public void DisableCamera() => Camera.DisableComponent();
        public void EnableCamera() => Camera.EnableComponent();

        public MalCamera ConfigureCameraSettings(IOutputCaptureHandler stillCaptureHandler = null, IOutputCaptureHandler videoCaptureHandler = null)
        {
            Camera.Initialise(stillCaptureHandler, videoCaptureHandler);
            return this;
        }

        public MalCamera EnableAnnotation()
        {
            Camera.SetAnnotateSettings();
            return this;
        }

        public MalCamera DisableAnnotation()
        {
            Camera.DisableAnnotate();
            return this;
        }

        public MmalOverlayRenderer AddOverlay(MmalVideoRenderer parent, PreviewOverlayConfiguration config, byte[] source) => new MmalOverlayRenderer(parent, config, source);

        public MalCamera WithMotionDetection(IMotionCaptureHandler handler, MotionConfig config, Action onDetect)
        {
            MmalCameraConfig.InlineMotionVectors = true;
            handler.ConfigureMotionDetection(config, onDetect);
            return this;
        }

        public void Cleanup()
        {
            MmalLog.Logger.LogDebug("Destroying final components");

            var tempList = new List<MmalDownstreamComponent>(MmalBootstrapper.DownstreamComponents);

            tempList.ForEach(c => c.Dispose());
            Camera.Dispose();

            BcmHost.Uninitialize();
        }

        async Task RefreshAnnotations(int msInterval, CancellationToken cancellationToken)
        {
            try
            {
                if (msInterval == 0)
                {
                    await Task.Delay(Timeout.Infinite, cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        await Task.Delay(msInterval, cancellationToken).ConfigureAwait(false);
                        Camera.SetAnnotateSettings();
                    }
                }
            }
            catch (OperationCanceledException)
            { // disregard token cancellation
            }
        }

        async Task ProcessRawAsync(IOutputPort cameraPort, CancellationToken cancellationToken = default)
        {
            using (cancellationToken.Register(() => cameraPort.Trigger.SetResult(true)))
            {
                cameraPort.DisablePort();
                cameraPort.Start();

                StartCapture(cameraPort);
                await cameraPort.Trigger.Task.ConfigureAwait(false);

                StopCapture(cameraPort);
                Camera.CleanPortPools();
            }
        }

        List<IDownstreamComponent> PopulateProcessingList()
        {
            var list = new List<IDownstreamComponent>();
            var initialStillDownstream = Camera.StillPort.ConnectedReference?.DownstreamComponent;
            var initialVideoDownstream = Camera.VideoPort.ConnectedReference?.DownstreamComponent;
            var initialPreviewDownstream = Camera.PreviewPort.ConnectedReference?.DownstreamComponent;

            if (initialStillDownstream != null)
                FindComponents(initialStillDownstream, list);

            if (initialVideoDownstream != null)
                FindComponents(initialVideoDownstream, list);

            if (initialPreviewDownstream != null)
                FindComponents(initialPreviewDownstream, list);

            return list;
        }

        static void FindComponents(IDownstreamComponent downstream, List<IDownstreamComponent> list)
        {
            if (downstream.Outputs.Count == 0)
                return;

            if (downstream.Outputs.Count == 1 && downstream.Outputs[0].ConnectedReference == null)
            {
                list.Add(downstream);
                return;
            }

            if (downstream.GetType().BaseType == typeof(MmalDownstreamHandlerComponent))
                list.Add((MmalDownstreamHandlerComponent)downstream);

            foreach (var output in downstream.Outputs.Where(output => output.ConnectedReference != null))
                FindComponents(output.ConnectedReference.DownstreamComponent, list);
        }
    }
}
