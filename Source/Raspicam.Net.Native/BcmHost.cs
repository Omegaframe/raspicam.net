using System.Runtime.InteropServices;

namespace Raspicam.Net.Native
{
    public static class BcmHost
    {
        [DllImport("libbcm_host.so", EntryPoint = "bcm_host_init", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Initialize();

        [DllImport("libbcm_host.so", EntryPoint = "bcm_host_deinit", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Uninitialize();
    }
}
