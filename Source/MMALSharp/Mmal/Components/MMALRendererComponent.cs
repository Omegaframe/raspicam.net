using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using MMALSharp.Config;
using MMALSharp.Mmal.Ports;
using MMALSharp.Mmal.Ports.Inputs;
using MMALSharp.Native.Parameters;
using MMALSharp.Native.Port;
using MMALSharp.Native.Util;
using MMALSharp.Utility;
using static MMALSharp.MmalNativeExceptionHelper;

namespace MMALSharp.Mmal.Components
{
    abstract class MmalRendererBase : MmalDownstreamComponent
    {
        protected unsafe MmalRendererBase(string name) : base(name)
        {
            Inputs.Add(new InputPort((IntPtr)(&(*Ptr->Input[0])), this, Guid.NewGuid()));
        }
    }

    class MmalNullSinkComponent : MmalRendererBase
    {
        public MmalNullSinkComponent() : base(MmalParameters.MmalComponentDefaultNullSink)
        {
            EnableComponent();
        }

        public override void PrintComponent()
        {
            MmalLog.Logger.LogInformation($"Component: Null sink renderer");
        }
    }

    class MmalVideoRenderer : MmalRendererBase
    {
        public PreviewConfiguration Configuration { get; }
        public List<MmalOverlayRenderer> Overlays { get; } = new List<MmalOverlayRenderer>();

        public MmalVideoRenderer() : base(MmalParameters.MmalComponentDefaultVideoRenderer)
        {
            EnableComponent();
        }

        public MmalVideoRenderer(PreviewConfiguration config) : base(MmalParameters.MmalComponentDefaultVideoRenderer)
        {
            EnableComponent();
            Configuration = config;
        }

        public void RemoveOverlay(MmalOverlayRenderer renderer)
        {
            Overlays.Remove(renderer);
            renderer.Dispose();
        }

        public unsafe void ConfigureRenderer()
        {
            if (Configuration == null)
                return;

            int fullScreen = 0, noAspect = 0, copyProtect = 0;

            MmalRect? previewWindow = default(MmalRect);

            if (!Configuration.FullScreen)
            {
                previewWindow = new MmalRect(
                    Configuration.PreviewWindow.X, Configuration.PreviewWindow.Y,
                    Configuration.PreviewWindow.Width, Configuration.PreviewWindow.Height);
            }

            uint displaySet = (int)MmalParametersVideo.MmalDisplaysetT.MmalDisplaySetLayer;
            displaySet |= (int)MmalParametersVideo.MmalDisplaysetT.MmalDisplaySetAlpha;

            if (Configuration.FullScreen)
            {
                displaySet |= (int)MmalParametersVideo.MmalDisplaysetT.MmalDisplaySetFullscreen;
                fullScreen = 1;
            }
            else
            {
                displaySet |= (int)MmalParametersVideo.MmalDisplaysetT.MmalDisplaySetDestRect |
                              (int)MmalParametersVideo.MmalDisplaysetT.MmalDisplaySetFullscreen;
            }

            if (Configuration.NoAspect)
            {
                displaySet |= (int)MmalParametersVideo.MmalDisplaysetT.MmalDisplaySetNoAspect;
                noAspect = 1;
            }

            if (Configuration.CopyProtect)
            {
                displaySet |= (int)MmalParametersVideo.MmalDisplaysetT.MmalDisplaySetCopyProtect;
                copyProtect = 1;
            }

            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<MmalDisplayRegionType>());

            var displayRegion = new MmalDisplayRegionType(
                new MmalParameterHeaderType(
                    MmalParametersVideo.MmalParameterDisplayregion,
                    Marshal.SizeOf<MmalDisplayRegionType>()), displaySet, 0, fullScreen, Configuration.DisplayTransform, (MmalRect) previewWindow, new MmalRect(0, 0, 0, 0), noAspect,
                    Configuration.DisplayMode, 0, 0, Configuration.Layer, copyProtect, Configuration.Opacity);

            Marshal.StructureToPtr(displayRegion, ptr, false);

            try
            {
                MmalCheck(MmalPort.SetParameter(Inputs[0].Ptr, (MmalParameterHeaderType*)ptr), "Unable to set preview renderer configuration");
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public override void PrintComponent()
        {
            MmalLog.Logger.LogInformation("Component: Video renderer");
        }

        public override void Dispose()
        {
            if (GetType() == typeof(MmalVideoRenderer))
                Overlays.ForEach(c => c.Dispose());

            base.Dispose();
        }
    }

    sealed class MmalOverlayRenderer : MmalVideoRenderer
    {
        public byte[] Source { get; }
        public MmalVideoRenderer ParentRenderer { get; }
        public PreviewOverlayConfiguration OverlayConfiguration { get; }

        public readonly IReadOnlyCollection<MmalEncoding> AllowedEncodings = new ReadOnlyCollection<MmalEncoding>(new List<MmalEncoding>
        {
            MmalEncoding.I420,
            MmalEncoding.Rgb24,
            MmalEncoding.Rgba,
            MmalEncoding.Bgr24,
            MmalEncoding.BGra
        });

        public MmalOverlayRenderer(MmalVideoRenderer parent, PreviewOverlayConfiguration config, byte[] source) : base(config)
        {
            Source = source;
            ParentRenderer = parent;
            OverlayConfiguration = config;
            parent.Overlays.Add(this);

            if (config == null)
                return;

            int width;
            int height;

            if (config.Resolution.Width > 0 && config.Resolution.Height > 0)
            {
                width = config.Resolution.Width;
                height = config.Resolution.Height;
            }
            else
            {
                width = parent.Inputs[0].Resolution.Width;
                height = parent.Inputs[0].Resolution.Height;
            }

            if (config.Encoding == null)
            {
                var sourceLength = source.Length;
                var planeSize = Inputs[0].Resolution.Pad();
                var planeLength = Math.Floor((double)planeSize.Width * planeSize.Height);

                if (Math.Floor(sourceLength / planeLength) == 3)
                    config.Encoding = MmalEncoding.Rgb24;
                else if (Math.Floor(sourceLength / planeLength) == 4)
                    config.Encoding = MmalEncoding.Rgba;
                else
                    throw new PiCameraError("Unable to determine encoding from image size.");
            }

            if (AllowedEncodings.All(c => c.EncodingVal != Inputs[0].NativeEncodingType))
                throw new PiCameraError($"Incompatible encoding type for use with Preview Render overlay {Inputs[0].NativeEncodingType.ParseEncoding().EncodingName}.");

            var portConfig = new MmalPortConfig(
                config.Encoding,
                null,
                width: width,
                height: height);

            Control.Start();
            Inputs[0].Start();
        }

        public void UpdateOverlay()
        {
            UpdateOverlay(Source);
        }

        public void UpdateOverlay(byte[] imageData)
        {
            var buffer = Inputs[0].BufferPool.Queue.GetBuffer();

            if (buffer == null)
            {
                MmalLog.Logger.LogWarning("Received null buffer when updating overlay.");
                return;
            }

            buffer.ReadIntoBuffer(imageData, imageData.Length, false);
            Inputs[0].SendBuffer(buffer);
        }
    }
}