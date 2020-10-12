using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MMALSharp.Common;

namespace MMALSharp.Processing.Processors.Effects
{
    public abstract class ConvolutionBase
    {
        public void ApplyConvolution(double[,] kernel, int kernelWidth, int kernelHeight, ImageContext context)
        {
            BitmapData bmpData;
            byte[] store = null;

            using (var ms = new MemoryStream(context.Data))
            using (var bmp = LoadBitmap(context, ms))
            {
                bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);

                if (context.IsRaw)
                    InitBitmapData(context, bmpData);

                var pNative = bmpData.Scan0;

                // Split image into 4 quadrants and process individually.
                var quadA = new Rectangle(0, 0, bmpData.Width / 2, bmpData.Height / 2);
                var quadB = new Rectangle(bmpData.Width / 2, 0, bmpData.Width / 2, bmpData.Height / 2);
                var quadC = new Rectangle(0, bmpData.Height / 2, bmpData.Width / 2, bmpData.Height / 2);
                var quadD = new Rectangle(bmpData.Width / 2, bmpData.Height / 2, bmpData.Width / 2, bmpData.Height / 2);

                var bytes = bmpData.Stride * bmp.Height;
                var rgbValues = new byte[bytes];

                // Copy the RGB values into the array.
                Marshal.Copy(pNative, rgbValues, 0, bytes);

                var bpp = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;

                var t1 = Task.Run(() => { ProcessQuadrant(quadA, bmp, bmpData, rgbValues, kernel, kernelWidth, kernelHeight, bpp); });
                var t2 = Task.Run(() => { ProcessQuadrant(quadB, bmp, bmpData, rgbValues, kernel, kernelWidth, kernelHeight, bpp); });
                var t3 = Task.Run(() => { ProcessQuadrant(quadC, bmp, bmpData, rgbValues, kernel, kernelWidth, kernelHeight, bpp); });
                var t4 = Task.Run(() => { ProcessQuadrant(quadD, bmp, bmpData, rgbValues, kernel, kernelWidth, kernelHeight, bpp); });

                Task.WaitAll(t1, t2, t3, t4);

                if (context.IsRaw && context.StoreFormat == null)
                {
                    store = new byte[bytes];
                    Marshal.Copy(pNative, store, 0, bytes);
                }

                bmp.UnlockBits(bmpData);

                if (!context.IsRaw || context.StoreFormat != null)
                {
                    using var ms2 = new MemoryStream();
                    bmp.Save(ms2, context.StoreFormat);
                    store = new byte[ms2.Length];
                    Array.Copy(ms2.ToArray(), 0, store, 0, ms2.Length);
                }
            }

            context.Data = store;
        }

        static Bitmap LoadBitmap(ImageContext imageContext, MemoryStream stream)
        {
            if (!imageContext.IsRaw)
                return new Bitmap(stream);

            PixelFormat format = default;

            // RGB16 doesn't appear to be supported by GDI?
            if (imageContext.PixelFormat == MmalEncoding.Rgb24)
                format = PixelFormat.Format24bppRgb;

            if (imageContext.PixelFormat == MmalEncoding.Rgb32)
                format = PixelFormat.Format32bppRgb;

            if (imageContext.PixelFormat == MmalEncoding.Rgba)
                format = PixelFormat.Format32bppArgb;

            if (format == default)
                throw new Exception($"Unsupported pixel format for Bitmap: {imageContext.PixelFormat}.");

            return new Bitmap(imageContext.Resolution.Width, imageContext.Resolution.Height, format);
        }

        static void InitBitmapData(ImageContext imageContext, BitmapData bmpData)
        {
            var pNative = bmpData.Scan0;
            Marshal.Copy(imageContext.Data, 0, pNative, imageContext.Data.Length);
        }

        static void ProcessQuadrant(Rectangle quad, Bitmap bmp, BitmapData bmpData, byte[] rgbValues, double[,] kernel, int kernelWidth, int kernelHeight, int pixelDepth)
        {
            unsafe
            {
                // Declare an array to hold the bytes of the bitmap.
                var stride = bmpData.Stride;

                var ptr1 = (byte*)bmpData.Scan0;

                for (var column = quad.X; column < quad.X + quad.Width; column++)
                {
                    for (var row = quad.Y; row < quad.Y + quad.Height; row++)
                    {
                        if (column > kernelWidth && row > kernelHeight)
                        {
                            int r1 = 0, g1 = 0, b1 = 0;

                            for (var l = 0; l < kernelWidth; l++)
                            {
                                for (var m = 0; m < kernelHeight; m++)
                                {
                                    r1 += (int)(rgbValues[(Bound(row + m, quad.Y + quad.Height) * stride) + (Bound(column + l, quad.X + quad.Width) * pixelDepth)] * kernel[l, m]);
                                    g1 += (int)(rgbValues[(Bound(row + m, quad.Y + quad.Height) * stride) + (Bound(column + l, quad.X + quad.Width) * pixelDepth) + 1] * kernel[l, m]);
                                    b1 += (int)(rgbValues[(Bound(row + m, quad.Y + quad.Height) * stride) + (Bound(column + l, quad.X + quad.Width) * pixelDepth) + 2] * kernel[l, m]);
                                }
                            }

                            ptr1[(column * pixelDepth) + (row * stride)] = (byte)Math.Max(0, r1);
                            ptr1[(column * pixelDepth) + (row * stride) + 1] = (byte)Math.Max(0, g1);
                            ptr1[(column * pixelDepth) + (row * stride) + 2] = (byte)Math.Max(0, b1);
                        }
                        else
                        {
                            ptr1[(column * pixelDepth) + (row * stride)] = 0;
                            ptr1[(column * pixelDepth) + (row * stride) + 1] = 0;
                            ptr1[(column * pixelDepth) + (row * stride) + 2] = 0;
                        }
                    }
                }
            }
        }

        static int Bound(int value, int endIndex)
        {
            if (value < 0)
                return 0;

            if (value < endIndex)
                return value;

            return endIndex - 1;
        }
    }
}