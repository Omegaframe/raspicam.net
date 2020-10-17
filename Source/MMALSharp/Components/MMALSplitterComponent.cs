using System;
using MMALSharp.Native;
using MMALSharp.Native.Parameters;
using MMALSharp.Ports;
using MMALSharp.Ports.Inputs;
using MMALSharp.Ports.Outputs;
using MMALSharp.Processing.Handlers;

namespace MMALSharp.Components
{
    public class MmalSplitterComponent : MmalDownstreamHandlerComponent
    {
        public unsafe MmalSplitterComponent()            : base(MmalParameters.MmalComponentDefaultVideoSplitter)
        {
            Inputs.Add(new InputPort((IntPtr)(&(*Ptr->Input[0])), this, Guid.NewGuid()));

            for (var i = 0; i < 4; i++)            
                Outputs.Add(new SplitterVideoPort((IntPtr)(&(*Ptr->Output[i])), this, Guid.NewGuid()));            
        }
    }
}
