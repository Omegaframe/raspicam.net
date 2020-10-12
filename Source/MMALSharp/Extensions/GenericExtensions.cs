using System;

namespace MMALSharp.Extensions
{
    static class GenericExtensions
    {
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0)
                return min;

            if (val.CompareTo(max) > 0)
                return max;

            return val;
        }

        public static float ToFloat(this byte val) => val / 255.0f;
        public static byte ToByte(this float val) => (byte)(val * 255.999f);
    }
}
