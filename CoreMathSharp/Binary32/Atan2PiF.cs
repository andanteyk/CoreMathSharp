using System;
using System.Runtime.CompilerServices;

namespace CoreMathSharp;

public static partial class StrictMathF
{
    public static float Atan2Pi(float y, float x)
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


        ReadOnlySpan<double> cn = [0.31830988618379069, 0.7979546675063276, 0.72750794754484616, 0.29372174016793834, 0.050820403623979257, 0.0029915422604631704, 2.5834878288675861e-05];
        ReadOnlySpan<double> cd = [1, 2.8401818546688959, 3.0322609083249099, 1.5083284691366383, 0.35061013533424623, 0.03311601651598859, 0.00083070468185660116];
        ReadOnlySpan<double> m = [0.0, 1.0];
        ReadOnlySpan<double> off = [0.0, 0.5, 1.0, 0.5, -0.0, -0.5, -1.0, -0.5];
        ReadOnlySpan<float> sgnf = [1.0f, -1.0f];
        ReadOnlySpan<double> sgn = [1.0, -1.0];


        uint tx = Polyfill.SingleToUInt32Bits(x), ty = Polyfill.SingleToUInt32Bits(y);
        uint ux = tx, uy = ty, ax = ux & (~0u >> 1), ay = uy & (~0u >> 1);

        if (ay >= (0xff << 23) || ax >= (0xff << 23))
        {
            if (ay > (0xff << 23))
            {
                return x + y;
            }
            if (ax > (0xff << 23))
            {
                return x + y;
            }
            bool yinf = ay == (0xff << 23), xinf = ax == (0xff << 23);
            if (yinf && xinf)
            {
                if (ux >> 31 != 0)
                {
                    return 0.75f * sgnf[(int)(uy >> 31)];
                }
                else
                {
                    return 0.25f * sgnf[(int)(uy >> 31)];
                }
            }
            if (xinf)
            {
                if (ux >> 31 != 0)
                {
                    return sgnf[(int)(uy >> 31)];
                }
                else
                {
                    return 0.0f * sgnf[(int)(uy >> 31)];
                }
            }
            if (yinf)
            {
                return 0.5f * sgnf[(int)(uy >> 31)];
            }
        }

        if (ay == 0)
        {
            if ((ay | ax) == 0)
            {
                int ii = (int)((uy >> 31) * 4 + (ux >> 31) * 2);
                return (float)off[ii];
            }
            if (ux >> 31 == 0)
            {
                return 0.0f * sgnf[(int)(uy >> 31)];
            }
        }

        if (ax == ay)
        {
            ReadOnlySpan<float> s = [0.25f, 0.75f, -0.25f, -0.75f];
            int ii = (int)((uy >> 31) * 2 + (ux >> 31));
            return s[ii];
        }

        int gt = ay > ax ? 1 : 0;
        int i = (int)((uy >> 31) * 4 + (ux >> 31) * 2) + gt;

        double zx = x, zy = y;
        double z = (m[gt] * zx + m[1 - gt] * zy) / (m[gt] * zy + m[1 - gt] * zx);
        double r = cn[0], z2 = z * z;
        z *= sgn[gt];

        if (z2 > 5.5511151231257827e-17)
        {
            double z4 = z2 * z2, z8 = z4 * z4;
            double cn0 = r + z2 * cn[1];
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

        r = z * r + off[i];
        ulong res = Polyfill.DoubleToUInt64Bits(r);

        if ((res << 1) > 0x6d40000000000000ul && ((res + 8) & 0xfffffff) <= 16)
        {
            if (ax == ay)
            {
                ReadOnlySpan<double> off2 = [0.25, 0.75, -0.25, -0.75];
                r = off2[(int)((uy >> 31) * 2 + (ux >> 31))];
            }
            else
            {
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

                ReadOnlySpan<double> c = [0.31830988618379069, -1.9678676678365386e-17, -0.1061032953945969, 6.5595655747053874e-18, 0.063661977236758135, -1.1625142324443452e-18, -0.045472840883398667, 1.6330933027767818e-19, 0.035367765131532274, -9.8697238731890759e-19, -0.028937262380343491, -9.6627448515008273e-20, 0.024485375860256887, -1.149537084009613e-18, -0.021220659078143779, 1.7059737274086658e-18, 0.018724110938995286, 8.8151093889895395e-19, -0.016753151736008654, -1.3202448604516062e-18, 0.015157611897126696, 6.75954372683546e-19, -0.01383954589266639, 8.0785266464125347e-19, 0.012732297425997631, 1.7919213416430043e-19, -0.011788699545460008, 6.6603721065115205e-19, 0.010973559311688615, 4.52760332181136e-19, -0.01025740427988246, 5.7158526053731179e-19, 0.0096091045982405095, -3.6666946471079842e-19, -0.0089863552590235263, -3.20433976550847e-19, 0.0083271159975979551, 8.6186948963733862e-19, -0.0075514681765014319, -8.492307901900414e-20, 0.0065853437637122614, 2.2245145527371671e-19, -0.0054054532914321511, -3.141319316546961e-19, 0.0040799324056043397, 1.6362318030205914e-19, -0.0027652579189205322, 1.6581771247399889e-19, 0.0016436307978093597, -5.3430775632975888e-21, -0.00083617426396587007, -1.9497106729570389e-20, 0.00035446593346812673, -5.6383244034139747e-21, -0.00012121521420485887, 5.1681775579281682e-21, 3.2011526076572051e-05, -1.1919997370531597e-21, -6.1101254037461421e-06, 1.3338248853591681e-22, 7.4856048056903009e-07, -9.5915121959841022e-24, -4.4129183432426896e-08, 1.7860214694149171e-24];

                var (ph, pl) = polydd(z2h, z2l, 32, c);
                zh *= sgn[gt];
                zl *= sgn[gt];
                (ph, pl) = muldd(zh, zl, ph, pl);

                double sh = ph + off[i], sl = ((off[i] - sh) + ph) + pl;
                float rf1 = (float)sh;
                double th = rf1, dh = sh - th, tm = dh + sl;
                r = th + tm;

                ulong d = Polyfill.DoubleToUInt64Bits(r - th);
                if (d << 12 == 0)
                {
                    double ad = StrictMath.Abs(Polyfill.UInt64BitsToDouble(d)), am = StrictMath.Abs(tm);
                    if (ad > am)
                    {
                        r -= Polyfill.UInt64BitsToDouble(d) * 0.0009765625;
                    }
                    if (ad < am)
                    {
                        r += Polyfill.UInt64BitsToDouble(d) * 0.0009765625;
                    }
                }
            }
        }

        float rf = (float)r;
        return rf;
    }
}
