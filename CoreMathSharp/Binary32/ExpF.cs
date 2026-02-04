using System;

namespace CoreMathSharp;

public static partial class StrictMathF
{
    /// <inheritdoc cref="StrictMath.Exp(double)"/>
    public static float Exp(float x)
    {
        ReadOnlySpan<double> c = [0.69314718055994529, 0.24022650695910072, 0.055504108664026088, 0.0096181291075005358, 0.001333362331326638, 0.00015403602972146417];
        ReadOnlySpan<double> b = [1, 0.69314718052023927, 0.2402288551437867, 0.055504596827996931];
        ReadOnlySpan<ulong> tb = [0x3ff0000000000000, 0x3ff02c9a3e778061, 0x3ff059b0d3158574, 0x3ff0874518759bc8, 0x3ff0b5586cf9890f, 0x3ff0e3ec32d3d1a2, 0x3ff11301d0125b51, 0x3ff1429aaea92de0, 0x3ff172b83c7d517b, 0x3ff1a35beb6fcb75, 0x3ff1d4873168b9aa, 0x3ff2063b88628cd6, 0x3ff2387a6e756238, 0x3ff26b4565e27cdd, 0x3ff29e9df51fdee1, 0x3ff2d285a6e4030b, 0x3ff306fe0a31b715, 0x3ff33c08b26416ff, 0x3ff371a7373aa9cb, 0x3ff3a7db34e59ff7, 0x3ff3dea64c123422, 0x3ff4160a21f72e2a, 0x3ff44e086061892d, 0x3ff486a2b5c13cd0, 0x3ff4bfdad5362a27, 0x3ff4f9b2769d2ca7, 0x3ff5342b569d4f82, 0x3ff56f4736b527da, 0x3ff5ab07dd485429, 0x3ff5e76f15ad2148, 0x3ff6247eb03a5585, 0x3ff6623882552225, 0x3ff6a09e667f3bcd, 0x3ff6dfb23c651a2f, 0x3ff71f75e8ec5f74, 0x3ff75feb564267c9, 0x3ff7a11473eb0187, 0x3ff7e2f336cf4e62, 0x3ff82589994cce13, 0x3ff868d99b4492ed, 0x3ff8ace5422aa0db, 0x3ff8f1ae99157736, 0x3ff93737b0cdc5e5, 0x3ff97d829fde4e50, 0x3ff9c49182a3f090, 0x3ffa0c667b5de565, 0x3ffa5503b23e255d, 0x3ffa9e6b5579fdbf, 0x3ffae89f995ad3ad, 0x3ffb33a2b84f15fb, 0x3ffb7f76f2fb5e47, 0x3ffbcc1e904bc1d2, 0x3ffc199bdd85529c, 0x3ffc67f12e57d14b, 0x3ffcb720dcef9069, 0x3ffd072d4a07897c, 0x3ffd5818dcfba487, 0x3ffda9e603db3285, 0x3ffdfc97337b9b5f, 0x3ffe502ee78b3ff6, 0x3ffea4afa2a490da, 0x3ffefa1bee615a27, 0x3fff50765b6e4540, 0x3fffa7c1819e90d8];

        const double iln2 = 1.4426950408889634, big = 105553116266496;

        uint t = Polyfill.SingleToUInt32Bits(x);
        double z = x, a = iln2 * z;
        ulong u = Polyfill.DoubleToUInt64Bits(a + big);
        uint ux = t << 1;

        if (ux > 0x8562e42eu || ux < 0x6f93813eu)
        {
            if (ux < 0x6f93813eu)
            {
                return (float)(1.0 + z * (1.0 + z * 0.5));
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
            if (t > 0xc2ce8ec0u)
            {
                double y = 1.4012984643248171e-45 + (z + 103.27892990343184) * 1.0108231726433641e-45;
                y = StrictMath.Max(y, 3.5032461608120427e-46);
                float r = (float)y;
                return r;
            }
            if ((t >> 31) == 0 && t > 0x42b17217u)
            {
                float r = 1.7014118346046923e+38f * 1.7014118346046923e+38f;
                return r;
            }
        }

        {
            double ia = big - Polyfill.UInt64BitsToDouble(u), h = a + ia;
            ulong sv = tb[(int)(u & 0x3f)] + ((u >> 6) << 52);
            double h2 = h * h, r = ((b[0] + h * b[1]) + h2 * (b[2] + h * b[3])) * Polyfill.UInt64BitsToDouble(sv);

            float ub = (float)r, lb = (float)(r - r * 1.45e-10);
            if (ub != lb)
            {
                const double iln2h = 1.442695040255785, iln2l = 6.3317841895660438e-10;
                h = (iln2h * z + ia) + iln2l * z;
                double s = Polyfill.UInt64BitsToDouble(sv);
                h2 = h * h;
                double w = s * h;
                r = s + w * ((c[0] + h * c[1]) + h2 * ((c[2] + h * c[3]) + h2 * (c[4] + h * c[5])));
                ub = (float)r;
            }

            return ub;
        }
    }
}
