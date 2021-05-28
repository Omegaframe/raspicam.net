using System.Runtime.InteropServices;

namespace Raspicam.Net.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterColorFxType
    {
        public MmalParameterHeaderType Hdr;
        public int Enable;
        public int U;
        public int V;

        public MmalParameterColorFxType(MmalParameterHeaderType hdr, int enable, int u, int v)
        {
            Hdr = hdr;
            Enable = enable;
            U = u;
            V = v;
        }
    }
}