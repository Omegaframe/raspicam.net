using MMALSharp.Common;

namespace MMALSharp.Processing.Processors.Effects
{
    public enum EdStrength
    {
        Low,
        Medium,
        High
    }

    public class EdgeDetection : ConvolutionBase, IFrameProcessor
    {
        public const int KernelWidth = 3;
        public const int KernelHeight = 3;

        public static double[,] LowStrengthKernel = 
        {
            { -1, 0, 1 },
            { 0, 0, 0 },
            { 1, 0, -1 }
        };

        public static double[,] MediumStrengthKernel = 
        {
            { 0, 1, 0 },
            { 1, -4, 1 },
            { 0, 1, 0 }
        };

        public static double[,] HighStrengthKernel = 
        {
            { -1, -1, -1 },
            { -1, 8, -1 },
            { -1, -1, -1 }
        };

        public double[,] Kernel { get; }

        public EdgeDetection(EdStrength strength)
        {
            Kernel = strength switch
            {
                EdStrength.Low => LowStrengthKernel,
                EdStrength.Medium => MediumStrengthKernel,
                EdStrength.High => HighStrengthKernel,
                _ => Kernel
            };
        }

        public void Apply(ImageContext context) => ApplyConvolution(Kernel, KernelWidth, KernelHeight, context);
    }
}