using Raspicam.Net.Extensions;
using Raspicam.Net.Mmal.Components;

namespace Raspicam.Net
{
    public class CurrentState
    {
        readonly MmalCameraComponent _camera;

        internal CurrentState(MmalCameraComponent camera)
        {
            _camera = camera;
        }

        public double AnalogGain { get => _camera.GetAnalogGain(); set => _camera.SetAnalogGain(value); }
        public double DigitalGain { get => _camera.GetDigitalGain(); set => _camera.SetDigitalGain(value); }
        public uint ShuttderSpeedMs { get => _camera.GetShutterSpeed(); set => _camera.SetShutterSpeed((int)value); }
        public uint Iso { get => _camera.GetIso(); set => _camera.SetIso((int)value); }
    }
}
