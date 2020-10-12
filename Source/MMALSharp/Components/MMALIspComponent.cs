using System;
using MMALSharp.Ports.Inputs;
using MMALSharp.Ports.Outputs;
using static MMALSharp.Native.Parameters.MmalParameters;

namespace MMALSharp.Components
{
    public class MmalIspComponent : MmalDownstreamHandlerComponent
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
