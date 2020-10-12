using System;
using MMALSharp.Components;
using MMALSharp.Native;

namespace MMALSharp.Ports.Inputs
{
    public unsafe class OverlayPort : InputPort
    {
        public OverlayPort(IntPtr ptr, IComponent comp, Guid guid) : base(ptr, comp, guid) { }

        public OverlayPort(IPort copyFrom) : base((IntPtr)copyFrom.Ptr, copyFrom.ComponentReference, copyFrom.Guid) { }

        internal override void NativeInputPortCallback(MMAL_PORT_T* port, MMAL_BUFFER_HEADER_T* buffer) { }
    }
}
