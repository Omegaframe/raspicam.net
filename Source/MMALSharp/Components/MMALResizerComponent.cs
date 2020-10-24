using System;
using MMALSharp.Ports.Inputs;
using MMALSharp.Ports.Outputs;
using static MMALSharp.Native.Parameters.MmalParameters;

namespace MMALSharp.Components
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
