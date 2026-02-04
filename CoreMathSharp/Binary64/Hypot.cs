using System.Runtime.CompilerServices;

namespace CoreMathSharp;

public static partial class StrictMath
{
    /// <summary>
    /// Computes the hypotenuse given two values representing the lengths of the shorter sides in a right-angled triangle.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    /// <remarks>
    /// Mathematically, returns sqrt(x * x + y * y), but without overflow.
    /// </remarks>
    public static double Hypot(double x, double y)
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double s, double e) fastTwoSum(double x, double y)
        {
            double s = x + y, z = s - x;
            return (s, y - z);
        }

        static double asHypotDenorm(ulong a, ulong b)
        {
            double af = (long)a, bf = (long)b;

            a <<= 1;
            b <<= 1;
            ulong rm = (ulong)Sqrt(af * af + bf * bf);
            long tm = (long)rm << 1;
            long D = (long)(a * a + b * b - (ulong)tm * (ulong)tm);

            while (D > 2 * tm)
            {
                D -= 2 * tm + 1;
                tm++;
            }
            while (D < 0)
            {
                D += 2 * tm - 1;
                tm--;
            }

            bool rb = (int)(tm & 1) != 0;
            bool rb2 = D >= tm;
            bool sb = D != 0;

            rm = (ulong)(tm >> 1);

            if (rb || sb)
            {
                // TODO: Since the rounding mode in C# is always NEAREST, `op == om` should be always true
                double op = 1.0 + 5.5511151231257827e-17, om = 1.0 - 5.5511151231257827e-17;
                if (op == om)
                {
                    if (sb)
                    {
                        rm += rb ? 1ul : 0ul;
                    }
                    else
                    {
                        rm += rm & 1;
                    }
                }
                else if (op > 1.0)
                {
                    rm++;
                }
            }

            ulong xi = rm;
            return Polyfill.UInt64BitsToDouble(xi);
        }

        static double asHypotHard(double x, double y)
        {
            double op = 1.0 + 5.5511151231257827e-17, om = 1.0 - 5.5511151231257827e-17;
            ulong xi = Polyfill.DoubleToUInt64Bits(x), yi = Polyfill.DoubleToUInt64Bits(y);
            ulong bm = (xi & (~0ul >> 12)) | 1ul << 52;
            ulong lm = (yi & (~0ul >> 12)) | 1ul << 52;
            int be = (int)(xi >> 52);
            int le = (int)(yi >> 52);
            ulong ri = Polyfill.DoubleToUInt64Bits(Sqrt(x * x + y * y));

            const int bs = 2;
            ulong rm = (ri & (~0ul >> 12));
            int re = (int)(ri >> 52) - 0x3ff;

            rm |= 1ul << 52;

            for (int i = 0; i < 3; i++)
            {
                if (rm == 1ul << 52)
                {
                    rm = ~0ul >> 11;
                    re--;
                }
                else
                {
                    rm--;
                }
            }

            bm <<= bs;
            ulong m2 = bm * bm;
            int de = be - le;
            int ls = bs - de;

            if (ls >= 0)
            {
                lm <<= ls;
                m2 += lm * lm;
            }
            else
            {
                Uint128 lm2 = (Uint128)lm * lm;
                ls *= 2;
                m2 += (lm2 >> -ls).lo;
                m2 |= (lm2 << (128 + ls)) != 0 ? 1ul : 0ul;
            }

            int k = bs + re;
            long D;
            do
            {
                rm += 1 + ((rm >= (1ul << 53)) ? 1ul : 0ul);
                ulong tm = rm << k, rm2 = tm * tm;
                D = (long)(m2 - rm2);
            } while (D > 0);

            if (D != 0)
            {
                if (op == om)
                {
                    ulong tm = (rm << k) - (1ul << (k - (rm <= (1ul << 53) ? 1 : 0)));
                    D = (long)(m2 - tm * tm);
                    if (D != 0)
                    {
                        rm += (ulong)(D >> 63);
                    }
                    else
                    {
                        rm -= rm & 1;
                    }
                }
                else
                {
                    rm -= (op == 1 ? 1ul : 0ul) << (rm > (1ul << 53) ? 1 : 0);
                }
            }
            if (rm >= (1ul << 53))
            {
                rm >>= 1;
                re++;
            }

            long e = be - 1 + re;
            xi = (ulong)(e << 52) + rm;
            return Polyfill.UInt64BitsToDouble(xi);
        }


        static double asHypotOverflow()
        {
            double z = 1.7976931348623157e+308;
            double f = z + z;
            return f;
        }




        ulong xi = Polyfill.DoubleToUInt64Bits(x), yi = Polyfill.DoubleToUInt64Bits(y);
        ulong emsk = 0x7fful << 52, ex = xi & emsk, ey = yi & emsk;

        x = Abs(x);
        y = Abs(y);

        if (ex == emsk || ey == emsk)
        {
            ulong wx = xi << 1, wy = yi << 1, wm = emsk << 1;
            bool ninf = (wx == wm) ^ (wy == wm);
            bool nqnn = ((wx >> 52) == 0xfff) ^ ((wy >> 52) == 0xfff);
            if (ninf && nqnn)
            {
                return (wx == wm) ? x * x : y * y;
            }
            return x + y;
        }

        double u = Max(x, y), v = Min(x, y);
        ulong xd = Polyfill.DoubleToUInt64Bits(u), yd = Polyfill.DoubleToUInt64Bits(v);
        ey = yd;

        if (ey >> 52 == 0)
        {
            if (yd == 0)
            {
                return Polyfill.UInt64BitsToDouble(xd);
            }
            ex = xd;
            if (ex >> 52 == 0)
            {
                if (ex == 0)
                {
                    return 0.0;
                }
                return asHypotDenorm(ex, ey);
            }
            int nz = Polyfill.LeadingZeroCount(ey);
            ey <<= nz - 11;
            ey &= ~0ul >> 12;
            ey -= (ulong)(nz - 12) << 52;
            ulong t = ey;
            yd = t;
        }

        ulong de = xd - yd;
        if (de > (27ul << 52))
        {
            double r = FusedMultiplyAdd(7.4505805969238281e-09, v, u);
            return r;
        }

        long off = (long)((0x3fful << 52) - (xd & emsk));
        xd += (ulong)off;
        yd += (ulong)off;
        x = Polyfill.UInt64BitsToDouble(xd);
        y = Polyfill.UInt64BitsToDouble(yd);

        double x2 = x * x, dx2 = FusedMultiplyAdd(x, x, -x2);
        double y2 = y * y, dy2 = FusedMultiplyAdd(y, y, -y2);
        double r2 = x2 + y2, ir2 = 0.5 / r2, dr2 = ((x2 - r2) + y2) + (dx2 + dy2);
        double th = Sqrt(r2), rsqrt = th * ir2;
        double dz = dr2 - FusedMultiplyAdd(th, th, -r2), tl = rsqrt * dz;

        (th, tl) = fastTwoSum(th, tl);
        ulong thd = Polyfill.DoubleToUInt64Bits(th), tld = Polyfill.DoubleToUInt64Bits(Abs(tl));
        ex = thd;
        ey = tld;
        ex &= 0x7fful << 52;
        ulong aidr = ey + (0x3feul << 52) - ex;
        ulong mid = (aidr - 0x3c90000000000000 + 16) >> 5;

        if (mid == 0 || aidr < 0x39b0000000000000ul || aidr > 0x3c9fffffffffff80ul)
        {
            thd = Polyfill.DoubleToUInt64Bits(asHypotHard(x, y));
        }
        thd -= (ulong)off;
        if (thd >= 0x7fful << 52)
        {
            return asHypotOverflow();
        }
        return Polyfill.UInt64BitsToDouble(thd);
    }
}
