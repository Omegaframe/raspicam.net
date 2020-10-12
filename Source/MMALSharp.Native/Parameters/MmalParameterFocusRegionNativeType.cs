using System.Runtime.InteropServices;
using MMALSharp.Native.Util;

namespace MMALSharp.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterFocusRegionNativeType
    {
        public MmalRect Rect;
        public int Weight;
        public int Mask;
        public MmalParameterFocusRegionType Type;

        public MmalParameterFocusRegionNativeType(MmalRect rect, int weight, int mask, MmalParameterFocusRegionType type)
        {
            Rect = rect;
            Weight = weight;
            Mask = mask;
            Type = type;
        }
    }
}