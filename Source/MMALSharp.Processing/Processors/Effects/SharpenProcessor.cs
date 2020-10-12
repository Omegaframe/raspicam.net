using MMALSharp.Common;

namespace MMALSharp.Processing.Processors.Effects
{
    public class SharpenProcessor : ConvolutionBase, IFrameProcessor
    {
        const int KernelWidth = 3;
        const int KernelHeight = 3;

        readonly double[,] _kernel = 
        {
            { 0, -1, 0 },
            { -1,  5, -1 },
            { 0, -1, 0 }
        };

        public void Apply(ImageContext context) => ApplyConvolution(_kernel, KernelWidth, KernelHeight, context);
    }
}
