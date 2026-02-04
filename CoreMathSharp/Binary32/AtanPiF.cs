using System;

namespace CoreMathSharp;

public static partial class StrictMathF
{
    /// <inheritdoc cref="StrictMath.AtanPi(double)"/>
    public static float AtanPi(float x)
    {
        uint t = Polyfill.SingleToUInt32Bits(x);
        int e = (int)(t >> 23) & 0xff;
        bool gt = e >= 127;

        if (e > 127 + 24)
        {
            float f = CopySign(0.5f, x);
            if (e == 0xff)
            {
                if ((t << 9) != 0)
                {
                    return x + x;
                }
                return f;
            }

            if (Abs(x) >= 2.7078809278823703e+37f)
            {
                return f - CopySign(1.4901161193847656e-08f, x);
            }
            else
            {
                return f - 0.31830987334251404f / x;
            }
        }

        double z = x;
        if (e < 127 - 13)
        {
            double sx = z * 0.31830988618379069;
            if (e < 127 - 25)
            {
                return (float)sx;
            }
            return (float)(sx - (0.33333333333333331 * sx) * (z * z));
        }

        uint ax = t & (~0u >> 1);
        if (ax == 0x3fa267ddu)
        {
            return CopySign(0.28753668069839478f, x) - CopySign(2.7755575615628914e-17f, x);
        }
        if (ax == 0x3f693531u)
        {
            return CopySign(0.23518063127994537f, x) + CopySign(3.7252902984619141e-09f, x);
        }
        if (ax == 0x3f800000u)
        {
            return CopySign(0.25f, x);
        }

        if (gt)
        {
            z = 1.0 / z;
        }
        double z2 = z * z, z4 = z2 * z2, z8 = z4 * z4;

        ReadOnlySpan<double> cn = [0.31830988618379064, 0.72506207550861268, 0.57978440400608933, 0.19347317070584699, 0.024698250108119251, 0.00080630154326152479];
        ReadOnlySpan<double> cd = [1, 2.6111830231477096, 2.4918407653440666, 1.0590480183430666, 0.19415473041607811, 0.012196596718179518, 0.00011321825378267113];

        double cn0 = cn[0] + z2 * cn[1];
        double cn2 = cn[2] + z2 * cn[3];
        double cn4 = cn[4] + z2 * cn[5];
        cn0 += z4 * cn2;
        cn0 += z8 * cn4;
        cn0 *= z;

        double cd0 = cd[0] + z2 * cd[1];
        double cd2 = cd[2] + z2 * cd[3];
        double cd4 = cd[4] + z2 * cd[5];
        double cd6 = cd[6];
        cd0 += z4 * cd2;
        cd4 += z4 * cd6;
        cd0 += z8 * cd4;

        double r = cn0 / cd0;
        if (gt)
        {
            r = StrictMath.CopySign(0.5, z) - r;
        }

        return (float)r;
    }
}
