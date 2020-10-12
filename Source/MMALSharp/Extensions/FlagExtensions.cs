using MMALSharp.Native;

namespace MMALSharp.Extensions
{
    static class FlagExtensions
    {
        public static bool HasFlag(this uint value, MmalBufferProperties flag) => ((int)value & (int)flag) == (int)flag;
    }
}
