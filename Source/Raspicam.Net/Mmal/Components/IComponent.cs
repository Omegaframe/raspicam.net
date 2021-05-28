using System.Collections.Generic;
using Raspicam.Net.Mmal.Ports;
using Raspicam.Net.Mmal.Ports.Controls;
using Raspicam.Net.Mmal.Ports.Inputs;
using Raspicam.Net.Mmal.Ports.Outputs;

namespace Raspicam.Net.Mmal.Components
{
    interface IComponent : IMmalObject
    {
        IControlPort Control { get; }
        List<IInputPort> Inputs { get; }
        List<IOutputPort> Outputs { get; }
        List<IPort> Clocks { get; }
        List<IPort> Ports { get; }
        string Name { get; }
        bool Enabled { get; }
        bool ForceStopProcessing { get; set; }
        void EnableConnections();
        void DisableConnections();
        void PrintComponent();
        void AcquireComponent();
        void ReleaseComponent();
        void DestroyComponent();
        void EnableComponent();
        void DisableComponent();
        void CleanPortPools();
    }
}
