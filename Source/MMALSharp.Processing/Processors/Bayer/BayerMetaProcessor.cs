using System;
using System.Text;
using MMALSharp.Common;

namespace MMALSharp.Processing.Processors.Bayer
{
    public class BayerMetaProcessor : IFrameProcessor
    {
        public const int BayerMetaLengthV1 = 6404096;
        public const int BayerMetaLengthV2 = 10270208;

        public CameraVersion CameraVersion { get; }

        public BayerMetaProcessor(CameraVersion camVersion)
        {
            CameraVersion = camVersion;
        }

        public void Apply(ImageContext context)
        {
            byte[] array;

            switch (CameraVersion)
            {
                case CameraVersion.Ov5647:
                    array = new byte[BayerMetaLengthV1];
                    Array.Copy(context.Data, context.Data.Length - BayerMetaLengthV1, array, 0, BayerMetaLengthV1);
                    break;
                case CameraVersion.Imx219:
                    array = new byte[BayerMetaLengthV2];
                    Array.Copy(context.Data, context.Data.Length - BayerMetaLengthV2, array, 0, BayerMetaLengthV2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Camera {CameraVersion} is unknown");
            }

            var meta = new byte[4];
            Array.Copy(array, 0, meta, 0, 4);

            if (Encoding.ASCII.GetString(meta) != "BRCM")
                throw new Exception("Could not find Bayer metadata in header");

            context.Data = new byte[array.Length];
            Array.Copy(array, context.Data, array.Length);
        }
    }
}
