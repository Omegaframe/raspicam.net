using System;
using MMALSharp.Native.Parameters;
using MMALSharp.Ports.Inputs;
using MMALSharp.Ports.Outputs;

namespace MMALSharp.Components.EncoderComponents
{
    class MmalImageDecoder : MmalEncoderBase, IImageDecoder
    {
        public unsafe MmalImageDecoder() : base(MmalParameters.MmalComponentDefaultImageDecoder)
        {
            Inputs.Add(new InputPort((IntPtr)(&(*Ptr->Input[0])), this, Guid.NewGuid()));
            Outputs.Add(new StillPort((IntPtr)(&(*Ptr->Output[0])), this, Guid.NewGuid()));
        }
    }
}
