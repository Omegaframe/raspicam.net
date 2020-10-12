using System;
using MMALSharp.Native;
using MMALSharp.Native.Parameters;
using MMALSharp.Ports.Inputs;
using MMALSharp.Ports.Outputs;

namespace MMALSharp.Components.EncoderComponents
{
    public class MmalVideoDecoder : MmalEncoderBase, IVideoDecoder
    {
        public unsafe MmalVideoDecoder() : base(MmalParameters.MmalComponentDefaultVideoDecoder)
        {
            Inputs.Add(new InputPort((IntPtr)(&(*Ptr->Input[0])), this, Guid.NewGuid()));
            Outputs.Add(new VideoPort((IntPtr)(&(*Ptr->Output[0])), this, Guid.NewGuid()));
        }
    }
}
