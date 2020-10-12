namespace MMALSharp.Common.Utility
{
    public class Helpers
    {
        public static string ConvertBytesToMegabytes(long bytes)
        {
            return ((bytes / 1024f) / 1024f).ToString("0.00mb");
        }

        public static int FourCcFromString(string s)
        {
            int a1 = s[0];
            int b1 = s[1];
            int c1 = s[2];
            int d1 = s[3];
            return a1 | (b1 << 8) | (c1 << 16) | (d1 << 24);
        }
    }
}
