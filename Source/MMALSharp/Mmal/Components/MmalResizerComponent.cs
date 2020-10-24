using System;
using MMALSharp.Mmal.Ports.Inputs;
using MMALSharp.Mmal.Ports.Outputs;
using static MMALSharp.Native.Parameters.MmalParameters;

namespace MMALSharp.Mmal.Components
{
    sealed class MmalResizerComponent : MmalDownstreamHandlerComponent
    {
        public unsafe MmalResizerComponent() : base(MmalComponentDefaultResizer)
        {
            // Default to use still image port behaviour.
            Inputs.Add(new InputPort((IntPtr)(&(*Ptr->Input[0])), this, Guid.NewGuid()));
            Outputs.Add(new StillPort((IntPtr)(&(*Ptr->Output[0])), this, Guid.NewGuid()));
        }
    }
}
