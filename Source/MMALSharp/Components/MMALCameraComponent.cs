using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using MMALSharp.Common;
using MMALSharp.Common.Utility;
using MMALSharp.Extensions;
using MMALSharp.Native;
using MMALSharp.Ports;
using MMALSharp.Ports.Outputs;
using MMALSharp.Processing.Handlers;

namespace MMALSharp.Components
{
    /// <summary>
    /// Defines a set of sensor modes that allow to configure how the raw image data is sent to the GPU before further processing. See wiki on GitHub for more information.
    /// </summary>
    /// <remarks>
    /// https://github.com/techyian/MMALSharp/wiki/OmniVision-OV5647-Camera-Module
    /// https://github.com/techyian/MMALSharp/wiki/Sony-IMX219-Camera-Module
    /// https://www.raspberrypi.org/forums/viewtopic.php?t=85714
    /// </remarks>
    public enum MMALSensorMode
    {
        /// <summary>
        /// Automatic mode (default).
        /// </summary>
        Mode0,

        /// <summary>
        /// 1080p cropped mode.
        /// </summary>
        Mode1,

        /// <summary>
        /// 4:3 ratio.
        /// </summary>
        Mode2,

        /// <summary>
        /// 4:3 ratio (low FPS with OV5647).
        /// </summary>
        Mode3,

        /// <summary>
        /// 2x2 binned 4:3.
        /// </summary>
        Mode4,

        /// <summary>
        /// 2x2 binned 16:9.
        /// </summary>
        Mode5,

        /// <summary>
        /// High FPS. Ratio and resolution depend on camera module.
        /// </summary>
        Mode6,

        /// <summary>
        /// VGA high FPS.
        /// </summary>
        Mode7
    }

    /// <summary>
    /// Represents a camera component.
    /// </summary>
    public sealed class MMALCameraComponent : MMALComponentBase, ICameraComponent
    {
        /// <summary>
        /// The output port number of the camera's preview port.
        /// </summary>
        public const int MMALCameraPreviewPort = 0;

        /// <summary>
        /// The output port number of the camera's video port.
        /// </summary>
        public const int MMALCameraVideoPort = 1;

        /// <summary>
        /// The output port number of the camera's still port.
        /// </summary>
        public const int MMALCameraStillPort = 2;

        /// <summary>
        /// Managed reference to the Preview port of the camera.
        /// </summary>
        public IOutputPort PreviewPort { get; set; }

        /// <summary>
        /// Managed reference to the Video port of the camera.
        /// </summary>
        public IOutputPort VideoPort { get; set; }

        /// <summary>
        /// Managed reference to the Still port of the camera.
        /// </summary>
        public IOutputPort StillPort { get; set; }

        /// <summary>
        /// Camera Info component. This is used to provide detailed info about the camera itself.
        /// </summary>
        public ICameraInfoComponent CameraInfo { get; set; }

        /// <summary>
        /// Initialises a new MMALCameraComponent.
        /// </summary>
        public unsafe MMALCameraComponent() : base(MMALParameters.MMAL_COMPONENT_DEFAULT_CAMERA)
        {
            Outputs.Add(new OutputPort((IntPtr)(&(*Ptr->Output[0])), this, Guid.NewGuid()));
            Outputs.Add(new VideoPort((IntPtr)(&(*Ptr->Output[1])), this, Guid.NewGuid()));
            Outputs.Add(new StillPort((IntPtr)(&(*Ptr->Output[2])), this, Guid.NewGuid()));

            if (CameraInfo == null)
                SetSensorDefaults();

            PreviewPort = Outputs[MMALCameraPreviewPort];
            VideoPort = Outputs[MMALCameraVideoPort];
            StillPort = Outputs[MMALCameraStillPort];

            /*
             * Stereoscopic mode is only supported with the compute module as it requires two camera modules to work.
             * I have added the code in for consistency with Raspistill, however this project currently only supports one camera module
             * and therefore will not work if enabled.
             * See: https://www.raspberrypi.org/forums/viewtopic.php?p=600720
            */
            PreviewPort.SetStereoMode(MmalCameraConfig.StereoMode);
            VideoPort.SetStereoMode(MmalCameraConfig.StereoMode);
            StillPort.SetStereoMode(MmalCameraConfig.StereoMode);

            Control.SetParameter(MMALParametersCamera.MMAL_PARAMETER_CAMERA_NUM, 0);

            var eventRequest = new MMAL_PARAMETER_CHANGE_EVENT_REQUEST_T(
                new MMAL_PARAMETER_HEADER_T(MMALParametersCommon.MMAL_PARAMETER_CHANGE_EVENT_REQUEST, Marshal.SizeOf<MMAL_PARAMETER_CHANGE_EVENT_REQUEST_T>()),
                MMALParametersCamera.MMAL_PARAMETER_CAMERA_SETTINGS, 1);

            if (MmalCameraConfig.SetChangeEventRequest)
                Control.SetChangeEventRequest(eventRequest);
        }

        /// <summary>
        /// Disposes of the current component, and frees any native resources still in use by it.
        /// </summary>
        public override void Dispose()
        {
            if (CameraInfo != null && CameraInfo.CheckState())
                CameraInfo.DestroyComponent();

            base.Dispose();
        }

        /// <summary>
        /// Prints a summary of the ports and the resolution associated with this component to the console.
        /// </summary>
        public override void PrintComponent()
        {
            base.PrintComponent();
            MmalLog.Logger.LogInformation($"    Still Width: {StillPort.Resolution.Width}. Video Height: {StillPort.Resolution.Height}");
            MmalLog.Logger.LogInformation($"    Video Width: {VideoPort.Resolution.Width}. Video Height: {VideoPort.Resolution.Height}");
            MmalLog.Logger.LogInformation($"    Max Width: {CameraInfo.MaxWidth}. Video Height: {CameraInfo.MaxHeight}");
        }

        /// <summary>
        /// Call to initialise the camera component.
        /// </summary>
        /// <param name="stillCaptureHandler">A capture handler when capturing raw image frames from the camera's still port (no encoder attached).</param>
        /// <param name="videoCaptureHandler">A capture handler when capturing raw video from the camera's video port (no encoder attached).</param>
        public void Initialise(IOutputCaptureHandler stillCaptureHandler = null, IOutputCaptureHandler videoCaptureHandler = null)
        {
            DisableComponent();

            var camConfig = new MMAL_PARAMETER_CAMERA_CONFIG_T(
                new MMAL_PARAMETER_HEADER_T(MMALParametersCamera.MMAL_PARAMETER_CAMERA_CONFIG, Marshal.SizeOf<MMAL_PARAMETER_CAMERA_CONFIG_T>()),
                                                                CameraInfo.MaxWidth,
                                                                CameraInfo.MaxHeight,
                                                                0,
                                                                1,
                                                                MmalCameraConfig.Resolution.Width,
                                                                MmalCameraConfig.Resolution.Height,
                                                                3 + Math.Max(0, (new MMAL_RATIONAL_T(MmalCameraConfig.Framerate).Num - 30) / 10),
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
            CameraInfo = new MMALCameraInfoComponent();
        }

        /// <summary>
        /// Initialises the camera's preview component using the user defined width/height for the video port.
        /// </summary>
        void InitialisePreview()
        {
            var portConfig = new MMALPortConfig(
                MmalCameraConfig.Encoding,
                MmalCameraConfig.EncodingSubFormat,
                width: MmalCameraConfig.Resolution.Width,
                height: MmalCameraConfig.Resolution.Height,
                framerate: MmalCameraConfig.Framerate);

            MmalLog.Logger.LogDebug("Commit preview");

            PreviewPort.Configure(portConfig, null, null);

            // Use Raspistill values.
            if (MmalCameraConfig.ShutterSpeed > 6000000)
                PreviewPort.SetFramerateRange(new MMAL_RATIONAL_T(5, 1000), new MMAL_RATIONAL_T(166, 1000));
            else if (MmalCameraConfig.ShutterSpeed > 1000000)
                PreviewPort.SetFramerateRange(new MMAL_RATIONAL_T(166, 1000), new MMAL_RATIONAL_T(999, 1000));
        }

        /// <summary>
        /// Initialises the camera's video port using the width, height and encoding as specified by the user.
        /// </summary>
        /// <param name="handler">The capture handler to associate with this port.</param>
        private void InitialiseVideo(IOutputCaptureHandler handler)
        {
            int currentWidth = MmalCameraConfig.Resolution.Width;
            int currentHeight = MmalCameraConfig.Resolution.Height;

            if (currentWidth == 0 || currentWidth > CameraInfo.MaxWidth)
                currentWidth = CameraInfo.MaxWidth;

            if (currentHeight == 0 || currentHeight > CameraInfo.MaxHeight)
                currentHeight = CameraInfo.MaxHeight;

            var portConfig = new MMALPortConfig(
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
                VideoPort.SetFramerateRange(new MMAL_RATIONAL_T(5, 1000), new MMAL_RATIONAL_T(166, 1000));
            else if (MmalCameraConfig.ShutterSpeed > 1000000)
                VideoPort.SetFramerateRange(new MMAL_RATIONAL_T(167, 1000), new MMAL_RATIONAL_T(999, 1000));
        }

        /// <summary>
        /// Initialises the camera's still port using the width, height and encoding as specified by the user.
        /// </summary>
        /// <param name="handler">The capture handler to associate with the still port.</param>
        private void InitialiseStill(IOutputCaptureHandler handler)
        {
            int currentWidth = MmalCameraConfig.Resolution.Width;
            int currentHeight = MmalCameraConfig.Resolution.Height;

            if (currentWidth == 0 || currentWidth > CameraInfo.MaxWidth)            
                currentWidth = CameraInfo.MaxWidth;            

            if (currentHeight == 0 || currentHeight > CameraInfo.MaxHeight)            
                currentHeight = CameraInfo.MaxHeight;            

            MmalCameraConfig.Resolution = new Resolution(currentWidth, currentHeight);

            MMALPortConfig portConfig;

            if (MmalCameraConfig.Encoding == MmalEncoding.Rgb32 ||
                MmalCameraConfig.Encoding == MmalEncoding.Rgb24 ||
                MmalCameraConfig.Encoding == MmalEncoding.Rgb16)
            {
                MmalLog.Logger.LogWarning("Encoding set to RGB. Setting width padding to multiple of 16.");

                var resolution = MmalCameraConfig.Resolution.Pad(16, 16);
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

                portConfig = new MMALPortConfig(
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

                portConfig = new MMALPortConfig(
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
                StillPort.SetFramerateRange(new MMAL_RATIONAL_T(5, 1000), new MMAL_RATIONAL_T(166, 1000));            
            else if (MmalCameraConfig.ShutterSpeed > 1000000)            
                StillPort.SetFramerateRange(new MMAL_RATIONAL_T(167, 1000), new MMAL_RATIONAL_T(999, 1000));            
        }

        void SetCameraParameters()
        {
            this.SetSensorMode(MmalCameraConfig.SensorMode);
            this.SetSaturation(MmalCameraConfig.Saturation);
            this.SetSharpness(MmalCameraConfig.Sharpness);
            this.SetContrast(MmalCameraConfig.Contrast);
            this.SetBrightness(MmalCameraConfig.Brightness);
            this.SetISO(MmalCameraConfig.Iso);
            this.SetVideoStabilisation(MmalCameraConfig.VideoStabilisation);
            this.SetExposureCompensation(MmalCameraConfig.ExposureCompensation);
            this.SetExposureMode(MmalCameraConfig.ExposureMode);
            this.SetExposureMeteringMode(MmalCameraConfig.ExposureMeterMode);
            this.SetAwbMode(MmalCameraConfig.AwbMode);
            this.SetAwbGains(MmalCameraConfig.AwbGainsR, MmalCameraConfig.AwbGainsB);
            this.SetImageFx(MmalCameraConfig.ImageFx);
            this.SetColourFx(MmalCameraConfig.ColourFx);
            this.SetRotation(MmalCameraConfig.Rotation);
            this.SetShutterSpeed(MmalCameraConfig.ShutterSpeed);
            this.SetStatsPass(MmalCameraConfig.StatsPass);
            this.SetDRC(MmalCameraConfig.DrcLevel);
            this.SetFlips(MmalCameraConfig.Flips);
            this.SetZoom(MmalCameraConfig.Roi);
            this.SetBurstMode(MmalCameraConfig.StillBurstMode);
            this.SetAnalogGain(MmalCameraConfig.AnalogGain);
            this.SetDigitalGain(MmalCameraConfig.DigitalGain);
        }
    }
}
