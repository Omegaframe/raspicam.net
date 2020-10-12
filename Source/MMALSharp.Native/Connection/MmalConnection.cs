using System;
using System.Runtime.InteropServices;

namespace MMALSharp.Native.Connection
{
    public static class MmalConnection
    {
        public const uint MmalConnectionFlagTunnelling = 0x1u;
        public const uint MmalConnectionFlagAllocationOnInput = 0x2u;

        public unsafe delegate int MmalConnectionCallbackT(MmalConnectionType* conn);

        [DllImport("libmmal.so", EntryPoint = "mmal_connection_create", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalUtil.MmalStatusT Create(IntPtr* connection, MMAL_PORT_T* output, MMAL_PORT_T* input, uint flags);

        [DllImport("libmmal.so", EntryPoint = "mmal_connection_destroy", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalUtil.MmalStatusT Destroy(MmalConnectionType* connection);

        [DllImport("libmmal.so", EntryPoint = "mmal_connection_enable", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalUtil.MmalStatusT Enable(MmalConnectionType* connection);

        [DllImport("libmmal.so", EntryPoint = "mmal_connection_disable", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalUtil.MmalStatusT Disable(MmalConnectionType* connection);
    }
}
