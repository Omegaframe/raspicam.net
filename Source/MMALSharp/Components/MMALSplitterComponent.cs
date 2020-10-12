using System;
using MMALSharp.Native;
using MMALSharp.Native.Parameters;
using MMALSharp.Ports;
using MMALSharp.Ports.Inputs;
using MMALSharp.Ports.Outputs;
using MMALSharp.Processing.Handlers;

namespace MMALSharp.Components
{
    public class MmalSplitterComponent : MmalDownstreamHandlerComponent
    {
        public unsafe MmalSplitterComponent()            : base(MmalParameters.MmalComponentDefaultVideoSplitter)
        {
            Inputs.Add(new InputPort((IntPtr)(&(*Ptr->Input[0])), this, Guid.NewGuid()));

            for (var i = 0; i < 4; i++)            
                Outputs.Add(new SplitterVideoPort((IntPtr)(&(*Ptr->Output[i])), this, Guid.NewGuid()));            
        }

        public override IDownstreamComponent ConfigureInputPort(IMmalPortConfig config, IPort copyPort, IInputCaptureHandler handler)
        {
            var bufferNum = Math.Max(Math.Max(Inputs[0].BufferNumRecommended, 3), config.BufferNum);
            
            config = new MmalPortConfig(
                config.EncodingType,
                config.PixelFormat,
                config.Quality,
                config.Bitrate,
                config.Timeout,
                config.Split,
                config.StoreMotionVectors,
                config.Width,
                config.Height,
                config.Framerate,
                config.ZeroCopy,
                bufferNum,
                config.BufferSize,
                config.Crop);

            base.ConfigureInputPort(config, copyPort, handler);

            return this;
        }
    }
}
