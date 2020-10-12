using System;
using System.Runtime.InteropServices;
using MMALSharp.Native.Component;
using MMALSharp.Native.Format;

namespace MMALSharp.Native.Port
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MmalPortType
    {
        public IntPtr Priv;
        public char* Name;
        public MmalPortTypeEnum Type;
        public ushort Index;
        public ushort IndexAll;
        public int IsEnabled;
        public MmalEsFormat* Format;
        public int BufferNumMin;
        public int BufferSizeMin;
        public int BufferAlignmentMin;
        public int BufferNumRecommended;
        public int BufferSizeRecommended;
        public int BufferNum;
        public int BufferSize;
        public MmalComponentType* Component;
        public IntPtr UserData;
        public uint Capabilities;

        public MmalPortType(IntPtr priv, char* name, MmalPortTypeEnum type, ushort index, ushort indexAll,
                           int isEnabled, MmalEsFormat* format, int bufferNumMin, int bufferSizeMin, int bufferAlignmentMin,
                           int bufferNumRecommended, int bufferSizeRecommended, int bufferNum, int bufferSize, MmalComponentType* component,
                           IntPtr userData, uint capabilities)
        {
            Priv = priv;
            Name = name;
            Type = type;
            Index = index;
            IndexAll = indexAll;
            IsEnabled = isEnabled;
            Format = format;
            BufferNumMin = bufferNumMin;
            BufferSizeMin = bufferSizeMin;
            BufferAlignmentMin = bufferAlignmentMin;
            BufferNumRecommended = bufferNumRecommended;
            BufferSizeRecommended = bufferSizeRecommended;
            BufferNum = bufferNum;
            BufferSize = bufferSize;
            Component = component;
            UserData = userData;
            Capabilities = capabilities;
        }
    }
}
