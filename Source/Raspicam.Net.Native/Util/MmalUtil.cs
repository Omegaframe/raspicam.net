using System.Runtime.InteropServices;
using Raspicam.Net.Native.Pool;
using Raspicam.Net.Native.Port;

namespace Raspicam.Net.Native.Util
{
    public static class MmalUtil
    {
        public static long MmalTimeUnknown => (long)1 << 63;


        [DllImport("libmmal.so", EntryPoint = "mmal_port_parameter_set_boolean", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum SetBoolean(MmalPortType* port, uint id, int value);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_parameter_get_boolean", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum GetBoolean(MmalPortType* port, uint id, ref int value);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_parameter_set_uint64", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum SetUint64(MmalPortType* port, uint id, ulong value);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_parameter_get_uint64", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum GetUint64(MmalPortType* port, uint id, ref ulong value);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_parameter_set_int64", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum SetInt64(MmalPortType* port, uint id, long value);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_parameter_get_int64", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum GetInt64(MmalPortType* port, uint id, ref long value);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_parameter_set_uint32", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum SetUint32(MmalPortType* port, uint id, uint value);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_parameter_get_uint32", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum GetUint32(MmalPortType* port, uint id, ref uint value);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_parameter_set_int32", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum SetInt32(MmalPortType* port, uint id, int value);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_parameter_get_int32", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum GetInt32(MmalPortType* port, uint id, ref int value);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_parameter_set_rational", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum SetRational(MmalPortType* port, uint id, MmalRational value);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_parameter_get_rational", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum GetRational(MmalPortType* port, uint id, ref MmalRational value);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_parameter_set_string", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalStatusEnum SetString(MmalPortType* port, uint id, [MarshalAs(UnmanagedType.LPTStr)] string value);

        [DllImport("libmmal.so", EntryPoint = "mmal_port_pool_create", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalPoolType* PoolCreate(MmalPortType* port, int headers, int payload_size);
    }
}
