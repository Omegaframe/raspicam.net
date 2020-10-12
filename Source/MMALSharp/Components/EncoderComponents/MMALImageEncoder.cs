using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.Logging;
using MMALSharp.Common.Utility;
using MMALSharp.Config;
using MMALSharp.Extensions;
using MMALSharp.Native;
using MMALSharp.Native.Parameters;
using MMALSharp.Native.Port;
using MMALSharp.Ports;
using MMALSharp.Ports.Inputs;
using MMALSharp.Ports.Outputs;
using MMALSharp.Processing.Handlers;
using static MMALSharp.MmalNativeExceptionHelper;

namespace MMALSharp.Components.EncoderComponents
{
    public unsafe class MmalImageEncoder : MmalEncoderBase, IImageEncoder
    {
        public const int MaxExifPayloadLength = 128;

        public bool RawBayer { get; }
        public bool UseExif { get; }
        public ExifTag[] ExifTags { get; }
        public bool ContinuousCapture { get; }
        public JpegThumbnail JpegThumbnailConfig { get; set; }

        public MmalImageEncoder(bool rawBayer = false, bool useExif = true, bool continuousCapture = false, JpegThumbnail thumbnailConfig = null, params ExifTag[] exifTags) : base(MmalParameters.MmalComponentDefaultImageEncoder)
        {
            RawBayer = rawBayer;
            UseExif = useExif;
            ExifTags = exifTags;
            ContinuousCapture = continuousCapture;
            JpegThumbnailConfig = thumbnailConfig;

            Inputs.Add(new InputPort((IntPtr)(&(*Ptr->Input[0])), this, Guid.NewGuid()));

            if (ContinuousCapture)
                Outputs.Add(new FastStillPort((IntPtr)(&(*Ptr->Output[0])), this, Guid.NewGuid()));
            else
                Outputs.Add(new StillPort((IntPtr)(&(*Ptr->Output[0])), this, Guid.NewGuid()));
        }

        public override IDownstreamComponent ConfigureOutputPort(int outputPort, IMmalPortConfig config, IOutputCaptureHandler handler)
        {
            base.ConfigureOutputPort(outputPort, config, handler);

            if (RawBayer)
                MalCamera.Instance.Camera.StillPort.SetRawCapture(true);

            if (UseExif)
                AddExifTags(ExifTags);

            if (JpegThumbnailConfig == null) 
                return this;

            var str = new MmalParameterThumbnailConfigType(
                new MmalParameterHeaderType(
                    MmalParametersCamera.MmalParameterThumbnailConfiguration,
                    Marshal.SizeOf<MmalParameterThumbnailConfigType>()),
                JpegThumbnailConfig.Enable, JpegThumbnailConfig.Width,
                JpegThumbnailConfig.Height, JpegThumbnailConfig.Quality);

            MmalCheck(MmalPort.SetParameter(Control.Ptr, &str.Hdr), "Unable to set JPEG thumbnail config.");

            return this;
        }

        void AddExifTags(params ExifTag[] exifTags)
        {
            // Add the same defaults as per Raspistill.c
            var sensorName = string.Empty;

            try
            {
                sensorName = MalCamera.Instance.Camera.CameraInfo.SensorName;
            }
            catch
            {
                MmalLog.Logger.LogWarning("Attempt to retrieve sensor name failed.");
            }

            var defaultTags = new List<ExifTag>
            {
                new ExifTag { Key = "IFD0.Model", Value = "RP_" + sensorName },
                new ExifTag { Key = "IFD0.Make", Value = "RaspberryPi" },
                new ExifTag { Key = "EXIF.DateTimeDigitized", Value = DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss") },
                new ExifTag { Key = "EXIF.DateTimeOriginal", Value = DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss") },
                new ExifTag { Key = "IFD0.DateTime", Value = DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss") }
            };

            this.SetDisableExif(false);

            defaultTags.ForEach(AddExifTag);

            if ((defaultTags.Count + exifTags.Length) > 32)            
                throw new PiCameraError("Maximum number of EXIF tags exceeded.");

            // Add user defined tags.
            foreach (var tag in exifTags)
                AddExifTag(tag);            
        }

        void AddExifTag(ExifTag exifTag)
        {
            var formattedExif = exifTag.Key + "=" + exifTag.Value + char.MinValue;

            if (formattedExif.Length > MaxExifPayloadLength)            
                throw new PiCameraError("EXIF payload greater than allowed max.");            

            var arr = new byte[128];

            var bytes = Encoding.ASCII.GetBytes(formattedExif);

            Array.Copy(bytes, arr, bytes.Length);

            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<MmalParameterExifType>() + (arr.Length - 1));

            var str = new MmalParameterExifType(
                new MmalParameterHeaderType(
                    MmalParametersCamera.MmalParameterExif,
                Marshal.SizeOf<MmalParameterExifTDummy>() + (arr.Length - 1)), 0, 0, 0, arr);

            Marshal.StructureToPtr(str, ptr, false);

            try
            {
                MmalCheck(MmalPort.SetParameter(Outputs[0].Ptr, (MmalParameterHeaderType*)ptr),
                    $"Unable to set EXIF {formattedExif}");
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}
