namespace MMALSharp.Processing.Processors.Bayer
{
    public static class BayerOperationsExtensions
    {
        public static IFrameProcessingContext StripBayerMetadata(this IFrameProcessingContext context, CameraVersion version) => context.Apply(new BayerMetaProcessor(version));
    }
}
