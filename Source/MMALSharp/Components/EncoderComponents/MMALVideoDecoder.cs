using System;
using MMALSharp.Components.EncoderComponents;
using MMALSharp.Native;
using MMALSharp.Ports.Inputs;
using MMALSharp.Ports.Outputs;

namespace MMALSharp.Components
{
    /// <summary>
    /// This conformant component accepts encoded video in a number of
    /// different formats, and decodes it to raw YUV frames.
    /// https://github.com/raspberrypi/firmware/blob/master/documentation/ilcomponents/video_decode.html
    /// </summary>
    public class MMALVideoDecoder : MMALEncoderBase, IVideoDecoder
    {
        public unsafe MMALVideoDecoder() : base(MMALParameters.MMAL_COMPONENT_DEFAULT_VIDEO_DECODER)
        {
            Inputs.Add(new InputPort((IntPtr)(&(*Ptr->Input[0])), this, Guid.NewGuid()));
            Outputs.Add(new VideoPort((IntPtr)(&(*Ptr->Output[0])), this, Guid.NewGuid()));
        }
    }
}
