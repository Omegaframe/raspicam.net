namespace MMALSharp.Mmal
{
    abstract class MmalObject : IMmalObject
    {
        public bool IsDisposed { get; private set; }

        protected MmalObject() { }

        public virtual void Dispose() => IsDisposed = true;
        
        public abstract bool CheckState();
    }
}
