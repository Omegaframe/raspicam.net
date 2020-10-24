using System.Collections.Generic;
using MMALSharp.Mmal.Components;

namespace MMALSharp.Mmal
{
    static class MmalBootstrapper
    {
        public static List<MmalDownstreamComponent> DownstreamComponents { get; } = new List<MmalDownstreamComponent>();
    }
}