using MMALSharp.Ports.Outputs;
using MMALSharp.Processing.Handlers;

namespace MMALSharp.Components
{
    public interface ICameraComponent : IComponent
    {
        IOutputPort PreviewPort { get; }
        IOutputPort VideoPort { get; }
        IOutputPort StillPort { get; }
        ICameraInfoComponent CameraInfo { get; }

        void Initialise(ICaptureHandler stillCaptureHandler = null, ICaptureHandler videoCaptureHandler = null);
    }
}
