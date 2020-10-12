﻿using System;
using MMALSharp.Components;
using MMALSharp.Native;
using MMALSharp.Native.Buffer;

namespace MMALSharp.Ports.Inputs
{
    public unsafe class OverlayPort : InputPort
    {
        public OverlayPort(IntPtr ptr, IComponent comp, Guid guid) : base(ptr, comp, guid) { }

        public OverlayPort(IPort copyFrom) : base((IntPtr)copyFrom.Ptr, copyFrom.ComponentReference, copyFrom.Guid) { }

        internal override void NativeInputPortCallback(MMAL_PORT_T* port, MmalBufferHeader* buffer) { }
    }
}
