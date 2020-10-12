// <copyright file="ConfigurationTests.cs" company="Techyian">
// Copyright (c) Ian Auty and contributors. All rights reserved.
// Licensed under the MIT License. Please see LICENSE.txt for License info.
// </copyright>

using System;
using MMALSharp.Common.Utility;
using MMALSharp.Components;
using MMALSharp.Config;
using MMALSharp.Extensions;
using MMALSharp.Native;
using MMALSharp.Utility;
using Xunit;

namespace MMALSharp.Tests
{
    public class ConfigurationTests : TestBase
    {
        public ConfigurationTests(MMALFixture fixture)
            : base(fixture)
        {
        }

        [Theory]
        [InlineData(MMALSensorMode.Mode0)]
        [InlineData(MMALSensorMode.Mode2)]
        [InlineData(MMALSensorMode.Mode4)]
        [MMALTestsAttribute]
        public void SetThenGetSensorMode(MMALSensorMode mode)
        {
            MmalCameraConfig.SensorMode = mode;

            Fixture.MalCamera.ConfigureCameraSettings();
            Assert.True(Fixture.MalCamera.Camera.GetSensorMode() == mode);
        }

        [Theory]
        [InlineData(40)]
        [InlineData(45)]
        [InlineData(-100)]
        [MMALTestsAttribute]
        public void SetThenGetBrightness(int brightness)
        {
            MmalCameraConfig.Brightness = brightness;

            if (brightness >= 0 && brightness <= 100)
            {
                Fixture.MalCamera.ConfigureCameraSettings();

                Assert.True(Fixture.MalCamera.Camera.GetBrightness() == brightness / 100);
            }
            else
            {
                Assert.ThrowsAny<Exception>(() => Fixture.MalCamera.ConfigureCameraSettings());
            }
        }

        [Theory]
        [InlineData(20)]
        [InlineData(38)]
        [InlineData(101)]
        [MMALTestsAttribute]
        public void SetThenGetSharpness(int sharpness)
        { 
            MmalCameraConfig.Sharpness = sharpness;

            if (sharpness >= -100 && sharpness <= 100)
            {
                Fixture.MalCamera.ConfigureCameraSettings();

                Assert.True(Fixture.MalCamera.Camera.GetSharpness() == sharpness / 100);
            }
            else
            {
                Assert.ThrowsAny<Exception>(() => Fixture.MalCamera.ConfigureCameraSettings());
            }
        }

        [Theory]
        [InlineData(10)]
        [InlineData(54)]
        [InlineData(-200)]
        [MMALTestsAttribute]
        public void SetThenGetContrast(int contrast)
        {  
            MmalCameraConfig.Contrast = contrast;

            if (contrast >= -100 && contrast <= 100)
            {
                Fixture.MalCamera.ConfigureCameraSettings();

                Assert.True(Fixture.MalCamera.Camera.GetContrast() == contrast / 100);
            }
            else
            {
                Assert.ThrowsAny<Exception>(() => Fixture.MalCamera.ConfigureCameraSettings());
            }
        }

        [Theory]
        [InlineData(30)]
        [InlineData(55)]
        [InlineData(90)]
        [MMALTestsAttribute]
        public void SetThenGetSaturation(int saturation)
        {    
            MmalCameraConfig.Saturation = saturation;
            Fixture.MalCamera.ConfigureCameraSettings();

            Assert.True(Fixture.MalCamera.Camera.GetSaturation() == saturation / 100);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(900)]
        [InlineData(0)]
        [MMALTestsAttribute]
        public void SetThenGetIso(int iso)
        {  
            MmalCameraConfig.Iso = iso;

            if ((iso >= 100 && iso <= 800) || iso == 0)
            {
                Fixture.MalCamera.ConfigureCameraSettings();

                Assert.True(Fixture.MalCamera.Camera.GetISO() == iso);
            }
            else
            {
                Assert.ThrowsAny<Exception>(() => Fixture.MalCamera.ConfigureCameraSettings());
            }
        }

        [Theory]
        [InlineData(5)]
        [InlineData(50)]
        [InlineData(-30)]
        [MMALTestsAttribute]
        public void SetThenGetExposureCompensation(int expCompensation)
        {   
            MmalCameraConfig.ExposureCompensation = expCompensation;

            if (expCompensation >= -10 && expCompensation <= 10)
            {
                Fixture.MalCamera.ConfigureCameraSettings();
                Assert.True(Fixture.MalCamera.Camera.GetExposureCompensation() == expCompensation);
            }
            else
            {
                Assert.ThrowsAny<Exception>(() => Fixture.MalCamera.ConfigureCameraSettings());
            }
        }

        [Theory]
        [InlineData(MMAL_PARAM_EXPOSUREMODE_T.MMAL_PARAM_EXPOSUREMODE_BEACH)]
        [InlineData(MMAL_PARAM_EXPOSUREMODE_T.MMAL_PARAM_EXPOSUREMODE_FIREWORKS)]
        [InlineData(MMAL_PARAM_EXPOSUREMODE_T.MMAL_PARAM_EXPOSUREMODE_ANTISHAKE)]
        [MMALTestsAttribute]
        public void SetThenGetExposureMode(MMAL_PARAM_EXPOSUREMODE_T expMode)
        {    
            MmalCameraConfig.ExposureMode = expMode;
            Fixture.MalCamera.ConfigureCameraSettings();

            Assert.True(Fixture.MalCamera.Camera.GetExposureMode() == expMode);
        }

        [Theory]
        [InlineData(MMAL_PARAM_EXPOSUREMETERINGMODE_T.MMAL_PARAM_EXPOSUREMETERINGMODE_BACKLIT)]
        [InlineData(MMAL_PARAM_EXPOSUREMETERINGMODE_T.MMAL_PARAM_EXPOSUREMETERINGMODE_MATRIX)]
        [InlineData(MMAL_PARAM_EXPOSUREMETERINGMODE_T.MMAL_PARAM_EXPOSUREMETERINGMODE_AVERAGE)]
        [MMALTestsAttribute]
        public void SetThenGetExposureMeteringMode(MMAL_PARAM_EXPOSUREMETERINGMODE_T expMetMode)
        {     
            MmalCameraConfig.ExposureMeterMode = expMetMode;
            Fixture.MalCamera.ConfigureCameraSettings();

            Assert.True(Fixture.MalCamera.Camera.GetExposureMeteringMode() == expMetMode);
        }

        [Theory]
        [InlineData(MMAL_PARAM_AWBMODE_T.MMAL_PARAM_AWBMODE_AUTO)]
        [InlineData(MMAL_PARAM_AWBMODE_T.MMAL_PARAM_AWBMODE_FLUORESCENT)]
        [InlineData(MMAL_PARAM_AWBMODE_T.MMAL_PARAM_AWBMODE_CLOUDY)]
        [MMALTestsAttribute]
        public void SetThenGetAwbMode(MMAL_PARAM_AWBMODE_T awbMode)
        {    
            MmalCameraConfig.AwbMode = awbMode;
            Fixture.MalCamera.ConfigureCameraSettings();

            Assert.True(Fixture.MalCamera.Camera.GetAwbMode() == awbMode);
        }

        [Theory]
        [InlineData(MMAL_PARAM_IMAGEFX_T.MMAL_PARAM_IMAGEFX_CARTOON)]
        [InlineData(MMAL_PARAM_IMAGEFX_T.MMAL_PARAM_IMAGEFX_COLOURBALANCE)]
        [InlineData(MMAL_PARAM_IMAGEFX_T.MMAL_PARAM_IMAGEFX_OILPAINT)]
        [MMALTestsAttribute]
        public void SetThenGetImageFx(MMAL_PARAM_IMAGEFX_T imgFx)
        {      
            MmalCameraConfig.ImageFx = imgFx;
            Fixture.MalCamera.ConfigureCameraSettings();
            Assert.True(Fixture.MalCamera.Camera.GetImageFx() == imgFx);
        }

        [Theory]
        [InlineData(true, 128, 128)]
        [InlineData(true, 50, 100)]
        [InlineData(false, 128, 128)]
        [MMALTestsAttribute]
        public void SetThenGetColourFx(bool enable, byte u, byte v)
        {        
            var color = MmalColor.FromYuvBytes(0, u, v);

            var colFx = new ColourEffects(enable, color);

            MmalCameraConfig.ColourFx = colFx;
            Fixture.MalCamera.ConfigureCameraSettings();

            var uv = MmalColor.RgbToYuvBytes(Fixture.MalCamera.Camera.GetColourFx().Color);
            
            Assert.True(Fixture.MalCamera.Camera.GetColourFx().Enable == enable &&
                        uv.Item2 == u &&
                        uv.Item3 == v);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(90, 90)]
        [InlineData(140, 90)]
        [InlineData(250, 180)]
        [InlineData(270, 270)]
        [MMALTestsAttribute]
        public void SetThenGetRotation(int rotation, int expectedResult)
        {     
            MmalCameraConfig.Rotation = rotation;
            Fixture.MalCamera.ConfigureCameraSettings();

            Assert.True(Fixture.MalCamera.Camera.GetRotation() == expectedResult);
        }

        [Theory]
        [InlineData(MMAL_PARAM_MIRROR_T.MMAL_PARAM_MIRROR_HORIZONTAL)]
        [InlineData(MMAL_PARAM_MIRROR_T.MMAL_PARAM_MIRROR_BOTH)]
        [InlineData(MMAL_PARAM_MIRROR_T.MMAL_PARAM_MIRROR_VERTICAL)]
        [MMALTestsAttribute]
        public void SetThenGetFlips(MMAL_PARAM_MIRROR_T flips)
        {     
            MmalCameraConfig.Flips = flips;
            Fixture.MalCamera.ConfigureCameraSettings();
            Assert.True(Fixture.MalCamera.Camera.GetFlips() == flips &&
                        Fixture.MalCamera.Camera.GetStillFlips() == flips &&
                        Fixture.MalCamera.Camera.GetVideoFlips() == flips);
        }

        [Theory]
        [InlineData(0.1, 0.1, 0.5, 1.0)]
        [InlineData(0.5, 0.1, 0.5, 1.0)]
        [InlineData(0.1, 1.1, 0.5, 1.0)]
        [MMALTestsAttribute]
        public void SetThenGetZoom(double x, double y, double width, double height)
        {      
            var zoom = new Zoom(x, y, width, height);

            MmalCameraConfig.Roi = zoom;

            if (x <= 1.0 && y <= 1.0 && height <= 1.0 && width <= 1.0)
            {
                Fixture.MalCamera.ConfigureCameraSettings();

                Assert.True(Fixture.MalCamera.Camera.GetZoom().Height == Convert.ToInt32(height * 65536) &&
                            Fixture.MalCamera.Camera.GetZoom().Width == Convert.ToInt32(width * 65536) &&
                            Fixture.MalCamera.Camera.GetZoom().X == Convert.ToInt32(x * 65536) &&
                            Fixture.MalCamera.Camera.GetZoom().Y == Convert.ToInt32(y * 65536));
            }
            else
            {
                Assert.ThrowsAny<Exception>(() => Fixture.MalCamera.ConfigureCameraSettings());
            }
        }

        [Theory]
        [InlineData(2500)]
        [MMALTestsAttribute]
        public void SetThenGetShutterSpeed(int shutterSpeed)
        {   
            MmalCameraConfig.Framerate = 0;
            MmalCameraConfig.SensorMode = MMALSensorMode.Mode1;
            MmalCameraConfig.AwbMode = MMAL_PARAM_AWBMODE_T.MMAL_PARAM_AWBMODE_OFF;
            MmalCameraConfig.ShutterSpeed = shutterSpeed;
            Fixture.MalCamera.ConfigureCameraSettings();

            Assert.True(Fixture.MalCamera.Camera.GetShutterSpeed() == shutterSpeed);
        }

        [Theory]
        [InlineData(MMAL_PARAMETER_DRC_STRENGTH_T.MMAL_PARAMETER_DRC_STRENGTH_HIGH)]
        [InlineData(MMAL_PARAMETER_DRC_STRENGTH_T.MMAL_PARAMETER_DRC_STRENGTH_LOW)]
        [InlineData(MMAL_PARAMETER_DRC_STRENGTH_T.MMAL_PARAMETER_DRC_STRENGTH_MEDIUM)]
        [MMALTestsAttribute]
        public void SetThenGetDrc(MMAL_PARAMETER_DRC_STRENGTH_T drc)
        { 
            MmalCameraConfig.DrcLevel = drc;
            Fixture.MalCamera.ConfigureCameraSettings();
            Assert.True(Fixture.MalCamera.Camera.GetDRC() == drc);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [MMALTestsAttribute]
        public void SetThenGetStatsPass(bool statsPass)
        {    
            MmalCameraConfig.StatsPass = statsPass;
            Fixture.MalCamera.ConfigureCameraSettings();
            Assert.True(Fixture.MalCamera.Camera.GetStatsPass() == statsPass);
        }

        [Theory]
        [InlineData(1500000)]
        [InlineData(6500000)]        
        [MMALTestsAttribute]
        public void SetThenGetFramerateRange(int shutterSpeed)
        {
            MmalCameraConfig.ShutterSpeed = shutterSpeed;

            Fixture.MalCamera.ConfigureCameraSettings();

            var previewRange = Fixture.MalCamera.Camera.PreviewPort.GetFramerateRange();
            var videoRange = Fixture.MalCamera.Camera.VideoPort.GetFramerateRange();
            var stillRange = Fixture.MalCamera.Camera.StillPort.GetFramerateRange();

            if (shutterSpeed > 6000000)
            {
                Assert.True(Math.Round(((double)previewRange.FpsLow.Num / previewRange.FpsLow.Den) * 1000) == 50);
                Assert.True(Math.Round(((double)previewRange.FpsHigh.Num / previewRange.FpsHigh.Den) * 1000) == 166);

                Assert.True(Math.Round(((double)videoRange.FpsLow.Num / videoRange.FpsLow.Den) * 1000) == 50);
                Assert.True(Math.Round(((double)videoRange.FpsHigh.Num / videoRange.FpsHigh.Den) * 1000) == 166);

                Assert.True(Math.Round(((double)stillRange.FpsLow.Num / stillRange.FpsLow.Den) * 1000) == 50);
                Assert.True(Math.Round(((double)stillRange.FpsHigh.Num / stillRange.FpsHigh.Den) * 1000) == 166);
            }
            else if (shutterSpeed > 1000000)
            {
                Assert.True(Math.Round(((double)previewRange.FpsLow.Num / previewRange.FpsLow.Den) * 1000) == 166);
                Assert.True(Math.Round(((double)previewRange.FpsHigh.Num / previewRange.FpsHigh.Den) * 1000) == 999);

                Assert.True(Math.Round(((double)videoRange.FpsLow.Num / videoRange.FpsLow.Den) * 1000) == 167);
                Assert.True(Math.Round(((double)videoRange.FpsHigh.Num / videoRange.FpsHigh.Den) * 1000) == 999);

                Assert.True(Math.Round(((double)stillRange.FpsLow.Num / stillRange.FpsLow.Den) * 1000) == 167);
                Assert.True(Math.Round(((double)stillRange.FpsHigh.Num / stillRange.FpsHigh.Den) * 1000) == 999);
            }            
        }

        [Theory]
        [InlineData(2)]
        [InlineData(7)]
        [InlineData(9)]
        [InlineData(0.5)]
        [MMALTests]
        public void SetThenGetAnalogGain(double analogGain)
        {
            MmalCameraConfig.AnalogGain = analogGain;
            
            if (analogGain >= 1.0 && analogGain <= 8.0)
            {
                Fixture.MalCamera.ConfigureCameraSettings();
                Assert.True(Fixture.MalCamera.Camera.GetAnalogGain() == analogGain);
            }
            else
            {
                Assert.ThrowsAny<Exception>(() => Fixture.MalCamera.ConfigureCameraSettings());
            }
        }

        [Theory]
        [InlineData(255)]
        [InlineData(1)]
        [InlineData(256)]
        [InlineData(0.5)]
        [MMALTests]
        public void SetThenGetDigitalGain(double digitalGain)
        {
            MmalCameraConfig.DigitalGain = digitalGain;
            
            if (digitalGain >= 1.0 && digitalGain <= 255.0)
            {
                Fixture.MalCamera.ConfigureCameraSettings();
                Assert.True(Fixture.MalCamera.Camera.GetDigitalGain() == digitalGain);
            }
            else
            {
                Assert.ThrowsAny<Exception>(() => Fixture.MalCamera.ConfigureCameraSettings());
            }
        }

        [Theory]
        [InlineData(25)]
        [InlineData(25.5)]
        [InlineData(0.005)]
        [MMALTests]
        public void SetThenGetFramerate(double framerate)
        {
            MmalCameraConfig.Framerate = framerate;

            Fixture.MalCamera.ConfigureCameraSettings();

            Assert.True((double)Fixture.MalCamera.Camera.StillPort.FrameRateRational.Num / Fixture.MalCamera.Camera.StillPort.FrameRateRational.Den == framerate);
        }
    }
}
