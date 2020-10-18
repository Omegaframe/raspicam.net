using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using MMALSharp.Common;
using MMALSharp.Common.Utility;
using MMALSharp.Extensions;
using MMALSharp.Native.Parameters;
using MMALSharp.Native.Util;
using MMALSharp.Ports;
using MMALSharp.Ports.Outputs;
using MMALSharp.Processing.Handlers;

namespace MMALSharp.Components
{
    public enum MmalSensorMode
    {
        Mode0,
        Mode1,
        Mode2,
        Mode3,
        Mode4,
        Mode5,
        Mode6,
        Mode7
    }

    public sealed class MmalCameraComponent : MmalComponentBase, ICameraComponent
    {
        public const int MmalCameraPreviewPort = 0;
        public const int MmalCameraVideoPort = 1;
        public const int MmalCameraStillPort = 2;

        public IOutputPort PreviewPort { get; set; }
        public IOutputPort VideoPort { get; set; }
        public IOutputPort StillPort { get; set; }
        public ICameraInfoComponent CameraInfo { get; set; }

        public unsafe MmalCameraComponent() : base(MmalParameters.MmalComponentDefaultCamera)
        {
            Outputs.Add(new OutputPort((IntPtr)(&(*Ptr->Output[0])), this, Guid.NewGuid()));
            Outputs.Add(new VideoPort((IntPtr)(&(*Ptr->Output[1])), this, Guid.NewGuid()));
            Outputs.Add(new StillPort((IntPtr)(&(*Ptr->Output[2])), this, Guid.NewGuid()));

            if (CameraInfo == null)
                SetSensorDefaults();

            PreviewPort = Outputs[MmalCameraPreviewPort];
            VideoPort = Outputs[MmalCameraVideoPort];
            StillPort = Outputs[MmalCameraStillPort];

            PreviewPort.SetStereoMode(MmalCameraConfig.StereoMode);
            VideoPort.SetStereoMode(MmalCameraConfig.StereoMode);
            StillPort.SetStereoMode(MmalCameraConfig.StereoMode);

            Control.SetParameter(MmalParametersCamera.MmalParameterCameraNum, 0);

            var eventRequest = new MmalParameterChangeEventRequestType(
                new MmalParameterHeaderType(MmalParametersCommon.MmalParameterChangeEventRequest, Marshal.SizeOf<MmalParameterChangeEventRequestType>()),
                MmalParametersCamera.MmalParameterCameraSettings, 1);

            if (MmalCameraConfig.SetChangeEventRequest)
                Control.SetChangeEventRequest(eventRequest);
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

        public void Initialise(ICaptureHandler stillCaptureHandler = null, ICaptureHandler videoCaptureHandler = null)
        {
            DisableComponent();

            var camConfig = new MmalParameterCameraConfigType(
                new MmalParameterHeaderType(MmalParametersCamera.MmalParameterCameraConfig, Marshal.SizeOf<MmalParameterCameraConfigType>()),
                                                                CameraInfo.MaxWidth,
                                                                CameraInfo.MaxHeight,
                                                                0,
                                                                1,
                                                                MmalCameraConfig.Resolution.Width,
                                                                MmalCameraConfig.Resolution.Height,
                                                                3 + Math.Max(0, (new MmalRational(MmalCameraConfig.Framerate).Num - 30) / 10),
                                                                0,
                                                                0,
                                                                MmalCameraConfig.ClockMode);

            this.SetCameraConfig(camConfig);

            MmalLog.Logger.LogDebug("Camera config set");

            Control.Start();

            MmalLog.Logger.LogDebug("Configuring camera parameters.");

            SetCameraParameters();

            InitialisePreview();
            InitialiseVideo(videoCaptureHandler);
            InitialiseStill(stillCaptureHandler);

            EnableComponent();

            MmalLog.Logger.LogDebug("Camera component configured.");
        }

        void SetSensorDefaults()
        {
            CameraInfo = new MmalCameraInfoComponent();
        }

        void InitialisePreview()
        {
            var portConfig = new MmalPortConfig(
                MmalCameraConfig.Encoding,
                MmalCameraConfig.EncodingSubFormat,
                width: MmalCameraConfig.Resolution.Width,
                height: MmalCameraConfig.Resolution.Height,
                framerate: MmalCameraConfig.Framerate);

            MmalLog.Logger.LogDebug("Commit preview");

            PreviewPort.Configure(portConfig, null, null);

            // Use Raspistill values.
            if (MmalCameraConfig.ShutterSpeed > 6000000)
                PreviewPort.SetFramerateRange(new MmalRational(5, 1000), new MmalRational(166, 1000));
            else if (MmalCameraConfig.ShutterSpeed > 1000000)
                PreviewPort.SetFramerateRange(new MmalRational(166, 1000), new MmalRational(999, 1000));
        }

        void InitialiseVideo(ICaptureHandler handler)
        {
            var currentWidth = MmalCameraConfig.Resolution.Width;
            var currentHeight = MmalCameraConfig.Resolution.Height;

            if (currentWidth == 0 || currentWidth > CameraInfo.MaxWidth)
                currentWidth = CameraInfo.MaxWidth;

            if (currentHeight == 0 || currentHeight > CameraInfo.MaxHeight)
                currentHeight = CameraInfo.MaxHeight;

            var portConfig = new MmalPortConfig(
                MmalCameraConfig.Encoding,
                MmalCameraConfig.EncodingSubFormat,
                width: currentWidth,
                height: currentHeight,
                framerate: MmalCameraConfig.Framerate,
                bufferNum: Math.Max(VideoPort.BufferNumRecommended, 3),
                bufferSize: Math.Max(VideoPort.BufferSizeRecommended, VideoPort.BufferSizeMin),
                crop: new Rectangle(0, 0, currentWidth, currentHeight));

            MmalLog.Logger.LogDebug("Commit video");

            VideoPort.Configure(portConfig, null, handler);

            // Use Raspistill values.
            if (MmalCameraConfig.ShutterSpeed > 6000000)
                VideoPort.SetFramerateRange(new MmalRational(5, 1000), new MmalRational(166, 1000));
            else if (MmalCameraConfig.ShutterSpeed > 1000000)
                VideoPort.SetFramerateRange(new MmalRational(167, 1000), new MmalRational(999, 1000));
        }

        void InitialiseStill(ICaptureHandler handler)
        {
            var currentWidth = MmalCameraConfig.Resolution.Width;
            var currentHeight = MmalCameraConfig.Resolution.Height;

            if (currentWidth == 0 || currentWidth > CameraInfo.MaxWidth)            
                currentWidth = CameraInfo.MaxWidth;            

            if (currentHeight == 0 || currentHeight > CameraInfo.MaxHeight)            
                currentHeight = CameraInfo.MaxHeight;            

            MmalCameraConfig.Resolution = new Resolution(currentWidth, currentHeight);

            MmalPortConfig portConfig;

            if (MmalCameraConfig.Encoding == MmalEncoding.Rgb32 ||
                MmalCameraConfig.Encoding == MmalEncoding.Rgb24 ||
                MmalCameraConfig.Encoding == MmalEncoding.Rgb16)
            {
                MmalLog.Logger.LogWarning("Encoding set to RGB. Setting width padding to multiple of 16.");

                var encoding = MmalCameraConfig.Encoding;

                try
                {
                    if (!StillPort.RgbOrderFixed())
                    {
                        MmalLog.Logger.LogWarning("Using old firmware. Setting encoding to BGR24");
                        encoding = MmalEncoding.Bgr24;
                    }
                }
                catch
                {
                    MmalLog.Logger.LogWarning("Using old firmware. Setting encoding to BGR24");
                    encoding = MmalEncoding.Bgr24;
                }

                portConfig = new MmalPortConfig(
                    encoding,
                    encoding,
                    width: currentWidth,
                    height: currentHeight,
                    framerate: MmalCameraConfig.Framerate,
                    bufferNum: Math.Max(StillPort.BufferNumRecommended, 3),
                    bufferSize: Math.Max(StillPort.BufferSizeRecommended, StillPort.BufferSizeMin),
                    crop: new Rectangle(0, 0, currentWidth, currentHeight));
            }
            else
            {
                var resolution = MmalCameraConfig.Resolution.Pad();

                portConfig = new MmalPortConfig(
                    MmalCameraConfig.Encoding,
                    MmalCameraConfig.EncodingSubFormat,
                    width: resolution.Width,
                    height: resolution.Height,
                    framerate: MmalCameraConfig.Framerate,
                    bufferNum: Math.Max(StillPort.BufferNumRecommended, 3),
                    bufferSize: Math.Max(StillPort.BufferSizeRecommended, StillPort.BufferSizeMin),
                    crop: new Rectangle(0, 0, currentWidth, currentHeight));
            }

            MmalLog.Logger.LogDebug("Commit still");
            StillPort.Configure(portConfig, null, handler);

            // Use Raspistill values.
            if (MmalCameraConfig.ShutterSpeed > 6000000)            
                StillPort.SetFramerateRange(new MmalRational(5, 1000), new MmalRational(166, 1000));            
            else if (MmalCameraConfig.ShutterSpeed > 1000000)            
                StillPort.SetFramerateRange(new MmalRational(167, 1000), new MmalRational(999, 1000));            
        }

        void SetCameraParameters()
        {
            this.SetSensorMode();
            this.SetSaturation(MmalCameraConfig.Saturation);
            this.SetSharpness(MmalCameraConfig.Sharpness);
            this.SetContrast(MmalCameraConfig.Contrast);
            this.SetBrightness(MmalCameraConfig.Brightness);
            this.SetIso(MmalCameraConfig.Iso);
            this.SetVideoStabilisation(MmalCameraConfig.VideoStabilisation);
            this.SetExposureCompensation(MmalCameraConfig.ExposureCompensation);
            this.SetExposureMode(MmalCameraConfig.ExposureMode);
            this.SetExposureMeteringMode(MmalCameraConfig.ExposureMeterMode);
            this.SetAwbMode(MmalCameraConfig.AwbMode);
            this.SetAwbGains(MmalCameraConfig.AwbGainsR, MmalCameraConfig.AwbGainsB);
            this.SetImageFx(MmalCameraConfig.ImageFx);
            this.SetColourFx(MmalCameraConfig.ColorFx);
            this.SetRotation(MmalCameraConfig.Rotation);
            this.SetShutterSpeed(MmalCameraConfig.ShutterSpeed);
            this.SetStatsPass(MmalCameraConfig.StatsPass);
            this.SetDrc(MmalCameraConfig.DrcLevel);
            this.SetFlips(MmalCameraConfig.Flips);
            this.SetZoom(MmalCameraConfig.Roi);
            this.SetBurstMode(MmalCameraConfig.StillBurstMode);
            this.SetAnalogGain(MmalCameraConfig.AnalogGain);
            this.SetDigitalGain(MmalCameraConfig.DigitalGain);
        }
    }
}
