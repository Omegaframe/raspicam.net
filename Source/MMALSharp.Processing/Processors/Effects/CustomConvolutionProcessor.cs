using MMALSharp.Common;

namespace MMALSharp.Processing.Processors.Effects
{
    public class CustomConvolutionProcessor : ConvolutionBase, IFrameProcessor
    {
        readonly int _kernelWidth;
        readonly int _kernelHeight;

        double[,] Kernel { get; }

        public CustomConvolutionProcessor(double[,] kernel, int kernelWidth, int kernelHeight)
        {
            Kernel = kernel;

            _kernelWidth = kernelWidth;
            _kernelHeight = kernelHeight;
        }

        public void Apply(ImageContext context) => ApplyConvolution(Kernel, _kernelWidth, _kernelHeight, context);
    }
}
