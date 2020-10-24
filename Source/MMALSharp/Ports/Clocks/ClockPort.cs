using System;
using MMALSharp.Components;

namespace MMALSharp.Ports.Clocks
{
    class ClockPort : GenericPort
    {
        public ClockPort(IntPtr ptr, IComponent comp, Guid guid) : base(ptr, comp, PortType.Clock, guid) { }
    }
}