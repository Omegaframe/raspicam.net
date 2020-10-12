using System;
using MMALSharp.Common;

namespace MMALSharp.Processing.Processors.Effects
{
    public enum GaussianMatrix
    {
        Matrix3x3,
        Matrix5x5
    }

    public class GaussianProcessor : ConvolutionBase, IFrameProcessor
    {
        readonly int _kernelWidth;
        readonly int _kernelHeight;

        double[,] Kernel { get; }

        public GaussianProcessor(GaussianMatrix matrix)
        {
            switch (matrix)
            {
                case GaussianMatrix.Matrix3x3:
                    _kernelWidth = 3;
                    _kernelHeight = 3;
                    Kernel = new[,]
                    {
                        { 0.0625, 0.125, 0.0625 },
                        { 0.125,  0.25,  0.125 },
                        { 0.0625, 0.125, 0.0625 }
                    };
                    break;
                case GaussianMatrix.Matrix5x5:
                    _kernelWidth = 5;
                    _kernelHeight = 5;
                    Kernel = new[,]
                    {
                        { 0.00390625, 0.015625, 0.0234375, 0.015625, 0.00390625 },
                        { 0.015625, 0.0625, 0.09375, 0.0625, 0.015625 },
                        { 0.0234375, 0.09375, 0.140625, 0.09375, 0.0234375 },
                        { 0.015625, 0.0625, 0.09375, 0.0625, 0.015625 },
                        { 0.00390625, 0.015625, 0.0234375, 0.015625, 0.00390625 },
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(matrix), matrix, null);
            }
        }

        public void Apply(ImageContext context) => ApplyConvolution(Kernel, _kernelWidth, _kernelHeight, context);
    }
}