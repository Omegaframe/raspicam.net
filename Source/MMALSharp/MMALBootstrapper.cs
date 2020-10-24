using System.Collections.Generic;
using MMALSharp.Components;

namespace MMALSharp
{
    static class MmalBootstrapper
    {
        public static List<MmalDownstreamComponent> DownstreamComponents { get; } = new List<MmalDownstreamComponent>();
    }
}