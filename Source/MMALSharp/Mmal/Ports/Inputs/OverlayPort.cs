using System;
using MMALSharp.Mmal.Components;
using MMALSharp.Native.Buffer;
using MMALSharp.Native.Port;

namespace MMALSharp.Mmal.Ports.Inputs
{
    unsafe class OverlayPort : InputPort
    {
        public OverlayPort(IntPtr ptr, IComponent comp, Guid guid) : base(ptr, comp, guid) { }

        public OverlayPort(IPort copyFrom) : base((IntPtr)copyFrom.Ptr, copyFrom.ComponentReference, copyFrom.Guid) { }

        internal override void NativeInputPortCallback(MmalPortType* port, MmalBufferHeader* buffer) { }
    }
}
