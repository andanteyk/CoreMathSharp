using System;

namespace CoreMathSharp;

public static partial class StrictMathF
{
    public static float Hypot(float x, float y)
    {
        float ax = Abs(x), ay = Abs(y);
        uint tx = Polyfill.SingleToUInt32Bits(ax), ty = Polyfill.SingleToUInt32Bits(ay);
        if (tx >= (0xffu << 23) || ty >= (0xffu << 23))
        {
            bool snanX = tx > (0xffu << 23) && ((tx >> 22) & 1) == 0;
            bool snanY = ty > (0xffu << 23) && ((ty >> 22) & 1) == 0;
            if (snanX || snanY)
            {
                return x + y;
            }

            if (tx == (0xffu << 23))
            {
                return ax;
            }
            if (ty == (0xffu << 23))
            {
                return ay;
            }
            return ax + ay;
        }

        float at = Max(ax, ay), c;
        ay = Min(ax, ay);

        double xd = at, yd = ay, x2 = xd * xd, y2 = yd * yd, r2 = x2 + y2;
        if (yd < xd * 0.00024414061044808477)
        {
            c = FusedMultiplyAdd(0.0001220703125f, ay, at);
            return c;
        }

        double r = StrictMath.Sqrt(r2);
        ulong t = Polyfill.DoubleToUInt64Bits(r);
        c = (float)r;

        if (t > 0x47efffffe0000000ul)
        {
            return c;
        }
        if (((t + 1) & 0xfffffff) > 2)
        {
            return c;
        }

        double cd = c;
        if ((cd * cd - x2) - y2 == 0.0)
        {
            return c;
        }
        double ir2 = 0.5 / r2, dr2 = (x2 - r2) + y2;
        double rs = r * ir2, dz = dr2 - StrictMath.FusedMultiplyAdd(r, r, -r2), dr = rs * dz;
        double rh = r + dr, rl = dr + (r - rh);
        t = Polyfill.DoubleToUInt64Bits(rh);
        if ((t & 0xfffffff) == 0)
        {
            if (rl > 0.0)
            {
                t++;
            }
            if (rl < 0.0)
            {
                t--;
            }
        }

        return (float)Polyfill.UInt64BitsToDouble(t);
    }
}
