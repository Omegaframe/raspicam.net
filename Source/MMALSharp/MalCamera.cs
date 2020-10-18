using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MMALSharp.Common.Utility;
using MMALSharp.Components;
using MMALSharp.Extensions;
using MMALSharp.Native;
using MMALSharp.Ports.Outputs;
using MMALSharp.Processing.Handlers;

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

        public MalCamera ConfigureCameraSettings(ICaptureHandler stillCaptureHandler = null, ICaptureHandler videoCaptureHandler = null)
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
