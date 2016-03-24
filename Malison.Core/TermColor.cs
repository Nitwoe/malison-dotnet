using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Malison.Core
{
    /// <summary>
    /// Identifies a color that can be used in a terminal.
    /// Malison uses custom structure to contain color data in order to not
    /// depend on System.Drawing or some other assemblies providing a color type.
    /// </summary>
    public struct TermColor
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public double H;
        public double S;
        public double L;

        public TermColor(byte r, byte g, byte b, byte a, double h, double s, double l)
        {
            R = r;
            G = g;
            B = b;
            A = a;

            H = h;
            S = s;
            L = l;
        }

        public TermColor(int r, int g, int b, int a=255)
            :this((byte)r, (byte)g, (byte)b, (byte)a)
        { }

        public TermColor(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;

            double r_n = R / 255f;
            double g_n = G / 255f;
            double b_n = B / 255f;

            double v, m, vm, r2, g2, b2;

            H = 0; // default to black
            S = 0;
            L = 0;

            v = Math.Max(r_n, Math.Max(g_n, b_n));
            m = Math.Min(r_n, Math.Min(g_n, b_n));

            L = (m + v) / 2f;

            if (L <= 0.0)
            {
                return;
            }

            vm = v - m;

            S = vm;

            if (S > 0)
            {
                S /= (L <= .5f) ? (v + m) : (2f - vm);
            }
            else
            {
                return;
            }

            r2 = (v - r_n) / vm;
            g2 = (v - g_n) / vm;
            b2 = (v - b_n) / vm;

            if (r_n == v)
            {
                H = (g_n == m ? 5f + b2 : 1f - g2);
            }
            else if (g_n == v)
            {
                H = (b_n == m ? 1f + r2 : 3f - b2);
            }
            else
            {
                H = (r_n == m ? 3f + g2 : 5f - r2);

            }
            H = (H / 6f)*360f;
        }

        public TermColor(float r, float g, float b, float a = 1f)
            : this(Convert.ToByte(r * 255), Convert.ToByte(g * 255), Convert.ToByte(b * 255), Convert.ToByte(a * 255))
        { }

        #region Basic color helpers

        public static TermColor Black
        {
            get
            {
                return new TermColor(0, 0, 0);
            }
        }

        public static TermColor White
        {
            get
            {
                return new TermColor(255, 255, 255);
            }
        }

        public static TermColor Red
        {
            get
            {
                return new TermColor(255, 0, 0);
            }
        }

        public static TermColor Green
        {
            get
            {
                return new TermColor(0, 255, 0);
            }
        }

        public static TermColor Blue
        {
            get
            {
                return new TermColor(0, 0, 255);
            }
        }

        public static TermColor Yellow
        {
            get
            {
                return TermColor.Red + TermColor.Green;
            }
        }

        public static TermColor Cyan
        {
            get
            {
                return TermColor.Green + TermColor.Blue;
            }
        }

        public static TermColor Magenta
        {
            get
            {
                return TermColor.Red + TermColor.Blue;
            }
        }

        #endregion

        #region Color utilities
        
        /// <summary>
        /// Returns a new color lightened by given <paramref name="amount"/>.
        /// This method leaves alpha channel unaffected.
        /// </summary>
        public static TermColor Lighten(TermColor source, double amount)
        {
            return TermColor.FromHSLA(source.H, source.S, Math.Max(Math.Min(source.L + amount, 1), 0), source.A);
        }

        /// <summary>
        /// Returns a new color lightened by given <paramref name="amount"/>.
        /// This method leaves alpha channel unaffected.
        /// </summary>
        public TermColor Lighter(double amount)
        {
            return TermColor.Lighten(this, amount);
        }

        /// <summary>
        /// Returns a new color darkened by given <paramref name="amount"/>.
        /// This method leaves alpha channel unaffected.
        /// </summary>
        public static TermColor Darken(TermColor source, double amount)
        {
            return TermColor.Lighten(source, -amount);
        }

        /// <summary>
        /// Returns a new color darkened by given <paramref name="amount"/>.
        /// This method leaves alpha channel unaffected.
        /// </summary>
        public TermColor Darker(double amount)
        {
            return TermColor.Darken(this, amount);
        }

        /// <summary>
        /// Returns a new color saturated by given <paramref name="amount"/>.
        /// This method leaves alpha channel unaffected.
        /// </summary>
        public static TermColor Saturate(TermColor source, double amount)
        {
            return TermColor.FromHSLA(source.H, Math.Max(Math.Min(source.S + amount, 1), 0), source.L, source.A);
        }

        /// <summary>
        /// Returns a new color saturated by given <paramref name="amount"/>.
        /// This method leaves alpha channel unaffected.
        /// </summary>
        public TermColor Saturated(double amount)
        {
            return TermColor.Saturate(this, amount);
        }

        /// <summary>
        /// Returns a new color desaturated by given <paramref name="amount"/>.
        /// This method leaves alpha channel unaffected.
        /// </summary>
        public static TermColor Desaturate(TermColor source, double amount)
        {
            return TermColor.Saturate(source, -amount);
        }

        /// <summary>
        /// Returns a new color desaturated by given <paramref name="amount"/>.
        /// This method leaves alpha channel unaffected.
        /// </summary>
        public TermColor Desaturated(double amount)
        {
            return TermColor.Desaturate(this, amount);
        }

        /// <summary>
        /// Return new grayscaled version based on given color
        /// This method leaves alpha channel unaffected.
        /// </summary>
        public static TermColor Grayscale(TermColor source)
        {
            return Desaturate(source, 1f);
        }

        /// <summary>
        /// Return new grayscaled color based on this color
        /// This method leaves alpha channel unaffected.
        /// </summary>
        public TermColor Grayscaled()
        {
            return Desaturate(this, 1f);
        }

        /// <summary>
        /// Returns a new color which is a result of <paramref name="a"/> * 0.5 + <paramref name="b"/> * 0.5.
        /// </summary>
        public static TermColor Blend(TermColor a, TermColor b)
        {
            return a * .5f + b * .5f;
        }

        /// <summary>
        /// Returns new TermColor instance based on HSL parameters
        /// </summary>
        public static TermColor FromHSL(double h, double s, double l)
        {
            return FromHSLA(h, s, l, 255);
        }

        public static TermColor FromHSLA(double h, double s, double l, byte a)
        {
            double original_h = h;
            h /= 360d;
            double v;
            double r, g, b;
            r = l;   // default to gray
            g = l;
            b = l;
            v = (l <= .5f) ? (l * (1f + s)) : (l + s - l * s);

            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;

                m = l + l - v;
                sv = (v - m) / v;
                h *= 6f;
                sextant = (int)h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;
                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }

            return new TermColor(Convert.ToByte(r * 255), Convert.ToByte(g * 255), Convert.ToByte(b * 255), a, original_h, s, l);
        }

        /// <summary>
        /// Returns a palette of colors for transition between <paramref name="source"/> and <paramref name="destination"/> in N <paramref name="steps"/>
        /// </summary>
        public static IEnumerable<TermColor> Gradient(TermColor source, TermColor destination, int steps)
        {
            int stepA = ((destination.A - source.A) / (steps - 1));
            int stepR = ((destination.R - source.R) / (steps - 1));
            int stepG = ((destination.G - source.G) / (steps - 1));
            int stepB = ((destination.B - source.B) / (steps - 1));

            for (int i = 0; i < steps; i++)
            {
                yield return new TermColor(source.R + (stepR * i), source.G + (stepG * i), source.B + (stepB * i), source.A + (stepA * i));
            }
        }

        /// <summary>
        /// Returns a triad of colors from given source
        /// </summary>
        public static IEnumerable<TermColor> Triad(TermColor source)
        {
            for (int i = 0; i < 3; i++)
            {
                yield return TermColor.FromHSLA((source.H + i * 120) % 360, source.S, source.L, source.A);
            }
        }

        /// <summary>
        /// Returns a tetrad of colors from given source
        /// </summary>
        public static IEnumerable<TermColor> Tetrad(TermColor source)
        {
            for (int i = 0; i < 4; i++)
            {
                yield return TermColor.FromHSLA((source.H + i * 90) % 360, source.S, source.L, source.A);
            }
        }

        /// <summary>
        /// Returns a set of analogous colors
        /// </summary>
        public static IEnumerable<TermColor> Analogous(TermColor source, int size=6)
        {
            TermColor basecolor = source;

            int step = 360/size;

            for (int i = 0; i < size; i++)
            {
                double h = (basecolor.H + i * step) % 360;
                yield return TermColor.FromHSLA(h, basecolor.S, basecolor.L, basecolor.A);
            }
        }

        /// <summary>
        /// Returns complementary color for this color.
        /// This method leaves alpha channel unaffected.
        /// </summary>
        public TermColor Negative()
        {
            return TermColor.Negate(this);
        }

        /// <summary>
        /// Returns complementary color from given source.
        /// This method leaves alpha channel unaffected.
        /// </summary>
        public static TermColor Negate(TermColor source)
        {
            return new TermColor(byte.MaxValue - source.R, byte.MaxValue - source.G, byte.MaxValue - source.B, source.A);
        }
        
        #endregion

        #region Mathematical operators

        /// <summary>
        /// Returns a sum of given colors.
        /// This method leaves alpha channel from color <paramref name="a"/> unaffected.
        /// </summary>
        public static TermColor operator +(TermColor a, TermColor b)
        {
            return new TermColor(a.R + b.R, a.G + b.G, a.B + b.B, a.A);
        }

        /// <summary>
        /// Substracts color <paramref name="b"/> from color <paramref name="a"/>.
        /// This method leaves alpha channel from color <paramref name="a"/> unaffected.
        /// </summary>
        public static TermColor operator -(TermColor a, TermColor b)
        {
            return new TermColor(a.R - b.R, a.G - b.G, a.B - b.B, a.A);
        }

        /// <summary>
        /// Returns color <paramref name="a"/> multiplied by color <paramref name="b"/>.
        /// This method leaves alpha channel from color <paramref name="a"/> unaffected.
        /// </summary>
        public static TermColor operator *(TermColor a, TermColor b)
        {
            return new TermColor(a.R * b.R, a.G * b.G, a.B * b.B, a.A);
        }

        /// <summary>
        /// Returns color <paramref name="a"/> scaled by specified <paramref name="amount"/>.
        /// This method leaves alpha channel unaffected.
        /// </summary>
        public static TermColor operator *(TermColor a, float amount)
        {
            return new TermColor((byte)(a.R * amount), (byte)(a.G * amount), (byte)(a.B * amount), a.A);
        }

        #endregion

        /// <summary>
        /// Returns color in hex string
        /// </summary>
        public override string ToString()
        {
            return String.Format("#{0:X}{1:X}{2:X}{3:X}", R, G, B, A);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Creates a new color from hex string in format of #rrggbb or #rrggbbaa
        /// </summary>
        public static TermColor Parse(string hexString)
        {
            hexString = hexString.Replace("#", string.Empty);
            int r = int.Parse(hexString.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            int g = int.Parse(hexString.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int b = int.Parse(hexString.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            int a = byte.MaxValue;

            if(hexString.Length > 6)
            {
                a = int.Parse(hexString.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }

            return new TermColor(r, g, b, a);
        }
    }
}
