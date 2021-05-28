using System.Runtime.InteropServices;
using Raspicam.Net.Native.Util;

namespace Raspicam.Net.Native.Pool
{
    public static class MmalPool
    {
        [DllImport("libmmal.so", EntryPoint = "mmal_pool_destroy", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void Destroy(MmalPoolType* pool);

        [DllImport("libmmal.so", EntryPoint = "mmal_pool_resize", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum Resize(MmalPoolType* pool, uint headers, uint payload_size);
    }
}
