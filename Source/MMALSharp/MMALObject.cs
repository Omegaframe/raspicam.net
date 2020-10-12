namespace MMALSharp
{
    public abstract class MMALObject : IMMALObject
    {
        public bool IsDisposed { get; internal set; }

        protected MMALObject() { }

        public virtual void Dispose() => IsDisposed = true;
        
        public abstract bool CheckState();
    }
}
