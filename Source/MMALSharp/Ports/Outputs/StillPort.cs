using System;
using Microsoft.Extensions.Logging;
using MMALSharp.Common;
using MMALSharp.Common.Utility;
using MMALSharp.Components;
using MMALSharp.Extensions;
using MMALSharp.Native;
using MMALSharp.Ports.Inputs;
using MMALSharp.Processing.Handlers;

namespace MMALSharp.Ports.Outputs
{
    public unsafe class StillPort : OutputPort, IStillPort
    {
        public override Resolution Resolution
        {
            get => new Resolution(Width, Height);
            internal set
            {
                if (value.Width == 0 || value.Height == 0)
                {
                    Width = MmalCameraConfig.Resolution.Pad().Width;
                    Height = MmalCameraConfig.Resolution.Pad().Height;
                }
                else
                {
                    Width = value.Pad().Width;
                    Height = value.Pad().Height;
                }
            }
        }

        public StillPort(IntPtr ptr, IComponent comp, Guid guid) : base(ptr, comp, guid) { }

        public StillPort(IPort copyFrom) : base((IntPtr)copyFrom.Ptr, copyFrom.ComponentReference, copyFrom.Guid) { }

        /// <inheritdoc />
        public override void Configure(IMMALPortConfig config, IInputPort copyFrom, IOutputCaptureHandler handler)
        {
            base.Configure(config, copyFrom, handler);

            if (config != null && config.EncodingType == MmalEncoding.Jpeg)
                this.SetParameter(MMALParametersCamera.MMAL_PARAMETER_JPEG_Q_FACTOR, config.Quality);
        }

        internal override void NativeOutputPortCallback(MMAL_PORT_T* port, MMAL_BUFFER_HEADER_T* buffer)
        {
            if (MmalCameraConfig.Debug)
                MmalLog.Logger.LogDebug($"{Name}: In native {nameof(StillPort)} output callback");

            base.NativeOutputPortCallback(port, buffer);
        }
    }
}
