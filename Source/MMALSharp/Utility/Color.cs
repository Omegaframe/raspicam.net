using System;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using MMALSharp.Extensions;

[assembly: InternalsVisibleTo("MMALSharp.Tests")]
namespace MMALSharp.Utility
{
    internal static class MmalColor
    {
        public static Color FromCie1960(float u, float v, float y)
        {
            // x and y chromaticity values
            var xc = (3f * u) / ((2f * u) - (8f * v) + 4);
            var yc = (2f * v) / ((2f * u) - (8f * v) + 4);

            var x = (y / yc) * xc;
            var z = (y / yc) * (1 - xc - yc);

            return FromCieXyz(x, y, z);
        }

        public static (float cu, float cv, float y) RgbToCie1960(Color c)
        {
            var (x, v, z) = RgbToCieXyz(c);

            var u = (2f / 3f) * x;
            var w = (1f / 2f) * (-x + (3 * v) + z);

            // calculate chromaticity variables
            var cu = u / (u + v + w);
            var cv = v / (u + v + w);

            return (cu, cv, v);
        }

        public static (float x, float y, float z) RgbToCieXyz(Color c)
        {
            var r = c.R.ToFloat();
            var g = c.G.ToFloat();
            var b = c.B.ToFloat();

            var rl = ToXyzLinear(r);
            var gl = ToXyzLinear(g);
            var bl = ToXyzLinear(b);

            var rVector = new Vector3(0.4124f * rl, 0.3576f * gl, 0.1805f * bl);
            var gVector = new Vector3(0.2126f * rl, 0.7152f * gl, 0.0722f * bl);
            var bVector = new Vector3(0.0193f * rl, 0.1192f * gl, 0.9505f * bl);

            var x = rVector.X + rVector.Y + rVector.Z;
            var y = gVector.X + gVector.Y + gVector.Z;
            var z = bVector.X + bVector.Y + bVector.Z;

            return (x, y, z);
        }

        public static (float y, float i, float q) RgbToYiq(Color c)
        {
            var r = c.R.ToFloat();
            var g = c.G.ToFloat();
            var b = c.B.ToFloat();

            var y = (float)((0.30 * r) + (0.59 * g) + (0.11 * b));
            var i = (float)((0.60 * r) - (0.28 * g) - (0.32 * b));
            var q = (float)((0.21 * r) - (0.52 * g) + (0.31 * b));

            return (y.Clamp(0, 1), i.Clamp(-1, 1), q.Clamp(-1, 1));
        }

        public static (float h, float l, float s) RgbToHls(Color c)
        {
            float h, s;

            var r = c.R.ToFloat();
            var g = c.G.ToFloat();
            var b = c.B.ToFloat();

            var maxC = GetMaxComponent(r, g, b);
            var minC = GetMinComponent(r, g, b);

            var l = (minC + maxC) / 2.0f;

            if (minC == maxC)
                return (0.0f, l, 0.0f);

            if (l <= 0.5f)
                s = (maxC - minC) / (maxC + minC);
            else
                s = (maxC - minC) / (2.0f - maxC - minC);

            var rc = (maxC - r) / (maxC - minC);
            var gc = (maxC - g) / (maxC - minC);
            var bc = (maxC - b) / (maxC - minC);

            if (r == maxC)
                h = bc - gc;
            else if (g == maxC)
                h = 2.0f + rc - bc;
            else
                h = 4.0f + gc - rc;

            h = (h / 6.0f) % 1.0f;

            return (h.Clamp(0, 1), l.Clamp(0, 1), s.Clamp(0, 1));
        }

        public static (float h, float s, float v) RgbToHsv(Color c)
        {
            float h;

            var r = c.R.ToFloat();
            var g = c.G.ToFloat();
            var b = c.B.ToFloat();

            var maxC = GetMaxComponent(r, g, b);
            var minC = GetMinComponent(r, g, b);

            var v = maxC;

            if (minC == maxC)
                return (0.0f, 0.0f, v);

            var s = (maxC - minC) / maxC;

            var rc = (maxC - r) / (maxC - minC);
            var gc = (maxC - g) / (maxC - minC);
            var bc = (maxC - b) / (maxC - minC);

            if (r == maxC)
                h = bc - gc;
            else if (g == maxC)
                h = 2.0f + rc - bc;
            else
                h = 4.0f + gc - rc;

            h = (h / 6.0f) % 1.0f;

            return (h.Clamp(0, 1), s.Clamp(0, 1), v.Clamp(0, 1));
        }

        public static (float y, float u, float v) RgbToYuv(Color c)
        {
            var r = c.R.ToFloat();
            var g = c.G.ToFloat();
            var b = c.B.ToFloat();

            var y = (0.299f * r) + (0.587f * g) + (0.114f * b);
            var u = (-0.147f * r) - (0.289f * g) + (0.436f * b);
            var v = (0.615f * r) - (0.515f * g) - (0.100f * b);

            return (y, u, v);
        }

        public static (byte y, byte u, byte v) RgbToYuvBytes(Color c)
        {
            var y = (((66 * c.R) + (129 * c.G) + (25 * c.B) + 128) >> 8) + 16;
            var u = (((-38 * c.R) - (74 * c.G) + (112 * c.B) + 128) >> 8) + 128;
            var v = (((112 * c.R) - (94 * c.G) - (18 * c.B) + 128) >> 8) + 128;

            return ((byte)y, (byte)u, (byte)v);
        }

        public static Color FromYuv(float y, float u, float v)
        {
            y = y.Clamp(0, 1);
            u = u.Clamp(-0.436f, 0.436f);
            v = v.Clamp(-0.615f, 0.615f);

            var r = y + (1.140f * v);
            var g = y - (0.395f * u) - (0.581f * v);
            var b = y + (2.032f * u);

            return Color.FromArgb(255, r.ToByte(), g.ToByte(), b.ToByte());
        }

        public static Color FromYuvBytes(byte y, byte u, byte v)
        {
            var c = y - 16;
            var d = u - 128;
            var e = v - 128;

            var r = (((298 * c) + (409 * e) + 128) >> 8).Clamp(0, 255);
            var g = (((298 * c) - (100 * d) - (208 * e) + 128) >> 8).Clamp(0, 255);
            var b = (((298 * c) + (516 * d) + 128) >> 8).Clamp(0, 255);

            return Color.FromArgb(255, r, g, b);
        }

        public static Color FromYiq(float y, float i, float q)
        {
            y = y.Clamp(0, 1);
            i = i.Clamp(-1, 1);
            q = q.Clamp(-1, 1);

            var r = (y + (0.948262f * i) + (0.624013f * q)).Clamp(0, 1);
            var g = (y - (0.276066f * i) - (0.639810f * q)).Clamp(0, 1);
            var b = (y - (1.105450f * i) + (1.729860f * q)).Clamp(0, 1);

            return Color.FromArgb(255, r.ToByte(), g.ToByte(), b.ToByte());
        }

        public static Color FromHls(float h, float l, float s)
        {
            h = h.Clamp(0, 1);
            l = l.Clamp(0, 1);
            s = s.Clamp(0, 1);

            float m2;

            if (s == 0.0f)
                return Color.FromArgb(255, l.ToByte(), l.ToByte(), l.ToByte());

            if (l < 0.5f)
                m2 = l * (1.0f + s);
            else
                m2 = l + s - (l * s);

            var m1 = (2.0f * l) - m2;

            var r = HlsConstant(m1, m2, h + (1.0f / 3.0f));
            var g = HlsConstant(m1, m2, h);
            var b = HlsConstant(m1, m2, h - (1.0f / 3.0f));

            return Color.FromArgb(255, r.ToByte(), g.ToByte(), b.ToByte());
        }

        public static Color FromHsv(float h, float s, float v)
        {
            h = h.Clamp(0, 1);
            s = s.Clamp(0, 1);
            v = v.Clamp(0, 1);

            if (s == 0.0f)
                return Color.FromArgb(255, v.ToByte(), v.ToByte(), v.ToByte());

            var i = (int)(h * 6);
            var f = (h * 6.0f) - i;
            var p = v * (1.0f - s);
            var q = v * (1.0f - (s * f));
            var t = v * (1.0f - (s * (1.0f - f)));

            i %= 6;

            return i switch
            {
                0 => Color.FromArgb(255, v.ToByte(), t.ToByte(), p.ToByte()),
                1 => Color.FromArgb(255, q.ToByte(), v.ToByte(), p.ToByte()),
                2 => Color.FromArgb(255, p.ToByte(), v.ToByte(), t.ToByte()),
                3 => Color.FromArgb(255, p.ToByte(), q.ToByte(), v.ToByte()),
                4 => Color.FromArgb(255, t.ToByte(), p.ToByte(), v.ToByte()),
                5 => Color.FromArgb(255, v.ToByte(), p.ToByte(), q.ToByte()),
                _ => throw new Exception("Calculated invalid HSV value.")
            };
        }

        public static Color FromCieXyz(float x, float y, float z)
        {
            x = x.Clamp(0, 0.9505f);
            y = y.Clamp(0, 1.0000f);
            z = z.Clamp(0, 1.0890f);

            var rLinear = new Vector3(3.2404542f * x, -1.5371385f * y, -0.4985314f * z);
            var gLinear = new Vector3(-0.9692660f * x, 1.8760108f * y, 0.0415560f * z);
            var bLinear = new Vector3(0.0556434f * x, -0.2040259f * y, 1.0572252f * z);

            var sr = StandardRgbLinearTransform(rLinear.X + rLinear.Y + rLinear.Z).Clamp(0, 1);
            var sg = StandardRgbLinearTransform(gLinear.X + gLinear.Y + gLinear.Z).Clamp(0, 1);
            var sb = StandardRgbLinearTransform(bLinear.X + bLinear.Y + bLinear.Z).Clamp(0, 1);

            return Color.FromArgb(255, sr.ToByte(), sg.ToByte(), sb.ToByte());
        }

        static float HlsConstant(float m1, float m2, float hue)
        {
            hue %= 1f;

            if (hue < (1f / 6f))
                return m1 + ((m2 - m1) * hue * 6f);

            if (hue < 0.5f)
                return m2;

            if (hue < (1f / 3f))
                return m1 + ((m2 - m1) * ((2f / 3f) - hue) * 6f);

            return m1;
        }

        static float StandardRgbLinearTransform(float c)
        {
            if (c <= 0.0031308f)
                return 12.92f * c;

            return 1.055f * (((float)Math.Pow(c, 1 / 2.4)) - 0.055f);
        }

        static float ToXyzLinear(float c)
        {
            if (c <= 0.04045f)
                return c / 12.92f;

            return (float)Math.Pow((c + 0.055) / 1.055, 2.4);
        }

        static float GetMaxComponent(float r, float g, float b) => Math.Max(Math.Max(r, g), b);

        static float GetMinComponent(float r, float g, float b) => Math.Min(Math.Min(r, g), b);
    }
}
