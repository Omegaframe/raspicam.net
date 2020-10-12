using System.Collections.Generic;
using MMALSharp.Components;

namespace MMALSharp
{
    public static class MmalBootstrapper
    {
        public static List<MMALDownstreamComponent> DownstreamComponents { get; } = new List<MMALDownstreamComponent>();
    }
}