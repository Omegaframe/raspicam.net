namespace Raspicam.Net.Mmal.Components
{
    interface ICameraInfoComponent : IComponent
    {
        string SensorName { get; }      
        int MaxWidth { get; }       
        int MaxHeight { get; }
    }
}
