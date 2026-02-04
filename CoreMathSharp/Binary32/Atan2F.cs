using System;
using System.Runtime.CompilerServices;

namespace CoreMathSharp;

public static partial class StrictMathF
{
    /// <inheritdoc cref="StrictMath.Atan2(double, double)"/>
    public static float Atan2(float y, float x)
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double ch, double l) muldd(double xh, double xl, double ch, double cl)
        {
            double ahlh = ch * xl, alhh = cl * xh, ahhh = ch * xh, ahhl = StrictMath.FusedMultiplyAdd(ch, xh, -ahhh);
            ahhl += alhh + ahlh;
            ch = ahhh + ahhl;
            double l = (ahhh - ch) + ahhl;
            return (ch, l);
        }

        static (double ch, double l) polydd(double xh, double xl, int n, ReadOnlySpan<double> c)
        {
            int i = n - 1;
            double ch = c[i * 2 + 0], cl = c[i * 2 + 1];

            while (--i >= 0)
            {
                (ch, cl) = muldd(xh, xl, ch, cl);
                double th = ch + c[i * 2 + 0], tl = (c[i * 2 + 0] - th) + ch;
                ch = th;
                cl += tl + c[i * 2 + 1];
            }

            return (ch, cl);
        }

        static float atan2fTiny(float y, float x)
        {
            double dy = y, dx = x;
            double z = dy / dx;
            double e = StrictMath.FusedMultiplyAdd(-z, x, y);

            const double c = -0.33333333333333331;
            double zz = z * z;
            double cz = c * z;
            e = e / dx + cz * zz;

            ulong t = Polyfill.DoubleToUInt64Bits(z);
            if ((t & 0xffffffful) == 0)
            {
                if (z * e > 0)
                {
                    t++;
                }
                else
                {
                    t--;
                }
            }
            float res = (float)Polyfill.UInt64BitsToDouble(t);
            return res;
        }



        ReadOnlySpan<double> cn = [1, 2.5068485213355651, 2.2855336234350774, 0.92275406111120506, 0.15965700667756133, 0.0093982071883745005, 8.1162663838090543e-05];
        ReadOnlySpan<double> cd = [1, 2.8401818546688959, 3.0322609083249099, 1.5083284691366383, 0.35061013533424623, 0.03311601651598859, 0.00083070468185660116];
        ReadOnlySpan<double> m = [0.0, 1.0];

        const double Pi = 3.1415926535897931;
        const double Pi2 = 1.5707963267948966;
        const double Pi2L = 6.123233995736766e-17;

        ReadOnlySpan<double> off = [0.0, Pi2, Pi, Pi2, -0.0, -Pi2, -Pi, -Pi2];
        ReadOnlySpan<double> offl = [0.0, Pi2L, 2 * Pi2L, Pi2L, -0.0, -Pi2L, -2 * Pi2L, -Pi2L];
        ReadOnlySpan<double> sgn = [1.0, -1.0];

        uint tx = Polyfill.SingleToUInt32Bits(x), ty = Polyfill.SingleToUInt32Bits(y);
        uint ux = tx, uy = ty, ax = ux & (~0u >> 1), ay = uy & (~0u >> 1);
        if (ay >= 0xff << 23 || ax >= 0xff << 23)
        {
            if (ay > 0xff << 23)
            {
                return x + y;
            }
            if (ax > 0xff << 23)
            {
                return x + y;
            }

            uint yinf = ay == (0xff << 23) ? 1u : 0u, xinf = ax == (0xff << 23) ? 1u : 0u;
            if (yinf != 0 && xinf != 0)
            {
                if (ux >> 31 != 0)
                {
                    return (float)(2.3561944901923448 * sgn[(int)(uy >> 31)]);
                }
                else
                {
                    return (float)(0.78539816339744828 * sgn[(int)(uy >> 31)]);
                }
            }

            if (xinf != 0)
            {
                if (ux >> 31 != 0)
                {
                    return (float)(Pi * sgn[(int)(uy >> 31)]);
                }
                else
                {
                    return (float)(0.0 * sgn[(int)(uy >> 31)]);
                }
            }
            if (yinf != 0)
            {
                return (float)(Pi2 * sgn[(int)(uy >> 31)]);
            }
        }

        if (ay == 0)
        {
            if (ax == 0)
            {
                int ii = (int)((uy >> 31) * 4 + (ux >> 31) * 2);
                if (ux >> 31 != 0)
                {
                    return (float)(off[ii] + offl[ii]);
                }
                else
                {
                    return (float)off[ii];
                }
            }
            if (ux >> 31 == 0)
            {
                return (float)(0.0 * sgn[(int)(uy >> 31)]);
            }
        }

        int gt = ay > ax ? 1 : 0;
        int i = (int)((uy >> 31) * 4 + (ux >> 31) * 2) + gt;

        double zx = x, zy = y;
        double z = (m[gt] * zx + m[1 - gt] * zy) / (m[gt] * zy + m[1 - gt] * zx);
        double r;
        int d = (int)ax - (int)ay;

        if (d < 27 << 23 && d > -(27 << 23))
        {
            double z2 = z * z, z4 = z2 * z2, z8 = z4 * z4;
            double cn0 = cn[0] + z2 * cn[1];
            double cn2 = cn[2] + z2 * cn[3];
            double cn4 = cn[4] + z2 * cn[5];
            double cn6 = cn[6];
            cn0 += z4 * cn2;
            cn4 += z4 * cn6;
            cn0 += z8 * cn4;

            double cd0 = cd[0] + z2 * cd[1];
            double cd2 = cd[2] + z2 * cd[3];
            double cd4 = cd[4] + z2 * cd[5];
            double cd6 = cd[6];
            cd0 += z4 * cd2;
            cd4 += z4 * cd6;
            cd0 += z8 * cd4;

            r = cn0 / cd0;
        }
        else
        {
            r = 1.0;
        }

        z *= sgn[gt];
        r = z * r + off[i];
        ulong res = Polyfill.DoubleToUInt64Bits(r);
        if (((res + 8) & 0xfffffff) <= 16)
        {
            if (ay < ax && ((ax - ay) >> 23 >= 25))
            {
                return atan2fTiny(y, x);
            }

            double zh, zl;
            if (gt == 0)
            {
                zh = zy / zx;
                zl = StrictMath.FusedMultiplyAdd(zh, -zx, zy) / zx;
            }
            else
            {
                zh = zx / zy;
                zl = StrictMath.FusedMultiplyAdd(zh, -zy, zx) / zy;
            }

            var (z2h, z2l) = muldd(zh, zl, zh, zl);


            ReadOnlySpan<double> c = [1, -9.9993713909802762e-27, -0.33333333333333331, -1.8503696081891694e-17, 0.20000000000000001, -1.1109570448589454e-17, -0.14285714285714285, -6.906377603268134e-18, 0.11111111111111104, -6.0498620754704096e-19, -0.090909090909087401, -4.813186107306423e-18, 0.07692307692296789, 6.0635784048294865e-18, -0.066666666664230045, 5.3573683924228454e-19, 0.058823529370947883, -3.0234229312063074e-18, -0.052631578418319884, 7.1219063142300131e-19, 0.047619042181978474, 3.2606056478649954e-18, -0.043478215705419529, 2.1474249882470664e-18, 0.039999692056834395, -1.4496021955960109e-18, -0.037035291887394496, 8.3946147130053678e-19, 0.034474453317332822, 3.5732167404770099e-19, -0.032224585930579239, -6.0591993063025197e-19, 0.030187892413408284, 8.5503597778482721e-19, -0.028231467664296316, 8.6457562390298748e-19, 0.026160406443643782, -1.1013132206776929e-18, -0.023723636947114012, 5.8205312635420101e-19, 0.0206884675894418, 8.7895316349397806e-19, -0.016981732349686016, 9.5194871038492798e-19, 0.012817485672589527, -7.7422393466476828e-19, -0.0086873139633617431, -5.6307156457055604e-19, 0.0051636184396118149, 3.2806307955849715e-19, -0.0026269189247960299, -1.3737775104430079e-19, 0.0011135875725313154, -6.6187460764317516e-20, -0.00038080882644929778, 1.5620808802045265e-20, 0.00010056717515235685, -5.1946273311009068e-21, -1.919552508092125e-05, 8.2028906555295314e-22, 2.3516721065233102e-06, -1.3883775330103416e-22, -1.3863591848022874e-07, -1.0947593470915086e-23];

            var (ph, pl) = polydd(z2h, z2l, 32, c);
            zh *= sgn[gt];
            zl *= sgn[gt];
            (ph, pl) = muldd(zh, zl, ph, pl);

            double sh = ph + off[i], sl = ((off[i] - sh) + ph) + pl + offl[i];
            float rf1 = (float)sh;
            double th = rf1, dh = sh - th, tm = dh + sl;
            ulong tth = Polyfill.DoubleToUInt64Bits(th);
            if (th + th * 8.6736173798840355e-19 == th - th * 8.6736173798840355e-19)
            {
                tth &= 0x7fful << 52;
                tth -= 24ul << 52;
                if (StrictMath.Abs(tm) > Polyfill.UInt64BitsToDouble(tth))
                {
                    tm *= 1.25;
                }
                else
                {
                    tm *= 0.75;
                }
            }
            r = th + tm;
        }

        float rf = (float)r;
        return rf;
    }
}
