using System.Runtime.InteropServices;

namespace MMALSharp.Native.Util
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalRect
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;

        public MmalRect(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Height = height;
            Width = width;
        }
    }
}
