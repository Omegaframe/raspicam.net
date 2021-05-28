using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Raspicam.Net.Config;
using Raspicam.Net.Extensions;
using Raspicam.Net.Mmal.Ports;
using Raspicam.Net.Mmal.Ports.Outputs;
using Raspicam.Net.Native.Parameters;
using Raspicam.Net.Native.Util;
using Raspicam.Net.Utility;

namespace Raspicam.Net.Mmal.Components
{
    sealed class MmalCameraComponent : MmalComponentBase, ICameraComponent
    {
        const int MmalCameraPreviewPort = 0;
        const int MmalCameraVideoPort = 1;
        const int MmalCameraStillPort = 2;

        public IOutputPort PreviewPort { get; }
        public IOutputPort VideoPort { get; }
        public IOutputPort StillPort { get; }
        public ICameraInfoComponent CameraInfo { get; }

        public unsafe MmalCameraComponent() : base(MmalParameters.MmalComponentDefaultCamera)
        {
            Outputs.Add(new OutputPort((IntPtr)(&(*Ptr->Output[0])), this, Guid.NewGuid()));
            Outputs.Add(new VideoPort((IntPtr)(&(*Ptr->Output[1])), this, Guid.NewGuid()));
            Outputs.Add(new StillPort((IntPtr)(&(*Ptr->Output[2])), this, Guid.NewGuid()));

            CameraInfo = new MmalCameraInfoComponent();

            PreviewPort = Outputs[MmalCameraPreviewPort];
            VideoPort = Outputs[MmalCameraVideoPort];
            StillPort = Outputs[MmalCameraStillPort];

            PreviewPort.SetStereoMode(CameraConfig.StereoMode);
            VideoPort.SetStereoMode(CameraConfig.StereoMode);
            StillPort.SetStereoMode(CameraConfig.StereoMode);

            Control.SetParameter(MmalParametersCamera.MmalParameterCameraNum, 0);
        }

        public override void Dispose()
        {
            if (CameraInfo != null && CameraInfo.CheckState())
                CameraInfo.DestroyComponent();

            base.Dispose();
        }

        public override void PrintComponent()
        {
            base.PrintComponent();
            MmalLog.Logger.LogInformation($"    Still Width: {StillPort.Resolution.Width}. Video Height: {StillPort.Resolution.Height}");
            MmalLog.Logger.LogInformation($"    Video Width: {VideoPort.Resolution.Width}. Video Height: {VideoPort.Resolution.Height}");
            MmalLog.Logger.LogInformation($"    Max Width: {CameraInfo.MaxWidth}. Video Height: {CameraInfo.MaxHeight}");
        }

        public void Initialise()
        {
            DisableComponent();

            var camConfig = new MmalParameterCameraConfigType(
                new MmalParameterHeaderType(MmalParametersCamera.MmalParameterCameraConfig, Marshal.SizeOf<MmalParameterCameraConfigType>()),
                                                                CameraInfo.MaxWidth,
                                                                CameraInfo.MaxHeight,
                                                                0,
                                                                1,
                                                                CameraConfig.Resolution.Width,
                                                                CameraConfig.Resolution.Height,
                                                                3 + Math.Max(0, (new MmalRational(CameraConfig.Framerate).Num - 30) / 10),
                                                                0,
                                                                0,
                                                                CameraConfig.ClockMode);

            this.SetCameraConfig(camConfig);

            MmalLog.Logger.LogDebug("Camera config set");

            Control.Start();

            MmalLog.Logger.LogDebug("Configuring camera parameters.");

            SetCameraParameters();

            InitialisePreview();
            InitialiseVideo();
            InitialiseStill();

            EnableComponent();

            MmalLog.Logger.LogDebug("Camera component configured.");
        }

        void InitialisePreview()
        {
            var portConfig = new MmalPortConfig(
                CameraConfig.Encoding,
                CameraConfig.EncodingSubFormat,
                width: CameraConfig.Resolution.Width,
                height: CameraConfig.Resolution.Height,
                framerate: CameraConfig.Framerate);

            MmalLog.Logger.LogDebug("Commit preview");

            PreviewPort.Configure(portConfig, null, null);

            // Use Raspistill values.
            if (CameraConfig.ShutterSpeed > 6000000)
                PreviewPort.SetFramerateRange(new MmalRational(5, 1000), new MmalRational(166, 1000));
            else if (CameraConfig.ShutterSpeed > 1000000)
                PreviewPort.SetFramerateRange(new MmalRational(166, 1000), new MmalRational(999, 1000));
        }

        void InitialiseVideo()
        {
            var currentWidth = CameraConfig.Resolution.Width;
            var currentHeight = CameraConfig.Resolution.Height;

            if (currentWidth == 0 || currentWidth > CameraInfo.MaxWidth)
                currentWidth = CameraInfo.MaxWidth;

            if (currentHeight == 0 || currentHeight > CameraInfo.MaxHeight)
                currentHeight = CameraInfo.MaxHeight;

            var portConfig = new MmalPortConfig(
                CameraConfig.Encoding,
                CameraConfig.EncodingSubFormat,
                width: currentWidth,
                height: currentHeight,
                framerate: CameraConfig.Framerate,
                bufferNum: Math.Max(VideoPort.BufferNumRecommended, 3),
                bufferSize: Math.Max(VideoPort.BufferSizeRecommended, VideoPort.BufferSizeMin),
                crop: new Rectangle(0, 0, currentWidth, currentHeight));

            MmalLog.Logger.LogDebug("Commit video");

            VideoPort.Configure(portConfig, null, null);

            // Use Raspistill values.
            if (CameraConfig.ShutterSpeed > 6000000)
                VideoPort.SetFramerateRange(new MmalRational(5, 1000), new MmalRational(166, 1000));
            else if (CameraConfig.ShutterSpeed > 1000000)
                VideoPort.SetFramerateRange(new MmalRational(167, 1000), new MmalRational(999, 1000));
        }

        void InitialiseStill()
        {
            var currentWidth = CameraConfig.Resolution.Width;
            var currentHeight = CameraConfig.Resolution.Height;

            if (currentWidth == 0 || currentWidth > CameraInfo.MaxWidth)
                currentWidth = CameraInfo.MaxWidth;

            if (currentHeight == 0 || currentHeight > CameraInfo.MaxHeight)
                currentHeight = CameraInfo.MaxHeight;

            CameraConfig.Resolution = new Resolution(currentWidth, currentHeight);

            var resolution = CameraConfig.Resolution.Pad();

            var portConfig = new MmalPortConfig(
                CameraConfig.Encoding,
                CameraConfig.EncodingSubFormat,
                width: resolution.Width,
                height: resolution.Height,
                framerate: CameraConfig.Framerate,
                bufferNum: Math.Max(StillPort.BufferNumRecommended, 3),
                bufferSize: Math.Max(StillPort.BufferSizeRecommended, StillPort.BufferSizeMin),
                crop: new Rectangle(0, 0, currentWidth, currentHeight));

            MmalLog.Logger.LogDebug("Commit still");
            StillPort.Configure(portConfig, null, null);

            // Use Raspistill values.
            if (CameraConfig.ShutterSpeed > 6000000)
                StillPort.SetFramerateRange(new MmalRational(5, 1000), new MmalRational(166, 1000));
            else if (CameraConfig.ShutterSpeed > 1000000)
                StillPort.SetFramerateRange(new MmalRational(167, 1000), new MmalRational(999, 1000));
        }

        void SetCameraParameters()
        {
            this.SetSensorMode();
            this.SetSaturation(CameraConfig.Saturation);
            this.SetSharpness(CameraConfig.Sharpness);
            this.SetContrast(CameraConfig.Contrast);
            this.SetBrightness(CameraConfig.Brightness);
            this.SetIso(CameraConfig.Iso);
            this.SetVideoStabilisation(CameraConfig.VideoStabilisation);
            this.SetExposureCompensation(CameraConfig.ExposureCompensation);
            this.SetExposureMode(CameraConfig.ExposureMode);
            this.SetExposureMeteringMode(CameraConfig.ExposureMeterMode);
            this.SetAwbMode(CameraConfig.AwbMode);
            this.SetAwbGains(CameraConfig.AwbGainsR, CameraConfig.AwbGainsB);
            this.SetImageFx(CameraConfig.ImageFx);
            this.SetColourFx(CameraConfig.ColorFx);
            this.SetRotation(CameraConfig.Rotation);
            this.SetShutterSpeed(CameraConfig.ShutterSpeed);
            this.SetStatsPass(CameraConfig.StatsPass);
            this.SetDrc(CameraConfig.DrcLevel);
            this.SetFlips(CameraConfig.Flips);
            this.SetZoom(CameraConfig.Roi);
            this.SetBurstMode(CameraConfig.StillBurstMode);
            this.SetAnalogGain(CameraConfig.AnalogGain);
            this.SetDigitalGain(CameraConfig.DigitalGain);
        }
    }
}
