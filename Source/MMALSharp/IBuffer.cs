using System.Collections.Generic;
using MMALSharp.Native.Buffer;

namespace MMALSharp
{
    public interface IBuffer : IMmalObject
    {
        uint Cmd { get; }
        uint AllocSize { get; }
        uint Length { get; }
        uint Offset { get; }
        uint Flags { get; }
        long Pts { get; }
        long Dts { get; }
        MmalBufferHeaderTypeSpecific Type { get; }
        List<MmalBufferProperties> Properties { get; }
        List<int> Events { get; }
        unsafe MmalBufferHeader* Ptr { get; }

        void PrintProperties();
        void ParseEvents();
        bool AssertProperty(MmalBufferProperties property);
        byte[] GetBufferData();
        void ReadIntoBuffer(byte[] source, int length, bool eof);
        void Acquire();
        void Release();
        void Reset();
    }
}
