using System.Runtime.InteropServices;

namespace MMALSharp.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterHeaderType
    {
        public int Id { get; set; }

        public int Size { get; set; }

        public MmalParameterHeaderType(int id, int size)
        {
            Id = id;
            Size = size;
        }
    }
}