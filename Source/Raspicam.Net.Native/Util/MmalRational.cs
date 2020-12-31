using System;
using System.Runtime.InteropServices;

namespace MMALSharp.Native.Util
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalRational
    {
        public int Num;
        public int Den;

        public MmalRational(double num)
        {
            if (num < 1)
            {
                var multiplier = 100;
                var doubleNum = num * 100;

                while (doubleNum < 1)
                {
                    doubleNum *= 10;
                    multiplier *= 10;
                }

                Num = Convert.ToInt32(doubleNum);
                Den = multiplier;
            }
            else
            {
                Num = Convert.ToInt32(num * 10);
                Den = 10;
            }
        }

        public MmalRational(int num, int den)
        {
            Num = num;
            Den = den;
        }
    }
}
