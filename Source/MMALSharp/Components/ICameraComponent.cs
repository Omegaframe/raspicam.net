﻿using MMALSharp.Handlers;
using MMALSharp.Ports.Outputs;

namespace MMALSharp.Components
{
    interface ICameraComponent : IComponent
    {
        IOutputPort PreviewPort { get; }
        IOutputPort VideoPort { get; }
        IOutputPort StillPort { get; }
        ICameraInfoComponent CameraInfo { get; }

        void Initialise(ICaptureHandler stillCaptureHandler = null, ICaptureHandler videoCaptureHandler = null);
    }
}
