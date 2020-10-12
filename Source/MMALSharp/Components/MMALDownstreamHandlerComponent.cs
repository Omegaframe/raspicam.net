namespace MMALSharp.Components
{
    public abstract class MmalDownstreamHandlerComponent : MmalDownstreamComponent, IDownstreamHandlerComponent
    {
        protected MmalDownstreamHandlerComponent(string name) : base(name) { }
    }
}
