using System;
using System.Runtime.InteropServices;

namespace MMALSharp.Native.Component
{
    public static class MmalComponent
    {
        [DllImport("libmmal.so", EntryPoint = "mmal_component_create", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalUtil.MmalStatusT Create(string name, IntPtr* comp);

        [DllImport("libmmal.so", EntryPoint = "mmal_component_acquire", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void Acquire(MmalComponentType* comp);

        [DllImport("libmmal.so", EntryPoint = "mmal_component_release", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalUtil.MmalStatusT Release(MmalComponentType* comp);

        [DllImport("libmmal.so", EntryPoint = "mmal_component_destroy", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalUtil.MmalStatusT Destroy(MmalComponentType* comp);

        [DllImport("libmmal.so", EntryPoint = "mmal_component_enable", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalUtil.MmalStatusT Enable(MmalComponentType* comp);

        [DllImport("libmmal.so", EntryPoint = "mmal_component_disable", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalUtil.MmalStatusT Disable(MmalComponentType* comp);
    }
}
