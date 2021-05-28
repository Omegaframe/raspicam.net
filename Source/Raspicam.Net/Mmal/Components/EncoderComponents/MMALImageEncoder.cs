using System;
using Raspicam.Net.Mmal.Handlers;
using Raspicam.Net.Mmal.Ports;
using Raspicam.Net.Mmal.Ports.Inputs;
using Raspicam.Net.Mmal.Ports.Outputs;
using Raspicam.Net.Native.Parameters;

namespace Raspicam.Net.Mmal.Components.EncoderComponents
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
