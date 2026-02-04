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
    /// <inheritdoc cref="StrictMath.Sinh(double)"/>
    public static float Sinh(float x)
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



        ReadOnlySpan<double> c = [1, 0.021660849391257477, 0.0002345984913513542, 1.6938658699950235e-06];
        ReadOnlySpan<double> ch = [1, 0.02166084939249829, 0.0002345961982022468, 1.6938509724129055e-06, 9.1725627017026289e-09, 3.973729405780548e-11, 1.4345723178374038e-13];
        ReadOnlySpan<ulong> tb = [0x3fe0000000000000, 0x3fe059b0d3158574, 0x3fe0b5586cf9890f, 0x3fe11301d0125b51, 0x3fe172b83c7d517b, 0x3fe1d4873168b9aa, 0x3fe2387a6e756238, 0x3fe29e9df51fdee1, 0x3fe306fe0a31b715, 0x3fe371a7373aa9cb, 0x3fe3dea64c123422, 0x3fe44e086061892d, 0x3fe4bfdad5362a27, 0x3fe5342b569d4f82, 0x3fe5ab07dd485429, 0x3fe6247eb03a5585, 0x3fe6a09e667f3bcd, 0x3fe71f75e8ec5f74, 0x3fe7a11473eb0187, 0x3fe82589994cce13, 0x3fe8ace5422aa0db, 0x3fe93737b0cdc5e5, 0x3fe9c49182a3f090, 0x3fea5503b23e255d, 0x3feae89f995ad3ad, 0x3feb7f76f2fb5e47, 0x3fec199bdd85529c, 0x3fecb720dcef9069, 0x3fed5818dcfba487, 0x3fedfc97337b9b5f, 0x3feea4afa2a490da, 0x3fef50765b6e4540];
        ReadOnlySpan<float> st = [5.2305432868479330458e+31f, 0.00055894249817356467f, 1.4551915228366852e-11f];

        const double iln2 = 46.166241308446828;


        uint t = Polyfill.SingleToUInt32Bits(x);
        double z = x;
        uint ux = t << 1;
        if (ux > 0x8565a9f8u)
        {
            float sgn = CopySign(2.0f, x);
            if (ux >= 0xff000000u)
            {
                if (ux << 8 != 0)
                {
                    return x + x;
                }
                return x;
            }

            float rr = sgn * 3.4028234663852886e+38f;
            return rr;
        }

        if (ux < 0x7c000000u)
        {
            if (ux <= 0x74250bfeu)
            {
                if (ux < 0x66000000u)
                {
                    return FusedMultiplyAdd(x, Abs(x), x);
                }

                if (ux == Polyfill.SingleToUInt32Bits(st[0]))
                {
                    float sgn = CopySign(1.0f, x);
                    return sgn * st[1] + sgn * st[2];
                }

                return (x * 0.1666666716337204f) * (x * x) + x;
            }

            ReadOnlySpan<double> cp = [0.16666666666666666, 0.0083333333333572308, 0.00019841269076590929, 2.7565149135114762e-06];
            double z2 = z * z, z4 = z2 * z2;
            double res = z + (z2 * z) * ((cp[0] + z2 * cp[1]) + z4 * (cp[2] + z2 * cp[3]));
            return (float)res;
        }

        double a = iln2 * z, ia = RoundEvenFinite(a), h = a - ia, h2 = h * h;
        ulong ja = Polyfill.DoubleToUInt64Bits(ia + 6755399441055744.0);
        long jp = (long)ja, jm = -jp;
        ulong sp = tb[(int)jp & 31] + ((ulong)(jp >> 5) << 52);
        ulong sm = tb[(int)jm & 31] + ((ulong)(jm >> 5) << 52);

        double te = c[0] + h2 * c[2], to = c[1] + h2 * c[3];
        double rp = Polyfill.UInt64BitsToDouble(sp) * (te + h * to);
        double rm = Polyfill.UInt64BitsToDouble(sm) * (te - h * to);
        double r = rp - rm;

        float ub = (float)r, lb = (float)(r - 1.52e-10 * r);
        if (ub != lb)
        {
            const double iln2h = 46.16624128818512, iln2l = 2.026170940661134e-08;

            h = (iln2h * z - ia) + iln2l * z;
            h2 = h * h;
            te = ch[0] + h2 * ch[2] + (h2 * h2) * (ch[4] + h2 * ch[6]);
            to = ch[1] + h2 * (ch[3] + h2 * ch[5]);
            r = Polyfill.UInt64BitsToDouble(sp) * (te + h * to) - Polyfill.UInt64BitsToDouble(sm) * (te - h * to);
            ub = (float)r;
        }

        return ub;
    }
}
