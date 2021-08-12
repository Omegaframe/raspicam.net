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

        public double AnalogGain  => _camera.GetAnalogGain();
        public double DigitalGain => _camera.GetDigitalGain();
        public int ShuttderSpeedMs => _camera.GetShutterSpeed();
        public int Iso => _camera.GetIso();
    }
}
