using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.Logging;
using MMALSharp.Common.Utility;
using MMALSharp.Extensions;
using MMALSharp.Native;
using static MMALSharp.MmalNativeExceptionHelper;

namespace MMALSharp
{
    public unsafe class MmalBuffer : MmalObject, IBuffer
    {
        public byte* Data => Ptr->data;
        public uint Cmd => Ptr->Cmd;
        public uint AllocSize => Ptr->AllocSize;
        public uint Length => Ptr->Length;
        public uint Offset => Ptr->Offset;
        public uint Flags => Ptr->Flags;
        public long Pts => Ptr->Pts;
        public long Dts => Ptr->Dts;
        public MMAL_BUFFER_HEADER_TYPE_SPECIFIC_T Type => Marshal.PtrToStructure<MMAL_BUFFER_HEADER_TYPE_SPECIFIC_T>(Ptr->Type);
        public List<MMALBufferProperties> Properties { get; }
        public List<int> Events { get; }
        public MMAL_BUFFER_HEADER_T* Ptr { get; }

        public MmalBuffer(MMAL_BUFFER_HEADER_T* ptr)
        {
            Ptr = ptr;
            Properties = new List<MMALBufferProperties>();
            Events = new List<int>();
        }

        public void PrintProperties()
        {
            if (MmalCameraConfig.Debug)
                MmalLog.Logger.LogDebug(ToString());
        }

        public void ParseEvents()
        {
            if (Cmd == MMALEvents.MMAL_EVENT_EOS)
                MmalLog.Logger.LogDebug("Buffer event: MMAL_EVENT_EOS");

            if (Cmd == MMALEvents.MMAL_EVENT_ERROR)
                MmalLog.Logger.LogDebug("Buffer event: MMAL_EVENT_ERROR");

            if (Cmd == MMALEvents.MMAL_EVENT_FORMAT_CHANGED)
                MmalLog.Logger.LogDebug("Buffer event: MMAL_EVENT_FORMAT_CHANGED");

            if (Cmd == MMALEvents.MMAL_EVENT_PARAMETER_CHANGED)
                MmalLog.Logger.LogDebug("Buffer event: MMAL_EVENT_PARAMETER_CHANGED");
        }

        public bool AssertProperty(MMALBufferProperties property) => ((int)Flags & (int)property) == (int)property;

        public override string ToString()
        {
            InitialiseProperties();

            var sb = new StringBuilder();

            sb.Append(
                "\r\n Buffer Header \r\n" +
                "---------------- \r\n" +
                $"Length: {Length} \r\n" +
                $"Presentation Timestamp: {Pts} \r\n" +
                "Flags: \r\n");

            foreach (var prop in Properties)
                sb.Append($"{prop} \r\n");

            sb.Append("---------------- \r\n");

            return sb.ToString();
        }

        public override bool CheckState() => Ptr != null && (IntPtr)Ptr != IntPtr.Zero;

        public byte[] GetBufferData()
        {
            if (MmalCameraConfig.Debug)
                MmalLog.Logger.LogDebug("Getting data from buffer");

            MmalCheck(MMALBuffer.mmal_buffer_header_mem_lock(Ptr), "Unable to lock buffer header.");

            try
            {
                var ps = Ptr->data + Offset;
                var buffer = new byte[(int)Ptr->Length];
                Marshal.Copy((IntPtr)ps, buffer, 0, buffer.Length);
                MMALBuffer.mmal_buffer_header_mem_unlock(Ptr);

                return buffer;
            }
            catch
            {
                // If something goes wrong, unlock the header.
                MMALBuffer.mmal_buffer_header_mem_unlock(Ptr);
                MmalLog.Logger.LogWarning("Unable to handle data. Returning null.");
                return null;
            }
        }

        public void ReadIntoBuffer(byte[] source, int length, bool eof)
        {
            if (MmalCameraConfig.Debug)
                MmalLog.Logger.LogDebug($"Reading {length} bytes into buffer");

            Ptr->length = (uint)length;
            Ptr->dts = Ptr->pts = MMALUtil.MMAL_TIME_UNKNOWN;
            Ptr->offset = 0;

            if (eof)
                Ptr->flags = (uint)MMALBufferProperties.MMAL_BUFFER_HEADER_FLAG_EOS;

            Marshal.Copy(source, 0, (IntPtr)Ptr->data, length);
        }

        public void Acquire()
        {
            if (CheckState())
                MMALBuffer.mmal_buffer_header_acquire(Ptr);
        }

        public void Release()
        {
            if (CheckState())
            {
                if (MmalCameraConfig.Debug)
                    MmalLog.Logger.LogDebug("Releasing buffer.");

                MMALBuffer.mmal_buffer_header_release(Ptr);
            }
            else
            {
                MmalLog.Logger.LogWarning("Buffer null, could not release.");
            }

            Dispose();
        }

        public void Reset()
        {
            if (CheckState())
                MMALBuffer.mmal_buffer_header_reset(Ptr);
        }

        void InitialiseProperties()
        {
            Properties.Clear();

            if (!CheckState())
                return;

            var availableFlags = Enum.GetValues(typeof(MMALBufferProperties)).Cast<MMALBufferProperties>();
            foreach (var flag in availableFlags)
            {
                if (Flags.HasFlag(flag))
                    Properties.Add(flag);
            }
        }
    }
}
