using System;
using System.Runtime.InteropServices;

namespace MMALSharp.Native
{
    public enum MmalBufferProperties
    {
        MmalBufferHeaderFlagEos = 1 << 0,
        MmalBufferHeaderFlagFrameStart = 1 << 1,
        MmalBufferHeaderFlagFrameEnd = 1 << 2,
        MmalBufferHeaderFlagFrame = MmalBufferHeaderFlagFrameStart | MmalBufferHeaderFlagFrameEnd,
        MmalBufferHeaderFlagKeyframe = 1 << 3,
        MmalBufferHeaderFlagDiscontinuity = 1 << 4,
        MmalBufferHeaderFlagConfig = 1 << 5,
        MmalBufferHeaderFlagEncrypted = 1 << 6,
        MmalBufferHeaderFlagCodecSideInfo = 1 << 7,
        MmalBufferHeaderFlagsSnapshot = 1 << 8,
        MmalBufferHeaderFlagCorrupted = 1 << 9,
        MmalBufferHeaderFlagTransmissionFailed = 1 << 10,
        MmalBufferHeaderFlagDecodeOnly = 1 << 11,
        MmalBufferHeaderFlagNal = 1 << 12,
        MmalBufferHeaderFlagUnknown = 9998,
        MmalBufferHeaderFlagCompleteFrame = 9999,
    }

    public static class MmalBuffer
    {
        public static int MmalBufferHeaderVideoFlagInterlaced = 1 << 0;
        public static int MmalBufferHeaderVideoFlagTopFieldFirst = 1 << 2;
        public static int MmalBufferHeaderVideoFlagDisplayExternal = 1 << 3;
        public static int MmalBufferHeaderVideoFlagProtected = 1 << 4;

        // Pointer to void * Pointer to MMAL_BUFFER_HEADER_T -> Returns MMAL_BOOL_T
        public unsafe delegate int MmalBhPreReleaseCbT(IntPtr ptr, MMAL_BUFFER_HEADER_T* ptr2);

        [DllImport("libmmal.so", EntryPoint = "mmal_buffer_header_acquire", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void mmal_buffer_header_acquire(MMAL_BUFFER_HEADER_T* header);

        [DllImport("libmmal.so", EntryPoint = "mmal_buffer_header_reset", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void mmal_buffer_header_reset(MMAL_BUFFER_HEADER_T* header);

        [DllImport("libmmal.so", EntryPoint = "mmal_buffer_header_release", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void mmal_buffer_header_release(MMAL_BUFFER_HEADER_T* header);

        [DllImport("libmmal.so", EntryPoint = "mmal_buffer_header_release_continue", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void mmal_buffer_header_release_continue(MMAL_BUFFER_HEADER_T* header);

        [DllImport("libmmal.so", EntryPoint = "mmal_buffer_header_pre_release_cb_set", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void mmal_buffer_header_pre_release_cb_set(MMAL_BUFFER_HEADER_T* header, [MarshalAs(UnmanagedType.FunctionPtr)] MmalBhPreReleaseCbT cb, void* userdata);

        [DllImport("libmmal.so", EntryPoint = "mmal_buffer_header_replicate", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalUtil.MmalStatusT mmal_buffer_header_replicate(MMAL_BUFFER_HEADER_T* header, MMAL_BUFFER_HEADER_T* header2);

        [DllImport("libmmal.so", EntryPoint = "mmal_buffer_header_mem_lock", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe MmalUtil.MmalStatusT mmal_buffer_header_mem_lock(MMAL_BUFFER_HEADER_T* header);

        [DllImport("libmmal.so", EntryPoint = "mmal_buffer_header_mem_unlock", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void mmal_buffer_header_mem_unlock(MMAL_BUFFER_HEADER_T* header);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_BUFFER_HEADER_VIDEO_SPECIFIC_T
    {
        uint planes;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        uint[] offset, pitch;

        uint flags;

        public uint Planes => this.planes;

        public uint[] Offset => this.offset;

        public uint[] Pitch => this.pitch;

        public uint Flags => this.flags;

        public MMAL_BUFFER_HEADER_VIDEO_SPECIFIC_T(uint planes, uint[] offset, uint[] pitch, uint flags)
        {
            this.planes = planes;
            this.offset = offset;
            this.pitch = pitch;
            this.flags = flags;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_BUFFER_HEADER_TYPE_SPECIFIC_T
    {
        MMAL_BUFFER_HEADER_VIDEO_SPECIFIC_T video;

        public MMAL_BUFFER_HEADER_VIDEO_SPECIFIC_T Video => video;

        public MMAL_BUFFER_HEADER_TYPE_SPECIFIC_T(MMAL_BUFFER_HEADER_VIDEO_SPECIFIC_T video)
        {
            this.video = video;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_BUFFER_HEADER_PRIVATE_T
    {
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MMAL_BUFFER_HEADER_T
    {
        MMAL_BUFFER_HEADER_T* next;
        IntPtr priv;
        uint cmd;
        public byte* data;
        public uint allocSize, length, offset, flags;
        public long pts, dts;

        IntPtr type, userData;

        public MMAL_BUFFER_HEADER_T* Next => this.next;

        public IntPtr Priv => this.priv;

        public uint Cmd => this.cmd;

        public byte* Data => this.data;

        public uint AllocSize => this.allocSize;

        public uint Length => this.length;

        public uint Offset => this.offset;

        public uint Flags => this.flags;

        public long Pts => this.pts;

        public long Dts => this.dts;

        public IntPtr Type => this.type;

        public IntPtr UserData => this.userData;

        public MMAL_BUFFER_HEADER_T(MMAL_BUFFER_HEADER_T* next, IntPtr priv, uint cmd, byte* data, uint allocSize, uint length, uint offset, uint flags, long pts, long dts, IntPtr type, IntPtr userData)
        {
            this.next = next;
            this.priv = priv;
            this.cmd = cmd;
            this.data = data;
            this.allocSize = allocSize;
            this.length = length;
            this.offset = offset;
            this.flags = flags;
            this.pts = pts;
            this.dts = dts;
            this.type = type;
            this.userData = userData;
        }
    }
}