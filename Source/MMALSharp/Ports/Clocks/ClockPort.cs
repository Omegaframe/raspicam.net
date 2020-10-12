using System;
using MMALSharp.Components;

namespace MMALSharp.Ports.Clocks
{
    public class ClockPort : GenericPort
    {
        public ClockPort(IntPtr ptr, IComponent comp, Guid guid) : base(ptr, comp, PortType.Clock, guid) { }
    }
}