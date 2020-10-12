using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.Logging;
using MMALSharp.Common.Utility;
using MMALSharp.Components.EncoderComponents;
using MMALSharp.Config;
using MMALSharp.Native;
using MMALSharp.Ports;
using MMALSharp.Ports.Inputs;
using MMALSharp.Ports.Outputs;
using MMALSharp.Extensions;
using MMALSharp.Processing.Handlers;
using static MMALSharp.MmalNativeExceptionHelper;

namespace MMALSharp.Components
{
    /// <summary>
    /// A conformant image encode component, which takes raw pixels on its
    /// input port, and encodes the image into various compressed formats on
    /// the output port.
    /// https://github.com/raspberrypi/firmware/blob/master/documentation/ilcomponents/image_encode.html
    /// </summary>
    public unsafe class MMALImageEncoder : MMALEncoderBase, IImageEncoder
    {
        /// <summary>
        /// Represents the maximum length of a formatted EXIF tag. This includes the tag's key, an equals sign, the tag's value and a null char.
        /// </summary>
        public const int MaxExifPayloadLength = 128;

        /// <summary>
        /// When enabled, raw bayer metadata will be included in JPEG still captures.
        /// </summary>
        public bool RawBayer { get; }

        /// <summary>
        /// When enabled, EXIF metadata will be included in image stills.
        /// </summary>
        public bool UseExif { get; }

        /// <summary>
        /// Custom list of user defined EXIF metadata.
        /// </summary>
        public ExifTag[] ExifTags { get; }

        /// <summary>
        /// If true, this component will be configured to process rapidly captured frames from the camera's video port.
        /// Note: The component pipeline must be configured as such. 
        /// </summary>
        public bool ContinuousCapture { get; }

        /// <summary>
        /// Configuration for JPEG thumbnail embedding.
        /// </summary>
        public JpegThumbnail JpegThumbnailConfig { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="MMALImageEncoder"/> class with the specified handler.
        /// </summary>
        /// <param name="rawBayer">Specifies whether to include raw bayer image data.</param>
        /// <param name="useExif">Specifies whether any EXIF tags should be used.</param>
        /// <param name="continuousCapture">Configure component for rapid capture mode.</param>
        /// <param name="thumbnailConfig">Configures the embedded JPEG thumbnail.</param>
        /// <param name="exifTags">A collection of custom EXIF tags.</param>
        public MMALImageEncoder(bool rawBayer = false, bool useExif = true, bool continuousCapture = false, JpegThumbnail thumbnailConfig = null, params ExifTag[] exifTags) : base(MMALParameters.MMAL_COMPONENT_DEFAULT_IMAGE_ENCODER)
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

        /// <inheritdoc />
        public override IDownstreamComponent ConfigureOutputPort(int outputPort, IMMALPortConfig config, IOutputCaptureHandler handler)
        {
            base.ConfigureOutputPort(outputPort, config, handler);

            if (RawBayer)
                MalCamera.Instance.Camera.StillPort.SetRawCapture(true);

            if (UseExif)
                AddExifTags(ExifTags);

            if (JpegThumbnailConfig != null)
            {
                var str = new MMAL_PARAMETER_THUMBNAIL_CONFIG_T(
                    new MMAL_PARAMETER_HEADER_T(
                        MMALParametersCamera.MMAL_PARAMETER_THUMBNAIL_CONFIGURATION,
                        Marshal.SizeOf<MMAL_PARAMETER_THUMBNAIL_CONFIG_T>()),
                        JpegThumbnailConfig.Enable, JpegThumbnailConfig.Width,
                        JpegThumbnailConfig.Height, JpegThumbnailConfig.Quality);

                MmalCheck(MMALPort.mmal_port_parameter_set(Control.Ptr, &str.Hdr), "Unable to set JPEG thumbnail config.");
            }

            return this;
        }

        /// <summary>
        /// Adds EXIF tags to the resulting image.
        /// </summary>
        /// <param name="exifTags">A list of user defined EXIF tags.</param>
        void AddExifTags(params ExifTag[] exifTags)
        {
            // Add the same defaults as per Raspistill.c
            string sensorName = string.Empty;

            try
            {
                sensorName = MalCamera.Instance.Camera.CameraInfo.SensorName;
            }
            catch
            {
                MmalLog.Logger.LogWarning("Attempt to retrieve sensor name failed.");
            }

            List<ExifTag> defaultTags = new List<ExifTag>
            {
                new ExifTag { Key = "IFD0.Model", Value = "RP_" + sensorName },
                new ExifTag { Key = "IFD0.Make", Value = "RaspberryPi" },
                new ExifTag { Key = "EXIF.DateTimeDigitized", Value = DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss") },
                new ExifTag { Key = "EXIF.DateTimeOriginal", Value = DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss") },
                new ExifTag { Key = "IFD0.DateTime", Value = DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss") }
            };

            this.SetDisableExif(false);

            defaultTags.ForEach(c => AddExifTag(c));

            if ((defaultTags.Count + exifTags.Length) > 32)            
                throw new PiCameraError("Maximum number of EXIF tags exceeded.");

            // Add user defined tags.
            foreach (ExifTag tag in exifTags)
                AddExifTag(tag);            
        }

        /// <summary>
        /// Provides a facility to add an EXIF tag to the image. 
        /// </summary>
        /// <param name="exifTag">The EXIF tag to add to.</param>
        void AddExifTag(ExifTag exifTag)
        {
            var formattedExif = exifTag.Key + "=" + exifTag.Value + char.MinValue;

            if (formattedExif.Length > MaxExifPayloadLength)            
                throw new PiCameraError("EXIF payload greater than allowed max.");            

            var arr = new byte[128];

            var bytes = Encoding.ASCII.GetBytes(formattedExif);

            Array.Copy(bytes, arr, bytes.Length);

            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf<MMAL_PARAMETER_EXIF_T>() + (arr.Length - 1));

            var str = new MMAL_PARAMETER_EXIF_T(
                new MMAL_PARAMETER_HEADER_T(
                    MMALParametersCamera.MMAL_PARAMETER_EXIF,
                Marshal.SizeOf<MMAL_PARAMETER_EXIF_T_DUMMY>() + (arr.Length - 1)), 0, 0, 0, arr);

            Marshal.StructureToPtr(str, ptr, false);

            try
            {
                MmalCheck(MMALPort.mmal_port_parameter_set(Outputs[0].Ptr, (MMAL_PARAMETER_HEADER_T*)ptr),
                    $"Unable to set EXIF {formattedExif}");
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}
