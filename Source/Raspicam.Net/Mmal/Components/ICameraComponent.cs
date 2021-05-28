using Raspicam.Net.Mmal.Ports.Outputs;

namespace Raspicam.Net.Mmal.Components
{
    interface ICameraComponent : IComponent
    {
        IOutputPort PreviewPort { get; }
        IOutputPort VideoPort { get; }
        IOutputPort StillPort { get; }
        ICameraInfoComponent CameraInfo { get; }

        void Initialise();
    }
}
