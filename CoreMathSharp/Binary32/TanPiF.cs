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
    /// <inheritdoc cref="StrictMath.TanPi(double)"/>
    public static float TanPi(float x)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float RoundEvenFinite(float x)
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

            float ix = BuiltinRound(x);
            if (Abs(ix - x) == 0.5f)
            {
                float u = ix;
                float v = ix - CopySign(1.0f, x);
                if (Polyfill.TrailingZeroCount(Polyfill.SingleToUInt32Bits(v)) > Polyfill.TrailingZeroCount(Polyfill.SingleToUInt32Bits(u)))
                {
                    ix = v;
                }
            }

            return ix;
        }




        uint ix = Polyfill.SingleToUInt32Bits(x);
        uint e = ix & (0xff << 23);
        if (e > (150 << 23))
        {
            if (e == (0xff << 23))
            {
                if (ix << 9 == 0)
                {
                    return float.NaN;
                }

                return x + x;
            }

            return CopySign(0.0f, x);
        }

        float x4 = 4.0f * x, nx4 = RoundEvenFinite(x4), dx4 = x4 - nx4;
        float ni = RoundEvenFinite(x), zf = x - ni;

        if (dx4 == 0.0f)
        {
            int k = (int)x4;
            if ((k & 1) != 0)
            {
                return CopySign(1.0f, zf);
            }
            k &= 6;
            if (k == 0)
            {
                return CopySign(0.0f, x);
            }
            if (k == 4)
            {
                return -CopySign(0.0f, x);
            }
            if (k == 2)
            {
                return 1.0f / 0.0f;
            }
            return -1.0f / 0.0f;
        }

        ix = Polyfill.SingleToUInt32Bits(zf);
        uint a = ix & (~0u >> 1);

        if (a == 0x3e933802u)
        {
            return CopySign(1.2687946557998657f, zf) + CopySign(2.9802322387695312e-08f, zf);
        }
        if (a == 0x38f26685u)
        {
            return CopySign(0.00036312273005023599f, zf) + CopySign(7.2759576141834259e-12f, zf);
        }

        double z = zf, z2 = z * z;

        ReadOnlySpan<double> cn = [0.78539816339744839, -0.2805387264887832, 0.02201158908691473, -0.00023103959012326923];
        ReadOnlySpan<double> cd = [1, -0.64706113409157673, 0.097314025548005403, -0.0032269805489163333];

        double z4 = z2 * z2;
        double r = (z - z * z2) * ((cn[0] + z2 * cn[1]) + z4 * (cn[2] + z2 * cn[3])) / (((cd[0] + z2 * cd[1]) + z4 * (cd[2] + z2 * cd[3])) * (0.25 - z2));
        return (float)r;
    }
}
