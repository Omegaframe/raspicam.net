using MMALSharp.Native;

namespace MMALSharp.Extensions
{
    static class FlagExtensions
    {
        public static bool HasFlag(this uint value, MMALBufferProperties flag)
        {
            return ((int)value & (int)flag) == (int)flag;
        }
    }
}
