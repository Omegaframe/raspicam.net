using System.Collections.Generic;
using Raspicam.Net.Mmal.Components;

namespace Raspicam.Net.Mmal
{
    static class MmalBootstrapper
    {
        public static List<MmalDownstreamComponent> DownstreamComponents { get; } = new List<MmalDownstreamComponent>();
    }
}