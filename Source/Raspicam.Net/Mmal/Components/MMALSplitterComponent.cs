using System;
using Raspicam.Net.Mmal.Ports.Inputs;
using Raspicam.Net.Mmal.Ports.Outputs;
using Raspicam.Net.Native.Parameters;

namespace Raspicam.Net.Mmal.Components
{
    class MmalSplitterComponent : MmalDownstreamHandlerComponent
    {
        public unsafe MmalSplitterComponent() : base(MmalParameters.MmalComponentDefaultVideoSplitter)
        {
            Inputs.Add(new InputPort((IntPtr)(&(*Ptr->Input[0])), this, Guid.NewGuid()));

            for (var i = 0; i < 4; i++)
                Outputs.Add(new SplitterVideoPort((IntPtr)(&(*Ptr->Output[i])), this, Guid.NewGuid()));
        }
    }
}
