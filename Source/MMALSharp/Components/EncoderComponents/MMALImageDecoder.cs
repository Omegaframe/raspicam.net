using System;
using MMALSharp.Components.EncoderComponents;
using MMALSharp.Native;
using MMALSharp.Ports.Inputs;
using MMALSharp.Ports.Outputs;

namespace MMALSharp.Components
{
    /// <summary>
    /// A conformant image decode component, which takes encoded still images
    /// in various compressed formats on its input port, and decodes the image
    /// into raw pixels which are emitted on the output port.
    /// https://github.com/raspberrypi/firmware/blob/master/documentation/ilcomponents/image_decode.html
    /// </summary>
    public class MMALImageDecoder : MMALEncoderBase, IImageDecoder
    {
        public unsafe MMALImageDecoder() : base(MMALParameters.MMAL_COMPONENT_DEFAULT_IMAGE_DECODER)
        {
            Inputs.Add(new InputPort((IntPtr)(&(*Ptr->Input[0])), this, Guid.NewGuid()));
            Outputs.Add(new StillPort((IntPtr)(&(*Ptr->Output[0])), this, Guid.NewGuid()));
        }
    }
}
