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
    /// <inheritdoc cref="StrictMath.Cos(double)"/>
    public static float Cos(float x)
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


        static (double z, int q) Rbig(uint u)
        {
            ReadOnlySpan<ulong> ipi = [0xfe5163abdebbc562, 0xdb6295993c439041, 0xfc2757d1f534ddc0, 0xa2f9836e4e441529];
            int e = (int)(u >> 23) & 0xff, i;
            ulong m = (u & ~0u >> 9) | (1u << 23);

            Uint128 p0 = (Uint128)m * ipi[0];
            Uint128 p1 = (Uint128)m * ipi[1]; p1 += p0.hi;
            Uint128 p2 = (Uint128)m * ipi[2]; p2 += p1.hi;
            Uint128 p3 = (Uint128)m * ipi[3]; p3 += p2.hi;
            ulong p3h = p3.hi, p3l = p3.lo, p2l = p2.lo, p1l = p1.lo;

            long a;
            int k = e - 124, s = k - 23;

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
                i = (int)(p3l << s | p2l >> -s);
                a = (long)(p2l << s | p1l >> -s);
            }

            int sgn = (int)u >> 31;
            long sm = a >> 63;
            i -= (int)sm;
            double z = (a ^ sgn) * 5.4210108624275222e-20;
            i = (i ^ sgn) - sgn;
            return (z, i);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double id, int q) Rltl(float z)
        {
            double x = z;
            double idl = -3.1558305786379073e-09 * x, idh = 5.0929581820964813 * x, id = RoundEvenFinite(idh);
            ulong Q = Polyfill.DoubleToUInt64Bits(6755399441055744.0 + id);
            return ((idh - id) + idl, (int)Q);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double id, int q) Rltl0(double x)
        {
            double idh = 5.0929581789406511 * x, id = RoundEvenFinite(idh);
            ulong Q = Polyfill.DoubleToUInt64Bits(6755399441055744.0 + id);
            return (idh - id, (int)Q);
        }

        static float asCosfDatabase(float x, double r)
        {
            ReadOnlySpan<float> st = [4.7123889923095703f, 1.1924880638503055e-08f, -2.2204460492503131e-16f, 2861650809978880f, 0.5339164137840271f, -1.4901161193847656e-08f, 23127222067920896f, 0.48877441883087158f, 7.4505805969238281e-09f, 1.100467763087514e+19f, 0.996410071849823f, 1.4901161193847656e-08f, 1.7269983397793917e+20f, 0.96905797719955444f, -1.4901161193847656e-08f];

            uint t = Polyfill.SingleToUInt32Bits(x);
            uint ax = t & (~0u >> 1);

            for (int i = 0; i < st.Length / 3; i++)
            {
                if (ax == Polyfill.SingleToUInt32Bits(st[i * 3 + 0]))
                {
                    return st[i * 3 + 1] + st[i * 3 + 2];
                }
            }

            return (float)r;
        }




        static float asCosfBig(float x)
        {
            ReadOnlySpan<double> b = [0.019276571095877645, -6.1931032202117844e-05, 7.9587859810943986e-08, -5.4777514393633976e-11];
            ReadOnlySpan<double> a = [0.19634954084936204, -0.0012616486279372187, 2.432025854080733e-06, -2.2318367225754577e-09];
            ReadOnlySpan<double> tb = [1, 0.98078528040323043, 0.92387953251128674, 0.83146961230254524, 0.70710678118654757, 0.55557023301960218, 0.38268343236508978, 0.19509032201612828, 0, -0.19509032201612828, -0.38268343236508978, -0.55557023301960218, -0.70710678118654757, -0.83146961230254524, -0.92387953251128674, -0.98078528040323043, -1, -0.98078528040323043, -0.92387953251128674, -0.83146961230254524, -0.70710678118654757, -0.55557023301960218, -0.38268343236508978, -0.19509032201612828, 0, 0.19509032201612828, 0.38268343236508978, 0.55557023301960218, 0.70710678118654757, 0.83146961230254524, 0.92387953251128674, 0.98078528040323043];


            uint u = Polyfill.SingleToUInt32Bits(x);
            uint ax = u << 1;
            if (ax >= 0xffu << 24)
            {
                if (ax << 8 != 0)
                {
                    return x + x;
                }
                return float.NaN;
            }

            (double z, int ia) = Rbig(u);
            double z2 = z * z, z4 = z2 * z2;
            double aa = (a[0] + z2 * a[1]) + z4 * (a[2] + z2 * a[3]);
            double bb = (b[0] + z2 * b[1]) + z4 * (b[2] + z2 * b[3]);
            double s0 = tb[(ia + 8) & 31], c0 = tb[ia & 31];
            double r = c0 + z * (aa * s0 - bb * (z * c0));

            ulong tr = Polyfill.DoubleToUInt64Bits(r);
            ulong tail = (tr + 6) & (~0ul >> 36);
            if (tail <= 12)
            {
                return asCosfDatabase(x, r);
            }
            return (float)r;
        }





        ReadOnlySpan<double> b = [0.019276571095877645, -6.1931032202117844e-05, 7.9587859810943986e-08, -5.4777514393633976e-11];
        ReadOnlySpan<double> a = [0.19634954084936204, -0.0012616486279372187, 2.432025854080733e-06, -2.2318367225754577e-09];
        ReadOnlySpan<double> tb = [1, 0.98078528040323043, 0.92387953251128674, 0.83146961230254524, 0.70710678118654757, 0.55557023301960218, 0.38268343236508978, 0.19509032201612828, 0, -0.19509032201612828, -0.38268343236508978, -0.55557023301960218, -0.70710678118654757, -0.83146961230254524, -0.92387953251128674, -0.98078528040323043, -1, -0.98078528040323043, -0.92387953251128674, -0.83146961230254524, -0.70710678118654757, -0.55557023301960218, -0.38268343236508978, -0.19509032201612828, 0, 0.19509032201612828, 0.38268343236508978, 0.55557023301960218, 0.70710678118654757, 0.83146961230254524, 0.92387953251128674, 0.98078528040323043];


        uint t = Polyfill.SingleToUInt32Bits(x);
        uint ax = t << 1;
        int ia;
        double z0 = x, z;

        if (ax > 0x99000000u || ax < 0x73000000)
        {
            if (ax < 0x73000000)
            {
                if (ax < 0x66000000u)
                {
                    if (ax == 0)
                    {
                        return 1.0f;
                    }
                    return 1.0f - 2.9802322387695312e-08f;
                }
                return -0.5f * x * x + 1.0f;
            }
            return asCosfBig(x);
        }

        if (ax < 0x82a41896u)
        {
            if (ax == 0x812d97c8u)
            {
                return asCosfDatabase(x, 0.0);
            }
            (z, ia) = Rltl0(z0);
        }
        else
        {
            (z, ia) = Rltl((float)z0);
        }

        double z2 = z * z, z4 = z2 * z2;
        double aa = (a[0] + z2 * a[1]) + z4 * (a[2] + z2 * a[3]);
        double bb = (b[0] + z2 * b[1]) + z4 * (b[2] + z2 * b[3]);
        double c0 = tb[ia & 31], s0 = tb[(ia + 8) & 31];
        double r = c0 + aa * (z * s0) - bb * (z2 * c0);
        return (float)r;
    }
}
