using System.Runtime.InteropServices;

namespace MMALSharp.Native
{
    public static class BcmHost
    {
        [DllImport("libbcm_host.so", EntryPoint = "bcm_host_init", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Initialize();

        [DllImport("libbcm_host.so", EntryPoint = "bcm_host_deinit", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Uninitialize();
    }
}
