using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MMALSharp.Common.Utility;
using MMALSharp.Components;
using MMALSharp.Native;

namespace MMALSharp
{
    public class MmalStandalone
    {
        public static MmalStandalone Instance => Lazy.Value;

        static readonly Lazy<MmalStandalone> Lazy = new Lazy<MmalStandalone>(() => new MmalStandalone());

        MmalStandalone()
        {
            BcmHost.Initialize();
        }

        public async Task ProcessAsync(IDownstreamComponent initialComponent, CancellationToken cancellationToken = default)
        {
            var handlerComponents = PopulateProcessingList(initialComponent);

            initialComponent.Control.Start();
            initialComponent.Inputs[0].Start();

            var tasks = new List<Task>
            {
                initialComponent.Inputs[0].Trigger.Task
            };

            // Enable all connections associated with these components
            foreach (var component in handlerComponents)
            {
                component.EnableConnections();
                component.ForceStopProcessing = false;

                foreach (var port in component.ProcessingPorts.Values)
                {
                    if (port.ConnectedReference != null)
                        continue;

                    port.Start();
                    tasks.Add(port.Trigger.Task);
                }
            }

            // Get buffer from input port pool                
            var inputBuffer = initialComponent.Inputs[0].BufferPool.Queue.GetBuffer();

            if (inputBuffer.CheckState())
                initialComponent.Inputs[0].SendBuffer(inputBuffer);

            if (cancellationToken == CancellationToken.None)
            {
                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            else
            {
                await Task.WhenAny(Task.WhenAll(tasks), Task.Delay(-1, cancellationToken)).ConfigureAwait(false);

                foreach (var component in handlerComponents)
                    component.ForceStopProcessing = true;

                await Task.WhenAll(tasks).ConfigureAwait(false);
            }

            // Cleanup each downstream component.
            foreach (var component in handlerComponents)
            {
                foreach (var port in component.ProcessingPorts.Values.Where(p => p.ConnectedReference == null))
                    port.DisablePort();

                component.CleanPortPools();
                component.DisableConnections();
            }
        }

        public void PrintPipeline(IDownstreamComponent initialComponent)
        {
            MmalLog.Logger.LogInformation("Current pipeline:");
            MmalLog.Logger.LogInformation(string.Empty);

            foreach (var component in PopulateProcessingList(initialComponent))
                component.PrintComponent();
        }

        public void Cleanup()
        {
            MmalLog.Logger.LogDebug("Destroying final components");

            var tempList = new List<MmalDownstreamComponent>(MmalBootstrapper.DownstreamComponents);

            tempList.ForEach(c => c.Dispose());

            BcmHost.Uninitialize();
        }

        List<IDownstreamComponent> PopulateProcessingList(IDownstreamComponent initialComponent)
        {
            var list = new List<IDownstreamComponent>();

            if (initialComponent != null)
                FindComponents(initialComponent, list);

            return list;
        }

        static void FindComponents(IDownstreamComponent downstream, List<IDownstreamComponent> list)
        {
            switch (downstream.Outputs.Count)
            {
                case 0:
                    return;
                case 1 when downstream.Outputs[0].ConnectedReference == null:
                    list.Add(downstream);
                    return;
            }

            if (downstream is IDownstreamHandlerComponent component)
                list.Add(component);

            foreach (var output in downstream.Outputs.Where(o => o.ConnectedReference != null))
                FindComponents(output.ConnectedReference.DownstreamComponent, list);
        }
    }
}