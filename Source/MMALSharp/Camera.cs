using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MMALSharp.Extensions;
using MMALSharp.Mmal;
using MMALSharp.Mmal.Components;
using MMALSharp.Mmal.Components.EncoderComponents;
using MMALSharp.Mmal.Handlers;
using MMALSharp.Mmal.Ports;
using MMALSharp.Mmal.Ports.Outputs;
using MMALSharp.Native;
using MMALSharp.Utility;

namespace MMALSharp
{
    public sealed class Camera : IDisposable
    {
        const int CameraWarmUpMs = 2000;

        readonly MmalCameraComponent _camera;
        readonly List<IDisposable> _cameraDisposables;

        public Camera()
        {
            BcmHost.Initialize();

            _camera = new MmalCameraComponent();
            _cameraDisposables = new List<IDisposable>();
        }

        public Task Capture(Action<byte[]> onVideoDataAvailable, Action<Stream> onFullFrameAvailable, CancellationToken cancellationToken, int videoQuantisation = 0, int videoBitrate = 2386093, int jpegQuality = 80)
        {
            _camera.Initialise();

            var imageCaptureHandler = new InMemoryImageHandler();
            var videoCaptureHandler = new InMemoryVideoHandler();
            var imgEncoder = new MmalImageEncoder();
            var vidEncoder = new MmalVideoEncoder();
            var splitter = new MmalSplitterComponent();
            var nullSink = new MmalNullSinkComponent();

            videoCaptureHandler.SetOnVideoDataAvailable(data => onVideoDataAvailable?.Invoke(data));
            imageCaptureHandler.SetOnFullFrameAvailable(stream => onFullFrameAvailable?.Invoke(stream));

            _cameraDisposables.AddRange(new IDisposable[] { imageCaptureHandler, videoCaptureHandler, imgEncoder, vidEncoder, splitter, nullSink });

            var imagePortConfig = new MmalPortConfig(MmalEncoding.Jpeg, MmalEncoding.I420, jpegQuality);

            var videoPortConfig = new MmalPortConfig(
                MmalEncoding.H264,
                MmalEncoding.I420,
                videoQuantisation,
                videoBitrate,
                null,
                null,
                false,
                CameraConfig.Resolution.Width,
                CameraConfig.Resolution.Height);

            imgEncoder.ConfigureOutputPort(imagePortConfig, imageCaptureHandler);
            vidEncoder.ConfigureOutputPort(videoPortConfig, videoCaptureHandler);

            _camera.VideoPort.ConnectTo(splitter);
            splitter.Outputs[0].ConnectTo(imgEncoder);
            splitter.Outputs[1].ConnectTo(vidEncoder);
            _camera.PreviewPort.ConnectTo(nullSink);

            // Camera warm up time
            Task.Delay(CameraWarmUpMs).Wait();

            return ProcessAsync(_camera.VideoPort, cancellationToken);
        }

        public Task Capture(Action<byte[]> onVideoDataAvailable, CancellationToken cancellationToken, int videoQuantisation = 0, int videoBitrate = 2386093)
        {
            _camera.Initialise();

            var videoCaptureHandler = new InMemoryVideoHandler();
            var vidEncoder = new MmalVideoEncoder();
            var nullSink = new MmalNullSinkComponent();

            videoCaptureHandler.SetOnVideoDataAvailable(data => onVideoDataAvailable?.Invoke(data));

            _cameraDisposables.AddRange(new IDisposable[] { videoCaptureHandler, vidEncoder, nullSink });

            var videoPortConfig = new MmalPortConfig(
                MmalEncoding.H264,
                MmalEncoding.I420,
                videoQuantisation,
                videoBitrate,
                null,
                null,
                false,
                CameraConfig.Resolution.Width,
                CameraConfig.Resolution.Height);
            ;
            vidEncoder.ConfigureOutputPort(videoPortConfig, videoCaptureHandler);

            _camera.VideoPort.ConnectTo(vidEncoder);
            _camera.PreviewPort.ConnectTo(nullSink);

            // Camera warm up time
            Task.Delay(CameraWarmUpMs).Wait();

            return ProcessAsync(_camera.VideoPort, cancellationToken);
        }

        public Task Capture(Action<Stream> onFullFrameAvailable, CancellationToken cancellationToken, int jpegQuality = 80)
        {
            _camera.Initialise();

            var imageCaptureHandler = new InMemoryImageHandler();
            var imgEncoder = new MmalImageEncoder();
            var nullSink = new MmalNullSinkComponent();

            imageCaptureHandler.SetOnFullFrameAvailable(stream => onFullFrameAvailable?.Invoke(stream));

            _cameraDisposables.AddRange(new IDisposable[] { imageCaptureHandler, imgEncoder, nullSink });

            var imagePortConfig = new MmalPortConfig(MmalEncoding.Jpeg, MmalEncoding.I420, jpegQuality);

            imgEncoder.ConfigureOutputPort(imagePortConfig, imageCaptureHandler);

            _camera.VideoPort.ConnectTo(imgEncoder);
            _camera.PreviewPort.ConnectTo(nullSink);

            // Camera warm up time
            Task.Delay(CameraWarmUpMs).Wait();

            return ProcessAsync(_camera.VideoPort, cancellationToken);
        }

        public void PrintPipeline()
        {
            MmalLog.Logger.LogInformation("Current pipeline:");
            MmalLog.Logger.LogInformation(string.Empty);

            _camera.PrintComponent();

            foreach (var component in MmalBootstrapper.DownstreamComponents)
                component.PrintComponent();
        }

        public void Dispose()
        {
            var tempList = new List<MmalDownstreamComponent>(MmalBootstrapper.DownstreamComponents);

            tempList.ForEach(c => c.Dispose());
            _camera.Dispose();

            BcmHost.Uninitialize();
        }

        void StartCapture(IPort port)
        {
            if (port == _camera.StillPort || port == _camera.VideoPort)
                port.SetImageCapture(true);
        }

        void StopCapture(IPort port)
        {
            if (port == _camera.StillPort || port == _camera.VideoPort)
                port.SetImageCapture(false);
        }

        async Task ProcessAsync(IOutputPort cameraPort, CancellationToken cancellationToken)
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

            // Prepare arguments for the annotation-refresh task
            var ctsRefreshAnnotation = new CancellationTokenSource();
            var refreshInterval = (int)(CameraConfig.Annotate?.RefreshRate ?? 0);

            if (!(CameraConfig.Annotate?.ShowDateText ?? false) && !(CameraConfig.Annotate?.ShowTimeText ?? false))
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

            _cameraDisposables.ForEach(d => d.Dispose());
            _cameraDisposables.Clear();
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
                        _camera.SetAnnotateSettings();
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
                _camera.CleanPortPools();
            }
        }

        List<IDownstreamComponent> PopulateProcessingList()
        {
            var list = new List<IDownstreamComponent>();
            var initialStillDownstream = _camera.StillPort.ConnectedReference?.DownstreamComponent;
            var initialVideoDownstream = _camera.VideoPort.ConnectedReference?.DownstreamComponent;
            var initialPreviewDownstream = _camera.PreviewPort.ConnectedReference?.DownstreamComponent;

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
