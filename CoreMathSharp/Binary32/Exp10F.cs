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
    /// <inheritdoc cref="StrictMath.Exp10(double)"/>
    public static float Exp10(float x)
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


        ReadOnlySpan<double> c = [0.69314718055994529, 0.24022650695910072, 0.055504108664026088, 0.0096181291075005358, 0.001333362331326638, 0.00015403602972146417];
        ReadOnlySpan<double> b = [1, 0.021660849391257477, 0.0002345984913513542, 1.6938658699950235e-06];
        ReadOnlySpan<ulong> tb = [0x3ff0000000000000, 0x3ff059b0d3158574, 0x3ff0b5586cf9890f, 0x3ff11301d0125b51, 0x3ff172b83c7d517b, 0x3ff1d4873168b9aa, 0x3ff2387a6e756238, 0x3ff29e9df51fdee1, 0x3ff306fe0a31b715, 0x3ff371a7373aa9cb, 0x3ff3dea64c123422, 0x3ff44e086061892d, 0x3ff4bfdad5362a27, 0x3ff5342b569d4f82, 0x3ff5ab07dd485429, 0x3ff6247eb03a5585, 0x3ff6a09e667f3bcd, 0x3ff71f75e8ec5f74, 0x3ff7a11473eb0187, 0x3ff82589994cce13, 0x3ff8ace5422aa0db, 0x3ff93737b0cdc5e5, 0x3ff9c49182a3f090, 0x3ffa5503b23e255d, 0x3ffae89f995ad3ad, 0x3ffb7f76f2fb5e47, 0x3ffc199bdd85529c, 0x3ffcb720dcef9069, 0x3ffd5818dcfba487, 0x3ffdfc97337b9b5f, 0x3ffea4afa2a490da, 0x3fff50765b6e4540];
        ReadOnlySpan<float> ex = [10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000, 1000000000, 10000000000];
        const double iln102 = 106.30169903639559, iln102h = 3.3219280913472176, iln102l = 3.5401447880558664e-09;


        uint t = Polyfill.SingleToUInt32Bits(x);
        double z = x;
        uint ux = t << 1;

        if (ux > 0x84344134u || ux < 0x72adf1c6u)
        {
            if (ux < 0x72adf1c6u)
            {
                return (float)(1.0 + z * (2.3025850929940459 + z * (2.6509490552391992 + z * 2.034678592293476)));
            }
            if (ux >= 0xffu << 24)
            {
                if (ux > 0xffu << 24)
                {
                    return x + x;
                }
                ReadOnlySpan<float> ir = [float.PositiveInfinity, 0.0f];
                return ir[(int)(t >> 31)];
            }
            if (t > 0xc23369f4u)
            {
                double y = 1.4012984643248171e-45 + (z + 44.8534693539332) * 2.3275063689815626e-45;
                y = StrictMath.Max(y, 3.5032461608120427e-46);
                float r1 = (float)y;
                return r1;
            }
            if (t < 0x80000000u)
            {
                float r1 = 1.7014118346046923e+38f * 1.7014118346046923e+38f;
                return r1;
            }
        }

        if (t << 12 == 0)
        {
            int k = (int)(t >> 20) - 1016;
            if (k <= 26)
            {
                int bt = 1 << k, msk = 0x7551101;
                if ((bt & msk) != 0)
                {
                    return ex[Polyfill.PopCount((uint)(msk & (bt - 1)))];
                }
            }
        }


        double a = iln102 * z, ia = RoundEvenFinite(a), h = a - ia;
        long ja = (long)ia;
        ulong sv = tb[(int)(ja & 0x1f)] + ((ulong)(ja >> 5) << 52);
        double h2 = h * h, r = ((b[0] + h * b[1]) + h2 * (b[2] + h * b[3])) * Polyfill.UInt64BitsToDouble(sv);

        float ub = (float)r, lb = (float)(r - r * 1.45e-10);
        if (ub != lb)
        {
            h = (iln102h * z - ia * 0.03125) + iln102l * z;
            double s = Polyfill.UInt64BitsToDouble(sv);
            h2 = h * h;
            double w = s * h;
            r = s + w * ((c[0] + h * c[1]) + h2 * ((c[2] + h * c[3]) + h2 * (c[4] + h * c[5])));
            ub = (float)r;
        }

        return ub;
    }
}
