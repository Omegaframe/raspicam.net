using System;
using MMALSharp.Mmal.Ports.Inputs;
using MMALSharp.Mmal.Ports.Outputs;
using MMALSharp.Native.Parameters;

namespace MMALSharp.Mmal.Components.EncoderComponents
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
