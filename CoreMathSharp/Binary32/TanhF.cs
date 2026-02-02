using System;

namespace CoreMathSharp;

public static partial class StrictMathF
{
    public static float Tanh(float x)
    {
        double z = x;
        uint t = Polyfill.SingleToUInt32Bits(x);
        uint ux = t;
        int e = (int)(ux >> 23) & 0xff;

        if (e == 0xff)
        {
            if (ux << 9 != 0)
            {
                return x + x;
            }

            ReadOnlySpan<float> ir = [1.0f, -1.0f];
            return ir[(int)(ux >> 31)];
        }

        if (e < 115)
        {
            if (e < 102)
            {
                if (ux << 1 == 0)
                {
                    return x;
                }

                float res = FusedMultiplyAdd(-x, Abs(x), x);
                return res;
            }

            float x2 = x * x;
            return FusedMultiplyAdd(x, -0.3333333432674408f * x2, x);
        }

        if ((ux << 1) > (0x41102cb3u << 1))
        {
            return CopySign(1.0f, x) - CopySign(2.9802322387695312e-08f, x);
        }

        double z2 = z * z, z4 = z2 * z2, z8 = z4 * z4;

        ReadOnlySpan<double> cn = [1, 0.14869591254532963, 0.00551287098907202, 7.653349704714027e-05, 4.4724281332217524e-07, 1.0666590627970085e-09, 8.3520936325383444e-13, 9.3766458598849877e-17];
        ReadOnlySpan<double> cd = [1, 0.48202924587866269, 0.032855952948627039, 0.00072620566435421243, 6.5102966654485567e-06, 2.4619801106746077e-08, 3.5204157099784045e-11, 1.2726168760182741e-14];

        double n0 = cn[0] + z2 * cn[1], n2 = cn[2] + z2 * cn[3], n4 = cn[4] + z2 * cn[5], n6 = cn[6] + z2 * cn[7];
        n0 += z4 * n2;
        n4 += z4 * n6;
        n0 += z8 * n4;

        double d0 = cd[0] + z2 * cd[1], d2 = cd[2] + z2 * cd[3], d4 = cd[4] + z2 * cd[5], d6 = cd[6] + z2 * cd[7];
        d0 += z4 * d2;
        d4 += z4 * d6;
        d0 += z8 * d4;

        double r = z * n0 / d0;
        return (float)r;
    }
}
