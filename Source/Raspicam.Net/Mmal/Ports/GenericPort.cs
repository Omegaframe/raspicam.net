using System;
using MMALSharp.Config;
using MMALSharp.Mmal.Callbacks;
using MMALSharp.Mmal.Components;

namespace MMALSharp.Mmal.Ports
{
    class GenericPort : PortBase<ICallbackHandler>
    {
        public override Resolution Resolution
        {
            get => new Resolution(Width, Height);
            internal set
            {
                Width = value.Pad().Width;
                Height = value.Pad().Height;
            }
        }

        public GenericPort(IntPtr ptr, IComponent comp, PortType type, Guid guid) : base(ptr, comp, type, guid) { }
    }
}