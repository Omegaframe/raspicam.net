namespace MMALSharp.Components
{
    /// <summary>
    /// Base class for all downstream components which support capture handlers.
    /// </summary>
    public abstract class MMALDownstreamHandlerComponent : MMALDownstreamComponent, IDownstreamHandlerComponent
    {
        protected MMALDownstreamHandlerComponent(string name) : base(name) { }
    }
}
