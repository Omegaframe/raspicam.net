using System.Collections.Generic;
using MMALSharp.Mmal.Ports;
using MMALSharp.Mmal.Ports.Controls;
using MMALSharp.Mmal.Ports.Inputs;
using MMALSharp.Mmal.Ports.Outputs;

namespace MMALSharp.Mmal.Components
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
