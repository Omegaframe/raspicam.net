using System;
using System.Runtime.InteropServices;
using Raspicam.Net.Native.Port;
using Raspicam.Net.Native.Util;

namespace Raspicam.Net.Native.Connection
{
    public static class MmalConnection
    {
        public const uint MmalConnectionFlagTunnelling = 0x1u;
        public const uint MmalConnectionFlagAllocationOnInput = 0x2u;

        public unsafe delegate int MmalConnectionCallbackT(MmalConnectionType* conn);

        [DllImport("libmmal.so", EntryPoint = "mmal_connection_create", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum Create(IntPtr* connection, MmalPortType* output, MmalPortType* input, uint flags);

        [DllImport("libmmal.so", EntryPoint = "mmal_connection_destroy", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum Destroy(MmalConnectionType* connection);

        [DllImport("libmmal.so", EntryPoint = "mmal_connection_enable", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum Enable(MmalConnectionType* connection);

        [DllImport("libmmal.so", EntryPoint = "mmal_connection_disable", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum Disable(MmalConnectionType* connection);
    }
}
