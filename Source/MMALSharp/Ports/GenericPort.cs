using System;
using MMALSharp.Callbacks;
using MMALSharp.Components;
using MMALSharp.Config;
using MMALSharp.Utility;

namespace MMALSharp.Ports
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