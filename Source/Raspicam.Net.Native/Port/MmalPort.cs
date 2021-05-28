using System;
using System.Runtime.InteropServices;
using Raspicam.Net.Native.Buffer;
using Raspicam.Net.Native.Parameters;
using Raspicam.Net.Native.Util;

namespace Raspicam.Net.Native.Port
{
    public static class MmalPort
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate void MmalPortBhCbT(MmalPortType* port, MmalBufferHeader* buffer);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_format_commit", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum Commit(MmalPortType* port);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_enable", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum Enable(MmalPortType* port, IntPtr cb);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_disable", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum Disable(MmalPortType* port);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_flush", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum Flush(MmalPortType* port);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_parameter_set", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum SetParameter(MmalPortType* port, MmalParameterHeaderType* header);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_parameter_get", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum GetParameter(MmalPortType* port, MmalParameterHeaderType* header);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_send_buffer", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum SendBuffer(MmalPortType* port, MmalBufferHeader* header);
    }
}