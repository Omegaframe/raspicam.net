using System;
using MMALSharp.Mmal.Components;

namespace MMALSharp.Mmal.Ports.Clocks
{
    class ClockPort : GenericPort
    {
        public ClockPort(IntPtr ptr, IComponent comp, Guid guid) : base(ptr, comp, PortType.Clock, guid) { }
    }
}