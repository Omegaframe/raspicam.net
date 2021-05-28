using System;
using Microsoft.Extensions.Logging;
using Raspicam.Net.Mmal.Ports.Inputs;
using Raspicam.Net.Native.Parameters;
using Raspicam.Net.Utility;

namespace Raspicam.Net.Mmal.Components
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