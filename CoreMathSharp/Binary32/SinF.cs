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
    /// <inheritdoc cref="StrictMath.Sin(double)"/>
    public static float Sin(float x)
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



        static (double value, int q) rbig(uint u)
        {
            ReadOnlySpan<ulong> ipi = [0xfe5163abdebbc562, 0xdb6295993c439041, 0xfc2757d1f534ddc0, 0xa2f9836e4e441529];
            int e = (int)(u >> 23) & 0xff, i;

            ulong m = (u & (~0u >> 9)) | 1 << 23;

            Uint128 p0 = (Uint128)m * ipi[0];
            Uint128 p1 = (Uint128)m * ipi[1];
            p1 += p0.hi;
            Uint128 p2 = (Uint128)m * ipi[2];
            p2 += p1.hi;
            Uint128 p3 = (Uint128)m * ipi[3];
            p3 += p2.hi;

            ulong p3h = p3.hi, p3l = p3.lo, p2l = p2.lo, p1l = p1.lo;

            ulong a;
            int k = e - 124, s = k - 23;

            if (s < 64)
            {
                i = (int)(p3h << s | p3l >> -s);
                a = p3l << s | p2l >> -s;
            }
            else if (s == 64)
            {
                i = (int)p3l;
                a = p2l;
            }
            else
            {
                i = (int)(p3l << s | p2l >> -s);
                a = p2l << s | p1l >> -s;
            }

            int sgn = (int)u;
            sgn >>= 31;
            long sm = (long)a >> 63;

            i -= (int)sm;

            double z = ((long)a ^ sgn) * 5.4210108624275222e-20;
            i = (i ^ sgn) - sgn;

            return (z, i);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double value, int q) rltl(float z)
        {
            double x = z;
            double idl = -3.1558305786379073e-09 * x, idh = 5.0929581820964813 * x, id = RoundEvenFinite(idh);
            ulong Q = Polyfill.DoubleToUInt64Bits(6755399441055744.0 + id);
            return ((idh - id) + idl, (int)Q);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double value, int q) rltl0(double x)
        {
            double idh = 5.0929581789406511 * x, id = RoundEvenFinite(idh);
            ulong Q = Polyfill.DoubleToUInt64Bits(6755399441055744.0 + id);
            return (idh - id, (int)Q);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float addSign(float x, float rh, float rl)
        {
            float sgn = CopySign(1.0f, x);
            return sgn * rh + sgn * rl;
        }

        static float asSinfDatabase(float x, double r)
        {
            ReadOnlySpan<float> st = [9830.3984375f, -0.34761324524879456f, -7.4505805969238281e-09f, 0.72992426156997681f, 0.6668131947517395f, -1.4901161193847656e-08f, 1.3086903095245361f, 0.96584641933441162f, -1.4901161193847656e-08f, 9.4247779846191406f, -2.384975950064927e-08f, -4.4408920985006262e-16f];

            uint t = Polyfill.SingleToUInt32Bits(x);
            uint ax = t & (~0u >> 1);

            for (int i = 0; i < st.Length / 3; i++)
            {
                if (ax == Polyfill.SingleToUInt32Bits(st[i * 3 + 0]))
                {
                    return addSign(x, st[i * 3 + 1], st[i * 3 + 2]);
                }
            }

            return (float)r;
        }

        static float asSinfBig(float x, ReadOnlySpan<double> a, ReadOnlySpan<double> b, ReadOnlySpan<double> tb)
        {
            uint t = Polyfill.SingleToUInt32Bits(x);
            uint ax = t << 1;

            if (ax >= 0xffu << 24)
            {
                if (ax << 8 != 0)
                {
                    return x + x;
                }
                return 0.0f / 0.0f;
            }

            (double z, int ia) = rbig(t);
            double z2 = z * z, z4 = z2 * z2;

            double aa = (a[0] + z2 * a[1]) + z4 * (a[2] + z2 * a[3]);
            double bb = (b[0] + z2 * b[1]) + z4 * (b[2] + z2 * b[3]);
            double s0 = tb[ia & 31], c0 = tb[(ia + 8) & 31];
            double r = s0 + z * (aa * c0 - bb * (z * s0));
            return (float)r;
        }




        ReadOnlySpan<double> b = [0.019276571095877645, -6.1931032202117844e-05, 7.9587859810943986e-08, -5.4777514393633976e-11];
        ReadOnlySpan<double> a = [0.19634954084936204, -0.0012616486279372187, 2.432025854080733e-06, -2.2318367225754577e-09];
        ReadOnlySpan<double> tb = [0, 0.19509032201612828, 0.38268343236508978, 0.55557023301960218, 0.70710678118654757, 0.83146961230254524, 0.92387953251128674, 0.98078528040323043, 1, 0.98078528040323043, 0.92387953251128674, 0.83146961230254524, 0.70710678118654757, 0.55557023301960218, 0.38268343236508978, 0.19509032201612828, 0, -0.19509032201612828, -0.38268343236508978, -0.55557023301960218, -0.70710678118654757, -0.83146961230254524, -0.92387953251128674, -0.98078528040323043, -1, -0.98078528040323043, -0.92387953251128674, -0.83146961230254524, -0.70710678118654757, -0.55557023301960218, -0.38268343236508978, -0.19509032201612828];



        uint t = Polyfill.SingleToUInt32Bits(x);
        uint ax = t << 1;
        int ia;
        double z0 = x, z;

        if (ax > 0x99000000u || ax < 0x73000000u)
        {
            if (ax < 0x73000000u)
            {
                if (ax < 0x66000000u)
                {
                    if (ax == 0)
                    {
                        return x;
                    }

                    float res = FusedMultiplyAdd(-x, Abs(x), x);
                    return res;
                }

                return (-0.1666666716337204f * x) * (x * x) + x;
            }

            return asSinfBig(x, a, b, tb);
        }
        if (ax < 0x822d97c8u)
        {
            if (ax == 0x7e75b8a2u || ax == 0x7f4f0654u)
            {
                return asSinfDatabase(x, 0.0);
            }
            (z, ia) = rltl0(z0);
        }
        else
        {
            if (ax == 0x8c333330u)
            {
                return asSinfDatabase(x, 0.0);
            }
            (z, ia) = rltl((float)z0);
        }

        double z2 = z * z, z4 = z2 * z2;

        double aa = (a[0] + z2 * a[1]) + z4 * (a[2] + z2 * a[3]);
        double bb = (b[0] + z2 * b[1]) + z4 * (b[2] + z2 * b[3]);
        double s0 = tb[ia & 31], c0 = tb[(ia + 8) & 31];
        double r = s0 + aa * (z * c0) - bb * (z2 * s0);
        return (float)r;
    }
}
