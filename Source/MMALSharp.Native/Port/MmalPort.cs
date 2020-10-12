using System;
using System.Runtime.InteropServices;
using MMALSharp.Native.Buffer;

namespace MMALSharp.Native.Port
{
    public static class MmalPort
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate void MmalPortBhCbT(MmalPortType* port, MmalBufferHeader* buffer);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_format_commit", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalUtil.MmalStatusT Commit(MmalPortType* port);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_enable", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalUtil.MmalStatusT Enable(MmalPortType* port, IntPtr cb);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_disable", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalUtil.MmalStatusT Disable(MmalPortType* port);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_flush", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalUtil.MmalStatusT Flush(MmalPortType* port);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_parameter_set", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalUtil.MmalStatusT SetParameter(MmalPortType* port, MMAL_PARAMETER_HEADER_T* header);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_parameter_get", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalUtil.MmalStatusT GetParameter(MmalPortType* port, MMAL_PARAMETER_HEADER_T* header);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_send_buffer", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalUtil.MmalStatusT SendBuffer(MmalPortType* port, MmalBufferHeader* header);
    }
}