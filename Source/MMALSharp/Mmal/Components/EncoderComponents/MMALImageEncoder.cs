using System;
using MMALSharp.Mmal.Handlers;
using MMALSharp.Mmal.Ports;
using MMALSharp.Mmal.Ports.Inputs;
using MMALSharp.Mmal.Ports.Outputs;
using MMALSharp.Native.Parameters;

namespace MMALSharp.Mmal.Components.EncoderComponents
{
    unsafe class MmalImageEncoder : MmalEncoderBase, IImageEncoder
    {
        public MmalImageEncoder() : base(MmalParameters.MmalComponentDefaultImageEncoder)
        {
            Inputs.Add(new InputPort((IntPtr)(&(*Ptr->Input[0])), this, Guid.NewGuid()));
            Outputs.Add(new FastStillPort((IntPtr)(&(*Ptr->Output[0])), this, Guid.NewGuid()));
        }

        public override IDownstreamComponent ConfigureOutputPort(int outputPort, IMmalPortConfig config, ICaptureHandler handler)
        {
            base.ConfigureOutputPort(outputPort, config, handler);

            return this;
        }
    }
}
