using System;
using MMALSharp.Mmal.Ports.Inputs;
using MMALSharp.Mmal.Ports.Outputs;
using static MMALSharp.Native.Parameters.MmalParameters;

namespace MMALSharp.Mmal.Components
{
    class MmalIspComponent : MmalDownstreamHandlerComponent
    {
        public unsafe MmalIspComponent() : base(MmalComponentIsp)
        {
            // Default to use still image port behaviour.
            Inputs.Add(new InputPort((IntPtr)(&(*Ptr->Input[0])), this, Guid.NewGuid()));

            for (var i = 0; i < Ptr->OutputNum; i++)
                Outputs.Add(new StillPort((IntPtr)(&(*Ptr->Output[i])), this, Guid.NewGuid()));
        }

        public unsafe MmalIspComponent(Type outputPortType) : base(MmalComponentIsp)
        {
            Inputs.Add(new InputPort((IntPtr)(&(*Ptr->Input[0])), this, Guid.NewGuid()));

            for (var i = 0; i < Ptr->OutputNum; i++)
                Outputs.Add((IOutputPort)Activator.CreateInstance(outputPortType, (IntPtr)(&(*Ptr->Output[i])), this, Guid.NewGuid()));
        }
    }
}
