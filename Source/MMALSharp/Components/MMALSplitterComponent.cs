using System;
using MMALSharp.Native.Parameters;
using MMALSharp.Ports.Inputs;
using MMALSharp.Ports.Outputs;

namespace MMALSharp.Components
{
    class MmalSplitterComponent : MmalDownstreamHandlerComponent
    {
        public unsafe MmalSplitterComponent()            : base(MmalParameters.MmalComponentDefaultVideoSplitter)
        {
            Inputs.Add(new InputPort((IntPtr)(&(*Ptr->Input[0])), this, Guid.NewGuid()));

            for (var i = 0; i < 4; i++)            
                Outputs.Add(new SplitterVideoPort((IntPtr)(&(*Ptr->Output[i])), this, Guid.NewGuid()));            
        }
    }
}
