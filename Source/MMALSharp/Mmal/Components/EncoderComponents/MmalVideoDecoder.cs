using System;
using MMALSharp.Mmal.Ports.Inputs;
using MMALSharp.Mmal.Ports.Outputs;
using MMALSharp.Native.Parameters;

namespace MMALSharp.Mmal.Components.EncoderComponents
{
    class MmalVideoDecoder : MmalEncoderBase, IVideoDecoder
    {
        public unsafe MmalVideoDecoder() : base(MmalParameters.MmalComponentDefaultVideoDecoder)
        {
            Inputs.Add(new InputPort((IntPtr)(&(*Ptr->Input[0])), this, Guid.NewGuid()));
            Outputs.Add(new VideoPort((IntPtr)(&(*Ptr->Output[0])), this, Guid.NewGuid()));
        }
    }
}
