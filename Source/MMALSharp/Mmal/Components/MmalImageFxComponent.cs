using System;
using System.Runtime.InteropServices;
using MMALSharp.Config;
using MMALSharp.Mmal.Ports.Inputs;
using MMALSharp.Mmal.Ports.Outputs;
using MMALSharp.Native.Parameters;
using MMALSharp.Native.Port;
using MMALSharp.Utility;
using static MMALSharp.MmalNativeExceptionHelper;
using static MMALSharp.Native.Parameters.MmalParametersCamera;

namespace MMALSharp.Mmal.Components
{
    class MmalImageFxComponent : MmalDownstreamHandlerComponent
    {
        public MmalParamImagefxType ImageEffect
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
            var colFx = new MmalParameterColorFxType(
                new MmalParameterHeaderType(MmalParameterColorEffect, Marshal.SizeOf<MmalParameterColorFxType>()),
                                                                                                        0,
                                                                                                        0,
                                                                                                        0);

            MmalCheck(MmalPort.GetParameter(Outputs[0].Ptr, &colFx.Hdr), "Unable to get colour enhancement value.");

            var fx = new ColorEffects(colFx.Enable == 1, MmalColor.FromYuvBytes(0, (byte)colFx.U, (byte)colFx.V));

            return fx;
        }

        unsafe void SetColourEnhancementValue(ColorEffects colorFx)
        {
            var (_, u, v) = MmalColor.RgbToYuvBytes(colorFx.Color);

            var colFx = new MmalParameterColorFxType(
                new MmalParameterHeaderType(MmalParameterColorEffect, Marshal.SizeOf<MmalParameterColorFxType>()),
                                                                                                        colorFx.Enable ? 1 : 0,
                                                                                                        u,
                                                                                                        v);

            MmalCheck(MmalPort.SetParameter(Outputs[0].Ptr, &colFx.Hdr), "Unable to set colour enhancement value.");
        }

        unsafe MmalParamImagefxType GetCurrentImageEffect()
        {
            const MmalParamImagefxType value = default;

            var effectStr = new MmalParameterImageFxType(
                new MmalParameterHeaderType(MmalParameterImageEffect, Marshal.SizeOf<MmalParameterImageFxType>()),
                value);

            MmalCheck(MmalPort.GetParameter(Outputs[0].Ptr, &effectStr.Hdr), "Unable to get current effect value.");

            return value;
        }

        unsafe void SetCurrentImageEffect(MmalParamImagefxType effect)
        {
            var effectStr = new MmalParameterImageFxType(
                new MmalParameterHeaderType(MmalParameterImageEffect, Marshal.SizeOf<MmalParameterImageFxType>()),
                effect);

            MmalCheck(MmalPort.SetParameter(Outputs[0].Ptr, &effectStr.Hdr), "Unable to set current effect value.");
        }
    }
}
