using System;
using Microsoft.Extensions.Logging;
using MMALSharp.Mmal.Ports.Inputs;
using MMALSharp.Native.Parameters;
using MMALSharp.Utility;

namespace MMALSharp.Mmal.Components
{
    abstract class MmalRendererBase : MmalDownstreamComponent
    {
        protected unsafe MmalRendererBase(string name) : base(name)
        {
            Inputs.Add(new InputPort((IntPtr)(&(*Ptr->Input[0])), this, Guid.NewGuid()));
        }
    }

    class MmalNullSinkComponent : MmalRendererBase
    {
        public MmalNullSinkComponent() : base(MmalParameters.MmalComponentDefaultNullSink)
        {
            EnableComponent();
        }

        public override void PrintComponent()
        {
            MmalLog.Logger.LogInformation("Component: Null sink renderer");
        }
    }
}