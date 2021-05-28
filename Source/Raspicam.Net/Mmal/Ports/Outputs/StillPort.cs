using System;
using Raspicam.Net.Config;
using Raspicam.Net.Extensions;
using Raspicam.Net.Mmal.Components;
using Raspicam.Net.Mmal.Handlers;
using Raspicam.Net.Mmal.Ports.Inputs;
using Raspicam.Net.Native.Parameters;

namespace Raspicam.Net.Mmal.Ports.Outputs
{
    class StillPort : OutputPort, IStillPort
    {
        public override Resolution Resolution
        {
            get => new Resolution(Width, Height);
            internal set
            {
                if (value.Width == 0 || value.Height == 0)
                {
                    Width = CameraConfig.Resolution.Pad().Width;
                    Height = CameraConfig.Resolution.Pad().Height;
                }
                else
                {
                    Width = value.Pad().Width;
                    Height = value.Pad().Height;
                }
            }
        }

        public StillPort(IntPtr ptr, IComponent comp, Guid guid) : base(ptr, comp, guid) { }

        public override void Configure(IMmalPortConfig config, IInputPort copyFrom, ICaptureHandler handler)
        {
            base.Configure(config, copyFrom, handler);

            if (config != null && config.EncodingType == MmalEncoding.Jpeg)
                this.SetParameter(MmalParametersCamera.MmalParameterJpegQFactor, config.Quality);
        }
    }
}
