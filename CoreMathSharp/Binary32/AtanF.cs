using System;

namespace CoreMathSharp;

public static partial class StrictMathF
{
    public static float Atan(float x)
    {
        const double pi2 = 1.5707963267948966;

        uint t = Polyfill.SingleToUInt32Bits(x);
        int e = (int)(t >> 23) & 0xff;
        bool gt = e >= 127;
        uint ta = t & 0x7fffffff;

        if (ta >= 0x4c700518u)
        {
            if (ta > 0x7f800000u)
            {
                return x + x;
            }

            return (float)StrictMath.CopySign(pi2, (double)x);
        }
        if (e < 127 - 13)
        {
            if (e < 127 - 25)
            {
                if ((t << 1) == 0)
                {
                    return x;
                }

                float res = FusedMultiplyAdd(-x, Abs(x), x);
                return res;
            }

            return FusedMultiplyAdd(-0.33333333333333331f * x, x * x, x);
        }

        double z = x;
        if (gt)
        {
            z = 1.0 / z;
        }
        double z2 = z * z, z4 = z2 * z2, z8 = z4 * z4;

        ReadOnlySpan<double> cn = [0.33000489885804146, 0.82699362601814941, 0.75366922678127057, 0.30412502065816388, 0.052585465033265374, 0.0030928116297212196, 2.6680447001914062e-05];
        ReadOnlySpan<double> cd = [0.33000489885804141, 0.93699525897082925, 1, 0.4972028591750377, 0.1155090060414157, 0.0109022453539874, 0.00027322693677761577];

        double cn0 = cn[0] + z2 * cn[1];
        double cn2 = cn[2] + z2 * cn[3];
        double cn4 = cn[4] + z2 * cn[5];
        double cn6 = cn[6];

        cn0 += z4 * cn2;
        cn4 += z4 * cn6;
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
        if (!gt)
        {
            return (float)r;
        }

        r = (StrictMath.CopySign(0.0082963267948966187, z) - r) + StrictMath.CopySign(1.5625, z);
        return (float)r;
    }
}
