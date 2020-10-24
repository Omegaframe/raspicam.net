using System;
using MMALSharp.Callbacks;
using MMALSharp.Common.Utility;
using MMALSharp.Components;

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