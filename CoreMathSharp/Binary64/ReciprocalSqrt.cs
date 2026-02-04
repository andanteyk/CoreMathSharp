namespace CoreMathSharp;

public static partial class StrictMath
{
    /// <summary>
    /// Computes the reciprocal square root of a value.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    /// <remarks>
    /// Mathematically, returns 1 / sqrt(x).
    /// </remarks>
    public static double ReciprocalSqrt(double x)
    {

        static double asRsqrtRefine(double rf, double a)
        {
            ulong ir = Polyfill.DoubleToUInt64Bits(rf), ia = Polyfill.DoubleToUInt64Bits(a);
            if (ia < 1ul << 52)
            {
                int nz = Polyfill.LeadingZeroCount(ia);
                ia <<= nz - 11;
                ia &= ~0ul >> 12;
                int e = nz - 12;
                ia |= (ulong)e << 52;
            }
            if (ia << 11 != 1ul << 63)
            {
                int e = (int)(ia >> 52) & 1;
                ulong rm, am;
                rm = (ir << 11 | 1ul << 63) >> 11;
                am = ((ia & (~0ul >> 12)) | 1ul << 52) << (5 - e);

                Uint128 rt = (Uint128)rm * am;
                ulong rth = rt.hi, rtl = rt.lo;
                Uint128 rrt = (Uint128)rtl * rm;
                ulong t0 = rrt.lo, t1 = rrt.hi + rth * rm;
                rrt = (Uint128)t1 << 64 | t0;

                long s = (long)(rrt >> 127).lo, dd = 1 - 2 * s;
                Uint128 rts = ((rt << 1) ^ (ulong)(-s)) + (ulong)s;

                Uint128 prrt;
                ulong am2 = am << 1, am20 = 0 - am;

                do
                {
                    ir -= (ulong)dd;
                    prrt = rrt;
                    am20 += am2;
                    Uint128 tt = rts - am20;
                    rrt -= tt;
                } while ((prrt ^ rrt) >> 127 == 0);

                ir += (rrt >> 127) != 0 ? 0 : (ulong)dd;
                rrt = (rrt >> 127) != 0 ? rrt : prrt;

                {
                    rm = (ir << 11 | 1ul << 63) >> 11;
                    rt = (Uint128)rm * am;
                    rrt += am >> 2;
                    rrt += rt;
                    ulong inc = (rrt >> 127).lo;
                    ir += inc;
                }

                rf = Polyfill.UInt64BitsToDouble(ir);
            }

            return rf;
        }




        ulong ix = Polyfill.DoubleToUInt64Bits(x);
        double r;
        if (ix < 1ul << 52)
        {
            if (ix != 0)
            {
                r = Sqrt(x) / x;
            }
            else
            {
                return 1.0 / 0.0;
            }
        }
        else if (ix >= 0x7fful << 52)
        {
            if (ix << 1 == 0)
            {
                return 1.0 / -0.0;
            }
            if (ix > 0xfff0000000000000ul)
            {
                return x + x;
            }
            if (ix >> 63 != 0)
            {
                return double.NaN;
            }
            if (ix << 12 == 0)
            {
                return 0.0;
            }
            return x + x;
        }
        else
        {
            if (ix > 0x7fd000000000000ul)
            {
                r = (4.0 / x) * (0.25 * Sqrt(x));
            }
            else
            {
                r = (1.0 / x) * Sqrt(x);
            }
        }

        double rx = r * x, drx = FusedMultiplyAdd(r, x, -rx);
        double h = FusedMultiplyAdd(r, rx, -1.0) + r * drx, dr = (r * 0.5) * h;
        double rf = r - dr;
        dr -= r - rf;

        ulong idr = Polyfill.DoubleToUInt64Bits(dr), ir = Polyfill.DoubleToUInt64Bits(rf);

        ulong aidr = (idr & (~0ul >> 1)) - (ir & (0x7fful << 52)) + (0x3feul << 52);
        ulong mid = (aidr - 0x3c90000000000000 + 16) >> 5;
        if (mid == 0 || aidr < 0x39b0000000000000 || aidr > 0x3c9fffffffffff80)
        {
            rf = asRsqrtRefine(rf, x);
        }

        return rf;
    }
}
