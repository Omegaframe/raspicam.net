using System;
using Raspicam.Net.Config;
using Raspicam.Net.Mmal.Callbacks;
using Raspicam.Net.Mmal.Components;

namespace Raspicam.Net.Mmal.Ports
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