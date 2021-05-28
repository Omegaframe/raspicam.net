﻿namespace Raspicam.Net.Mmal.Components.EncoderComponents
{
    abstract class MmalEncoderBase : MmalDownstreamHandlerComponent, IEncoder
    {
        protected MmalEncoderBase(string encoderName) : base(encoderName) { }
    }
}
