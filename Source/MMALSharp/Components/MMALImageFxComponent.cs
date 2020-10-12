using System;
using System.Runtime.InteropServices;
using MMALSharp.Config;
using MMALSharp.Native;
using MMALSharp.Ports.Inputs;
using MMALSharp.Ports.Outputs;
using MMALSharp.Utility;
using static MMALSharp.MmalNativeExceptionHelper;
using static MMALSharp.Native.MmalParametersCamera;

namespace MMALSharp.Components
{
    public class MmalImageFxComponent : MmalDownstreamHandlerComponent
    {
        public MmalParamImagefxT ImageEffect
        {
            get => GetCurrentImageEffect();
            set => SetCurrentImageEffect(value);
        }

        public ColorEffects ColorEnhancement
        {
            get => GetColourEnhancementValue();
            set => SetColourEnhancementValue(value);
        }

        public unsafe MmalImageFxComponent() : base(MmalParameters.MmalComponentDefaultImageFx)
        {
            // Default to use still image port behaviour.
            Inputs.Add(new InputPort((IntPtr)(&(*Ptr->Input[0])), this, Guid.NewGuid()));
            Outputs.Add(new StillPort((IntPtr)(&(*Ptr->Output[0])), this, Guid.NewGuid()));
        }

        unsafe ColorEffects GetColourEnhancementValue()
        {
            var colFx = new MMAL_PARAMETER_COLOURFX_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterColorEffect, Marshal.SizeOf<MMAL_PARAMETER_COLOURFX_T>()),
                                                                                                        0,
                                                                                                        0,
                                                                                                        0);

            MmalCheck(MmalPort.mmal_port_parameter_get(Outputs[0].Ptr, &colFx.Hdr), "Unable to get colour enhancement value.");

            var fx = new ColorEffects(colFx.Enable == 1, MmalColor.FromYuvBytes(0, (byte)colFx.U, (byte)colFx.V));

            return fx;
        }

        unsafe void SetColourEnhancementValue(ColorEffects colorFx)
        {
            var (_, u, v) = MmalColor.RgbToYuvBytes(colorFx.Color);

            var colFx = new MMAL_PARAMETER_COLOURFX_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterColorEffect, Marshal.SizeOf<MMAL_PARAMETER_COLOURFX_T>()),
                                                                                                        colorFx.Enable ? 1 : 0,
                                                                                                        u,
                                                                                                        v);

            MmalCheck(MmalPort.mmal_port_parameter_set(Outputs[0].Ptr, &colFx.Hdr), "Unable to set colour enhancement value.");
        }

        unsafe MmalParamImagefxT GetCurrentImageEffect()
        {
            const MmalParamImagefxT value = default;

            var effectStr = new MMAL_PARAMETER_IMAGEFX_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterImageEffect, Marshal.SizeOf<MMAL_PARAMETER_IMAGEFX_T>()),
                value);

            MmalCheck(MmalPort.mmal_port_parameter_get(Outputs[0].Ptr, &effectStr.Hdr), "Unable to get current effect value.");

            return value;
        }

        unsafe void SetCurrentImageEffect(MmalParamImagefxT effect)
        {
            var effectStr = new MMAL_PARAMETER_IMAGEFX_T(
                new MMAL_PARAMETER_HEADER_T(MmalParameterImageEffect, Marshal.SizeOf<MMAL_PARAMETER_IMAGEFX_T>()),
                effect);

            MmalCheck(MmalPort.mmal_port_parameter_set(Outputs[0].Ptr, &effectStr.Hdr), "Unable to set current effect value.");
        }
    }
}
