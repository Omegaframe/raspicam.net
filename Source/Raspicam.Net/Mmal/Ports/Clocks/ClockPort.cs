using System;
using Raspicam.Net.Mmal.Components;

namespace Raspicam.Net.Mmal.Ports.Clocks
{
    class ClockPort : GenericPort
    {
        public ClockPort(IntPtr ptr, IComponent comp, Guid guid) : base(ptr, comp, PortType.Clock, guid) { }
    }
}