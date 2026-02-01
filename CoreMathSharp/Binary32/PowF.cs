using System;
using System.Runtime.CompilerServices;

#if NETCOREAPP3_0_OR_GREATER
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
#endif

namespace CoreMathSharp;

public static partial class StrictMathF
{
    public static float Pow(float x, float y)
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double RoundEvenFinite(double x)
        {
#if NETCOREAPP3_0_OR_GREATER
            if (Avx.IsSupported)
            {
                return Avx.RoundToNearestInteger(Vector256.CreateScalarUnsafe(x)).ToScalar();
            }
            if (AdvSimd.IsSupported)
            {
                return AdvSimd.RoundToNearestScalar(Vector64.CreateScalarUnsafe(x)).ToScalar();
            }
            if (Sse41.IsSupported)
            {
                return Sse41.RoundToNearestIntegerScalar(Vector128.CreateScalarUnsafe(x)).ToScalar();
            }
#endif

            double ix = StrictMath.BuiltinRound(x);
            if (StrictMath.Abs(ix - x) == 0.5)
            {
                double u = ix;
                double v = ix - StrictMath.CopySign(1.0, x);
                if (Polyfill.TrailingZeroCount(Polyfill.DoubleToUInt64Bits(v)) > Polyfill.TrailingZeroCount(Polyfill.DoubleToUInt64Bits(u)))
                {
                    ix = v;
                }
            }

            return ix;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool isSignalingF(float x)
        {
            uint u = Polyfill.SingleToUInt32Bits(x);
            u ^= 0x00400000;
            return (u & 0x7fffffff) > 0x7fc00000;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double h, double l) muldd(double xh, double xl, double ch, double cl)
        {
            double ahlh = ch * xl, alhh = cl * xh, ahhh = ch * xh, ahhl = StrictMath.FusedMultiplyAdd(ch, xh, -ahhh);
            ahhl += alhh + ahlh;
            ch = ahhh + ahhl;
            return (ch, (ahhh - ch) + ahhl);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double h, double l) mulddd(double xh, double xl, double ch)
        {
            double ahlh = ch * xl, ahhh = ch * xh, ahhl = StrictMath.FusedMultiplyAdd(ch, xh, -ahhh);
            ahhl += ahlh;
            ch = ahhh + ahhl;
            return (ch, (ahhh - ch) + ahhl);
        }

        static (double h, double l) polydd(double xh, double xl, int n, ReadOnlySpan<double> c)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool isInt(float y0)
        {
            uint wy = Polyfill.SingleToUInt32Bits(y0);
            int ey = (int)((wy >> 23) & 0xff) - 127, s = ey + 9;
            if (ey >= 0)
            {
                if (s >= 32)
                {
                    return true;
                }
                return (wy << s) == 0;
            }
            return (wy << 1) == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool isOdd(float y0)
        {
            uint wy = Polyfill.SingleToUInt32Bits(y0);
            int ey = (int)((wy >> 23) & 0xff) - 127, s = ey + 9, odd = 0;
            if (ey >= 0)
            {
                if (s < 32 && (wy << s) == 0)
                {
                    odd = (int)(wy >> (32 - s)) & 1;
                }

                if (s == 32)
                {
                    odd = (int)(wy & 1);
                }
            }
            return odd != 0;
        }

        static bool isExact(float x, float y)
        {
            uint v = Polyfill.SingleToUInt32Bits(x), w = Polyfill.SingleToUInt32Bits(y);

            if ((v << 1) != 0x7f000000 && (w << (32 - 16)) != 0)
            {
                return false;
            }

            if ((v << 1) == 0x7f000000)
            {
                return true;
            }

            ReadOnlySpan<uint> xmax = [0, 0xffffff, 4095, 255, 63, 27, 15, 9, 7, 5, 5, 3, 3, 3, 3, 3];

            if (y >= 0.0f && isInt(y))
            {
                uint m = v & 0x7fffff;
                int e = (int)((v << 1) >> 24) - 0x96;
                if (e >= -149)
                {
                    m |= 0x800000;
                }
                else
                {
                    e++;
                }

                int t = Polyfill.TrailingZeroCount(m);
                m >>= t;
                e += t;

                if (y == 0.0f || y == 1.0f)
                {
                    return true;
                }
                if (m == 1)
                {
                    return -149 <= y * e && y * e < 128;
                }

                if (y < 0 || 15 < y)
                {
                    return false;
                }

                int yInt = (int)y;
                if (m > xmax[yInt])
                {
                    return false;
                }

                ulong my = m * m;
                for (int i = 2; i < yInt; i++)
                {
                    my *= m;
                }

                t = 32 - Polyfill.LeadingZeroCount(m);

                int ez = e * yInt + t;
                if (ez <= -149 || 128 < ez)
                {
                    return false;
                }

                return e * yInt >= -149;
            }

            {
                uint n = w & 0x7fffff;
                int f = (int)((w << 1) >> 24) - 0x96;
                if (f >= -149)
                {
                    n |= 0x800000;
                }
                else
                {
                    f++;
                }

                int t = Polyfill.TrailingZeroCount(n);
                n >>= t;
                f += t;

                uint m = v & 0x7fffff;
                int e = (int)((v << 1) >> 24) - 0x96;
                if (e >= -149)
                {
                    m |= 0x800000;
                }
                else
                {
                    e++;
                }

                t = Polyfill.TrailingZeroCount(m);
                m >>= t;
                e += t;

                if (y < 0)
                {
                    int ez;
                    if (m != 1)
                    {
                        return false;
                    }

                    if (f >= 0)
                    {
                        ez = ((e >= 0) ? -(e << f) : (-e << f)) * (int)n;
                    }
                    else
                    {
                        t = Polyfill.TrailingZeroCount((uint)e);
                        if (-f > t)
                        {
                            return false;
                        }
                        ez = (-e >> (-f)) * (int)n;
                    }

                    return -149 <= ez && ez < 128;
                }

                while (f++ != 0)
                {
                    if ((e & 1) != 0)
                    {
                        return false;
                    }

                    e /= 2;
                    float dm = (float)m;
                    float s = BuiltinRound(Sqrt(dm));
                    if (s * s != dm)
                    {
                        return false;
                    }

                    m = (uint)s;
                }

                if (m > 1)
                {
                    if (15 < n)
                    {
                        return false;
                    }
                    if (m > xmax[(int)n])
                    {
                        return false;
                    }
                }

                uint my = m, n0 = n;
                while (n0-- > 1)
                {
                    my *= m;
                }

                t = 32 - Polyfill.LeadingZeroCount(my);

                return -149 <= e * (int)n && e * (int)n + t <= 128;
            }
        }

        static float asPowfAccurate2(float x0, float y0, bool isExact)
        {
            ReadOnlySpan<double> o = [1.0, 2.0];
            ReadOnlySpan<double> ch = [2.8853900817779268, 4.071054748191002e-17, 0.96179669392597555, 5.0577616098239652e-17, 0.57707801635558531, 5.2552074957089844e-17, 0.41219858311113239, 1.2966716928545934e-17, 0.32059889797532548, 2.2721007160373763e-17, 0.26230818925246924, -1.3180082845309455e-17, 0.22195308322397042, -7.0373041121025102e-18, 0.19235933776779732, 1.3451966316321192e-17, 0.16972889712828465, -4.8923563015061318e-18, 0.15185945002402543, 1.1034764462533595e-18, 0.13749878152691522, -1.0236531880234422e-17, 0.12347055433477307, -2.2899638320795317e-18, 0.13806280141791244, 8.1220545391699306e-18];
            ReadOnlySpan<double> ce = [1, 6.210306603644812e-30, 0.69314718055994529, 2.3190468138467075e-17, 0.24022650695910072, -9.4939312572070923e-18, 0.055504108664821583, -3.1658222912778202e-18, 0.0096181291076284769, 2.8324649708472296e-19, 0.0013333558146428443, 1.392811665254468e-20, 0.00015403530393381609, 1.1765991431639673e-20, 1.5252733804059841e-05, -8.0442948550066915e-22, 1.3215486790144314e-06, -6.293372509344185e-23, 1.0178086009239703e-07, -1.3006186993534895e-24, 7.0549116207969337e-09, -1.659037271615654e-25, 4.4455382718682324e-10, -5.2967182610893153e-28, 2.5678436021925767e-11, 6.132224146744111e-28, 1.3691488868804127e-12, -2.8921457069199386e-29, 6.7787151063505117e-14, -6.1746754242368079e-30, 3.1324315691939719e-15, 1.9136583918642863e-31, 1.3594238037092167e-16, -8.2381249732358202e-33, 5.5427716400763788e-18, -4.787321441390354e-35];

            double x = x0, y = y0;
            ulong t = Polyfill.DoubleToUInt64Bits(x);
            int e = ((int)(t >> 52) & 0x7ff) - 0x3ff;

            t &= ~0ul >> 12;
            int k = t > 0x6a09e667f3bcdul ? 1 : 0;
            e += k;
            t |= 0x3fful << 52;
            x = Polyfill.UInt64BitsToDouble(t);

            double xm = x - o[k], xp = x + o[k], zh = xm / xp, zl = StrictMath.FusedMultiplyAdd(zh, -xp, xm) / xp;
            var (z2h, z2l) = muldd(zh, zl, zh, zl);
            (z2h, z2l) = polydd(z2h, z2l, 13, ch);
            (zh, zl) = muldd(zh, zl, z2h, z2l);
            (zh, zl) = mulddd(zh, zl, y);

            double ey = e * y, eh = ey + zh, el = ((ey - eh) + zh) + zl, ee = RoundEvenFinite(eh);
            eh -= ee;
            (eh, el) = polydd(eh, el, 18, ce);

            ulong r = (ulong)(0x3ff + (long)ee) << 52;
            uint ty = Polyfill.SingleToUInt32Bits(y0);
            int et = ((int)(ty >> 23) & 0xff) - 0x7f;

            uint kk = (8 + et >= 0) ? ty << (8 + et) : ty >> (-8 - et);
            bool isint = ((int)kk << 1 | et >> 31) == 0 || et >= 23;
            ulong ll = Polyfill.DoubleToUInt64Bits(el), lh = Polyfill.DoubleToUInt64Bits(eh);

            if (((ll >> (6 * 4 - 1)) & ((1 << 29) - 1)) == ((1 << 29) - 1))
            {
                if (eh < 1)
                {
                    if (el >= 5.5511151231257827e-17)
                    {
                        el -= 1.1102230246251565e-16;
                        eh += 1.1102230246251565e-16;
                    }
                    else if (el <= -5.5511151231257827e-17)
                    {
                        el += 1.1102230246251565e-16;
                        eh -= 1.1102230246251565e-16;
                    }
                }
                else
                {
                    if (el >= 1.1102230246251565e-16)
                    {
                        el -= 2.2204460492503131e-16;
                        eh += 2.2204460492503131e-16;
                    }
                    else if (el <= -1.1102230246251565e-16)
                    {
                        el += 2.2204460492503131e-16;
                        eh -= 2.2204460492503131e-16;
                    }
                }
            }
            else if (((ll >> (6 * 4 - 1)) & ((1 << 29) - 1)) == 0)
            {
                if (el > 0)
                {
                    if (eh < 1)
                    {
                        if (el >= 1.1102230246251565e-16)
                        {
                            el -= 1.1102230246251565e-16;
                            eh += 1.1102230246251565e-16;
                        }
                    }
                    else
                    {
                        if (el >= 2.2204460492503131e-16)
                        {
                            el -= 2.2204460492503131e-16;
                            eh += 2.2204460492503131e-16;
                        }
                    }
                }
                else
                {
                    if (eh < 1)
                    {
                        if (el <= -1.1102230246251565e-16)
                        {
                            el += 1.1102230246251565e-16;
                            eh -= 1.1102230246251565e-16;
                        }
                    }
                    else
                    {
                        if (el <= -2.2204460492503131e-16)
                        {
                            el += 2.2204460492503131e-16;
                            eh -= 2.2204460492503131e-16;
                        }
                    }
                }
            }

            ll = Polyfill.DoubleToUInt64Bits(el);
            lh = Polyfill.DoubleToUInt64Bits(eh);
            if ((lh & 0xfffffff) == 0)
            {
                if (StrictMath.Abs(Polyfill.UInt64BitsToDouble(ll)) > 4.0389678347315804e-28)
                {
                    if (el < 0)
                    {
                        lh--;
                        eh = Polyfill.UInt64BitsToDouble(lh);
                    }
                    else
                    {
                        lh++;
                        eh = Polyfill.UInt64BitsToDouble(lh);
                    }
                }
            }

            eh *= Polyfill.UInt64BitsToDouble(r);
            el *= Polyfill.UInt64BitsToDouble(r);

            if (isint && kk != 0)
            {
                eh = StrictMath.CopySign(eh, x0);
            }

            float res = (float)eh;
            return res;
        }






        ReadOnlySpan<double> ix = [1, 0.96969696969608776, 0.9411764705873793, 0.91428571428696159, 0.88888888889050577, 0.86486486486683134, 0.84210526316019241, 0.8205128205154324, 0.80000000000291038, 0.78048780487733893, 0.76190476190822665, 0.7441860465114587, 0.72727272727206582, 0.71111111110803904, 0.69565217391209444, 0.68085106382932281, 0.66666666666424135, 0.65306122448964743, 0.63999999999941792, 0.6274509803915862, 0.61538461538293632, 0.60377358490222832, 0.59259259259124519, 0.58181818181765266, 0.57142857142753201, 0.56140350877103629, 0.55172413792752195, 0.54237288135482231, 0.53333333333284827, 0.52459016393549973, 0.5161290322575951, 0.50793650793639245, 0.5];
        ReadOnlySpan<double> lix = [0, 0, -0.04443359375, 3.9474390234438854e-05, -0.08740234375, -6.049750165153175e-05, -0.12890625, -0.00037676694299827008, -0.169921875, -3.1264396881159154e-06, -0.208984375, -0.00046899062566947312, -0.248046875, 0.0001193615603508767, -0.28515625, -0.00024596885765590962, -0.322265625, 0.00033753011788614611, -0.357421875, -0.00013012961939581666, -0.392578125, 0.00026070222780032859, -0.42578125, -0.00048350470242596976, -0.458984375, -0.00044724363860937968, -0.4921875, 0.00033440366409270264, 0.4765625, -0.00012445605898105753, 0.4453125, 9.8648321378535202e-05, 0.4140625, 0.00097499927359532461, 0.384765625, 0.00052453088446375424, 0.35546875, 0.0006750602239631808, 0.328125, -0.00055034197280771316, 0.298828125, 0.00073215685497146914, 0.271484375, 0.00059517042860004891, 0.2451171875, -4.6896667488531014e-06, 0.21875, -0.00010971352597172757, 0.1923828125, 0.00026226543977164556, 0.1669921875, 0.00011779833296210864, 0.1416015625, 0.00041744236324301479, 0.1171875, 0.00016945063520646333, 0.09326171875, -0.00015231435983065282, 0.0693359375, -7.3275059933998606e-05, 0.0458984375, -9.4747888187332295e-05, 0.022705078125, 1.4998374755498776e-05, 0, 0];

        double xx = x, yy = y;
        ulong tx = Polyfill.DoubleToUInt64Bits(xx), ty = Polyfill.DoubleToUInt64Bits(yy);

        if (tx << 1 == 0x3fful << 53)
        {
            if (tx >> 63 != 0)
            {
                if ((ty << 1) > 0x7fful << 53)
                {
                    return y + y;
                }
                if (isInt(y))
                {
                    return isOdd(y) ? x : -x;
                }
                return (x - x) / (x - x);
            }
            return isSignalingF(y) ? x + y : x;
        }

        if (ty << 1 == 0)
        {
            return isSignalingF(x) ? x + y : 1.0f;
        }
        if (ty << 1 >= 0x7fful << 53)
        {
            if (tx << 1 > 0x7fful << 53)
            {
                return x + y;
            }
            if (ty << 1 == 0x7fful << 53)
            {
                if (((tx << 1) < 0x3fful << 53) ^ (ty >> 63 != 0))
                {
                    return 0.0f;
                }
                else
                {
                    return float.PositiveInfinity;
                }
            }
            return x + y;
        }

        if (tx >= 0x7fful << 52)
        {
            if (tx << 1 == 0x7fful << 53)
            {
                if (!isOdd(y))
                {
                    x = Abs(x);
                }
                if (ty >> 63 != 0)
                {
                    return 1.0f / x;
                }
                else
                {
                    return x;
                }
            }
            if (tx << 1 > 0x7fful << 53)
            {
                return x + x;
            }
            if (tx > 0x7fful << 52)
            {
                if (!isInt(y) && x != 0.0f)
                {
                    return (x - x) / (x - x);
                }
            }
        }

        if (tx << 1 == 0)
        {
            if (ty >> 63 != 0)
            {
                if (isOdd(y))
                {
                    return 1.0f / CopySign(0.0f, x);
                }
                else
                {
                    return 1.0f / 0.0f;
                }
            }
            else
            {
                if (isOdd(y))
                {
                    return CopySign(1.0f, x) * 0.0f;
                }
                else
                {
                    return 0.0f;
                }
            }
        }


        ulong m = tx & (~0ul >> 12);
        int e = (int)((tx >> 52) & 0x7ff) - 0x3ff;
        int j = (int)((m + (1ul << (52 - 6))) >> (52 - 5)), k = j > 13 ? 1 : 0;
        e += k;

        ulong xd = m | (0x3fful << 52);
        double z = StrictMath.FusedMultiplyAdd(Polyfill.UInt64BitsToDouble(xd), ix[j], -1.0);

        ReadOnlySpan<double> c = [1.4426950408889634, -0.72134752044448169, 0.4808983469635712, -0.36067376022317404, 0.28853899623008594, -0.24044915938489397, 0.20617758474822143, -0.18041517056759179];
        double z2 = z * z, z4 = z2 * z2;
        double c6 = c[6] + z * c[7];
        double c4 = c[4] + z * c[5];
        double c2 = c[2] + z * c[3];
        double c0 = c[0] + z * c[1];

        c0 += z2 * c2;
        c4 += z2 * c6;
        c0 += z4 * c4;

        double l = z * c0 - lix[j * 2 + 1];
        yy *= 16.0;
        double zt = (e - lix[j * 2 + 0]) * yy;
        z = l * yy + zt;

        if (z > 2048)
        {
            if (isOdd(y))
            {
                return CopySign(1.7014118346046923e+38f, x) * 1.7014118346046923e+38f;
            }
            else
            {
                return 1.7014118346046923e+38f * 1.7014118346046923e+38f;
            }
        }
        if (z < -2400)
        {
            if (isOdd(y))
            {
                return CopySign(1.1754943508222875e-38f, x) * 1.1754943508222875e-38f;
            }
            else
            {
                return 1.1754943508222875e-38f * 1.1754943508222875e-38f;
            }
        }
        if (StrictMath.Abs(z) < 1.4901161193847656e-08)
        {
            return (float)(1.0 + z);
        }

        double ia = StrictMath.BuiltinFloor(z), h = StrictMath.FusedMultiplyAdd(l, yy, zt - ia);

        ReadOnlySpan<double> ce = [0.043321698784995886, 0.00093838479282008368, 1.3550807712983854e-05, 1.4676119301623784e-07, 1.2713094157155389e-09, 9.3824389539780747e-12];
        ReadOnlySpan<double> tb = [1, 1.0442737824274138, 1.0905077326652577, 1.1387886347566916, 1.189207115002721, 1.241857812073484, 1.2968395546510096, 1.3542555469368927, 1.4142135623730951, 1.4768261459394993, 1.5422108254079407, 1.6104903319492543, 1.681792830507429, 1.7562521603732995, 1.8340080864093424, 1.9152065613971474];

        int il = (int)ia, jl = il & 0xf, el = il - jl;
        el >>= 4;
        double s = tb[jl];
        ulong su = ((ulong)el + 0x3fful) << 52;
        s *= Polyfill.UInt64BitsToDouble(su);

        double h2 = h * h;
        c0 = ce[0] + h * ce[1];
        c2 = ce[2] + h * ce[3];
        c4 = ce[4] + h * ce[5];
        c0 += h2 * (c2 + h2 * c4);

        double w = s * h;
        ulong rr = Polyfill.DoubleToUInt64Bits(s + w * c0);
        ulong off = 468;

        if (((rr + off) & 0xfffffff) <= 2 * off)
        {
            return asPowfAccurate2(x, y, isExact(x, y));
        }

        int et = ((int)(ty >> 52) & 0x7ff) - 0x3ff;
        ulong kk = (et >= -11) ? ty << (11 + et) : ty >> (-11 - et);

        if (kk << 1 == 0 && kk != 0)
        {
            rr = Polyfill.DoubleToUInt64Bits(StrictMath.CopySign(Polyfill.UInt64BitsToDouble(rr), xx));
        }

        float res = (float)Polyfill.UInt64BitsToDouble(rr);
        return res;
    }
}
