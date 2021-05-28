using System;
using System.Runtime.InteropServices;
using Raspicam.Net.Native.Util;

namespace Raspicam.Net.Native.Component
{
    public static class MmalComponent
    {
        [DllImport("libmmal.so", EntryPoint = "mmal_component_create", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum Create(string name, IntPtr* comp);

        [DllImport("libmmal.so", EntryPoint = "mmal_component_acquire", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void Acquire(MmalComponentType* comp);

        [DllImport("libmmal.so", EntryPoint = "mmal_component_release", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum Release(MmalComponentType* comp);

        [DllImport("libmmal.so", EntryPoint = "mmal_component_destroy", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum Destroy(MmalComponentType* comp);

        [DllImport("libmmal.so", EntryPoint = "mmal_component_enable", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum Enable(MmalComponentType* comp);

        [DllImport("libmmal.so", EntryPoint = "mmal_component_disable", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum Disable(MmalComponentType* comp);
    }
}
