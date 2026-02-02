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
    public static float Tan(float x)
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


        static (double value, int q) rltl(float z)
        {
            double x = z;

            double idl = -3.9447882232973841e-10 * x, idh = 0.63661977276206017 * x, id = RoundEvenFinite(idh);
            return ((idh - id) + idl, (int)id);
        }

        static (double value, int q) rbig(uint u)
        {
            ReadOnlySpan<ulong> ipi = [0xfe5163abdebbc562, 0xdb6295993c439041, 0xfc2757d1f534ddc0, 0xa2f9836e4e441529];

            int e = (int)(u >> 23) & 0xff;
            int i;
            ulong m = (u & (~0u >> 9)) | 1 << 23;

            Uint128 p0 = (Uint128)m * ipi[0];
            Uint128 p1 = (Uint128)m * ipi[1];
            p1 += p0.hi;
            Uint128 p2 = (Uint128)m * ipi[2];
            p2 += p1.hi;
            Uint128 p3 = (Uint128)m * ipi[3];
            p3 += p2.hi;
            ulong p3h = p3.hi, p3l = p3.lo, p2l = p2.lo, p1l = p1.lo;

            long a;
            int k = e - 127, s = k - 23;

            if (s < 64)
            {
                i = (int)(p3h << s | p3l >> -s);
                a = (long)(p3l << s | p2l >> -s);
            }
            else if (s == 64)
            {
                i = (int)p3l;
                a = (long)p2l;
            }
            else
            {
                i = (int)(p3h << s | p2l >> -s);
                a = (long)(p2l << s | p1l >> -s);
            }

            int sgn = (int)u >> 31;
            long sm = a >> 63;
            i -= (int)sm;

            double z = (a ^ sgn) * 5.4210108624275222e-20;
            i = (i ^ sgn) - sgn;
            return (z, i);
        }





        uint t = Polyfill.SingleToUInt32Bits(x);
        int e = (int)(t >> 23) & 0xff;
        int i;
        double z;

        if (e < 127 + 28)
        {
            if (e < 115)
            {
                if (e < 102)
                {
                    return FusedMultiplyAdd(x, Abs(x), x);
                }

                float x2 = x * x;
                return FusedMultiplyAdd(x, 0.3333333432674408f * x2, x);
            }

            (z, i) = rltl(x);
        }
        else if (e < 0xff)
        {
            (z, i) = rbig(t);
        }
        else
        {
            if (t << 9 != 0)
            {
                return x + x;
            }
            return float.NaN;
        }

        double z2 = z * z, z4 = z2 * z2;

        ReadOnlySpan<double> cn = [1.5707963267948966, -0.49720165641032027, 0.026834022769159881, -0.00017660096093977045];
        ReadOnlySpan<double> cd = [1, -1.1389954387488281, 0.1421268437745497, -0.0031314039049681057];
        ReadOnlySpan<double> s = [0, 1];

        double n = cn[0] + z2 * cn[1], n2 = cn[2] + z2 * cn[3];
        n += z4 * n2;
        double d = cd[0] + z2 * cd[1], d2 = cd[2] + z2 * cd[3];
        d += z4 * d2;
        n *= z;

        double s0 = s[i & 1], s1 = s[1 - (i & 1)];
        double r1 = (n * s1 - d * s0) / (n * s0 + d * s1);

        ulong tr = Polyfill.DoubleToUInt64Bits(r1);
        ulong tail = (tr + 7) & (~0ul >> 35);
        if (tail <= 14)
        {
            ReadOnlySpan<float> st = [1.079082727432251f, 1.8670953512191772f, -3.4097189530735692e-16f, 225260880f, 0.23828700184822083f, 4.3587930826721458e-18f, 474309780832256f, 0.34455025196075439f, -3.2680321123916472e-17f, 4512995942072320f, 1.7895326614379883f, 2.1771658420191705e-16f, 9.3166134376399477e+21f, 0.67480170726776123f, 1.7449127529366843e-16f, 6.955935609971147e+25f, -3.9000706672668457f, 8.1730743770115394e-16f, 1.277849588535595e+26f, -0.88550704717636108f, -1.2783292861530832e-16f, 7.1710354546296099e+30f, -1.8912676572799683f, -2.4787918922562627e-16f];

            uint ax = t & (~0u >> 1), sgn = t >> 31;
            for (int j = 0; j < st.Length / 8; j++)
            {
                if (ax == Polyfill.SingleToUInt32Bits(st[j * 3 + 0]))
                {
                    if (sgn != 0)
                    {
                        return -st[j * 3 + 1] - st[j * 3 + 2];
                    }
                    else
                    {
                        return st[j * 3 + 1] + st[j * 3 + 2];
                    }
                }
            }
        }

        return (float)r1;
    }
}
