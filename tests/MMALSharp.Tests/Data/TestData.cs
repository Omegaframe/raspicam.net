// <copyright file="TestData.cs" company="Techyian">
// Copyright (c) Ian Auty and contributors. All rights reserved.
// Licensed under the MIT License. Please see LICENSE.txt for License info.
// </copyright>

using MMALSharp.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using MMALSharp.Common;
using Xunit;
using MMALSharp.Extensions;

namespace MMALSharp.Tests
{
    [Collection("MMALCollection")]
    public class TestBase
    {
        private static MMALFixture _fixture;
        public static MMALFixture Fixture
        {
            get
            {
                if (_fixture == null)
                {
                    _fixture = new MMALFixture();
                }

                return _fixture;
            }
            set => _fixture = value;
        }
        
        public TestBase(MMALFixture fixture)
        {
            Fixture = fixture;
        }
        
        public static List<MmalEncoding> PixelFormats = MmalEncodingHelpers.EncodingList.Where(c => c.EncType == MmalEncoding.EncodingType.PixelFormat).ToList();

        private static IEnumerable<object[]> GetVideoEncoderData(MmalEncoding encodingType, string extension)
        {
            var supportedEncodings = Fixture.MMALCamera.Camera.VideoPort.GetSupportedEncodings();
            return PixelFormats.Where(c => supportedEncodings.Contains(c.EncodingVal) && c != MmalEncoding.Opaque).Select(pixFormat => new object[] { extension, encodingType, pixFormat }).ToList();
        }

        private static IEnumerable<object[]> GetImageEncoderData(MmalEncoding encodingType, string extension)
        {
            var supportedEncodings = Fixture.MMALCamera.Camera.StillPort.GetSupportedEncodings();
            return PixelFormats.Where(c => supportedEncodings.Contains(c.EncodingVal) && c != MmalEncoding.Opaque).Select(pixFormat => new object[] { extension, encodingType, pixFormat }).ToList();
        }
        
        private static object[] GetEncoderData(MmalEncoding encodingType, MmalEncoding pixelFormat, string extension)
        {
            var supportedEncodings = Fixture.MMALCamera.Camera.StillPort.GetSupportedEncodings();

            if (!supportedEncodings.Contains(pixelFormat.EncodingVal))
            {
                throw new ArgumentException("Unsupported pixel format requested.");
            }

            return new object[] { extension, encodingType, pixelFormat };
        } 
        
        #region Still image encoders

        public static IEnumerable<object[]> JpegEncoderData => GetImageEncoderData(MmalEncoding.Jpeg, "jpg");
        
        public static IEnumerable<object[]> GifEncoderData => GetImageEncoderData(MmalEncoding.Gif, "gif");
        
        public static IEnumerable<object[]> PngEncoderData => GetImageEncoderData(MmalEncoding.Png, "png");
        
        public static IEnumerable<object[]> PpmEncoderData => GetImageEncoderData(MmalEncoding.Ppm, "ppm");
        
        public static IEnumerable<object[]> TgaEncoderData => GetImageEncoderData(MmalEncoding.Tga, "tga");
        
        public static IEnumerable<object[]> BmpEncoderData => GetImageEncoderData(MmalEncoding.Bmp, "bmp");
        
        #endregion

        #region Video encoders

        public static IEnumerable<object[]> H264EncoderData => GetVideoEncoderData(MmalEncoding.H264, "avi");

        public static IEnumerable<object[]> MvcEncoderData => GetVideoEncoderData(MmalEncoding.Mvc, "mvc");

        public static IEnumerable<object[]> H263EncoderData => GetVideoEncoderData(MmalEncoding.H263, "h263");

        public static IEnumerable<object[]> Mp4EncoderData => GetVideoEncoderData(MmalEncoding.Mp4V, "mp4");

        public static IEnumerable<object[]> Mp2EncoderData => GetVideoEncoderData(MmalEncoding.Mp2V, "mp2");

        public static IEnumerable<object[]> Mp1EncoderData => GetVideoEncoderData(MmalEncoding.Mp1V, "mp1");

        public static IEnumerable<object[]> Wmv3EncoderData => GetVideoEncoderData(MmalEncoding.Wmv3, "wmv");

        public static IEnumerable<object[]> Wmv2EncoderData => GetVideoEncoderData(MmalEncoding.Wmv2, "wmv");

        public static IEnumerable<object[]> Wmv1EncoderData => GetVideoEncoderData(MmalEncoding.Wmv1, "wmv");

        public static IEnumerable<object[]> Wvc1EncoderData => GetVideoEncoderData(MmalEncoding.Wvc1, "asf");

        public static IEnumerable<object[]> Vp8EncoderData => GetVideoEncoderData(MmalEncoding.Vp8, "webm");

        public static IEnumerable<object[]> Vp7EncoderData => GetVideoEncoderData(MmalEncoding.Vp7, "webm");

        public static IEnumerable<object[]> Vp6EncoderData => GetVideoEncoderData(MmalEncoding.Vp6, "webm");

        public static IEnumerable<object[]> TheoraEncoderData => GetVideoEncoderData(MmalEncoding.Theora, "ogv");

        public static IEnumerable<object[]> SparkEncoderData => GetVideoEncoderData(MmalEncoding.Spark, "flv");

        public static IEnumerable<object[]> MjpegEncoderData => GetVideoEncoderData(MmalEncoding.MJpeg, "mjpeg");

        #endregion

        #region IsRaw image encode

        public static object[] Yuv420EncoderData => GetEncoderData(MmalEncoding.I420, MmalEncoding.I420, "i420");
        public static object[] Yuv422EncoderData => GetEncoderData(MmalEncoding.I422, MmalEncoding.I422, "i422");
        public static object[] Rgb16EncoderData => GetEncoderData(MmalEncoding.Rgb16, MmalEncoding.Rgb16, "rgb16");
        public static object[] Rgb24EncoderData => GetEncoderData(MmalEncoding.Rgb24, MmalEncoding.Rgb24, "rgb24");
        public static object[] RgbaEncoderData => GetEncoderData(MmalEncoding.Rgba, MmalEncoding.Rgba, "rgba");
        
        #endregion

    }
}
