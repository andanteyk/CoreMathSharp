using System;
using System.Runtime.CompilerServices;

namespace CoreMathSharp;

public static partial class StrictMathF
{
    /// <inheritdoc cref="StrictMath.Acos(double)"/>
    public static float Acos(float x)
    {
        static float asSpecial(float x)
        {
            float pih = 3.1415927410125732f, pil = -5.9604644775390625e-08f;

            uint t = Polyfill.SingleToUInt32Bits(x);
            if (t == 0x7fu << 23)
            {
                return 0.0f;
            }
            if (t == 0x17fu << 23)
            {
                return pih + pil;
            }

            uint ax = t << 1;
            if (ax > (0xffu << 24))
            {
                return x + x;
            }

            return 0.0f / 0.0f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double poly12(double z, ReadOnlySpan<double> c)
        {
            double z2 = z * z;
            double z4 = z2 * z2;

            double c0 = c[0] + z * c[1];
            double c2 = c[2] + z * c[3];
            double c4 = c[4] + z * c[5];
            double c6 = c[6] + z * c[7];
            double c8 = c[8] + z * c[9];
            double c10 = c[10] + z * c[11];

            c0 += c2 * z2;
            c4 += c6 * z2;
            c8 += z2 * c10;
            c0 += z4 * (c4 + z4 * c8);
            return c0;
        }


        double pi2 = 1.5707963267948966;
        ReadOnlySpan<double> o = [0.0, 3.1415926535897931];
        double xs = x;
        double r;
        uint t = Polyfill.SingleToUInt32Bits(x);
        uint ax = t << 1;

        if (ax >= 0x7fu << 24)
        {
            return asSpecial(x);
        }
        if (ax < 0x7ec2a1dcu)
        {
            ReadOnlySpan<double> b = [0.99999999972205611, 0.16666753055233149, 0.074919539383817041, 0.047534405138862854, -0.024905344107261872, 0.66988898180361689, -5.003757071019054, 27.026426908343559, -103.66551324982036, 288.04495822181497, -580.91218490636027, 842.69255408719835, -857.28682388830748, 581.05677607632458, -235.92908248702702, 43.515672212468452];

            if (ax < 0x40000000u)
            {
                float pi2h = 1.5707963705062866f, pi2l = -4.3711388286737929e-08f;
                return pi2h + pi2l;
            }

            double z = xs, z2 = z * z, z4 = z2 * z2, z8 = z4 * z4, z16 = z8 * z8;
            r = z * ((((b[0] + z2 * b[1]) + z4 * (b[2] + z2 * b[3])) + z8 * ((b[4] + z2 * b[5]) + z4 * (b[6] + z2 * b[7]))) +
                z16 * (((b[8] + z2 * b[9]) + z4 * (b[10] + z2 * b[11])) + z8 * ((b[12] + z2 * b[13]) + z4 * (b[14] + z2 * b[15]))));
            float ub = (float)(1.5707963270725467 - r);
            float lb = (float)(1.5707963265172467 - r);
            if (ub == lb)
            {
                return ub;
            }
        }
        if (ax < 0x7eu << 24)
        {
            ReadOnlySpan<double> c = [0.16666666666664731, 0.075000000004254955, 0.044642856775806136, 0.030381960865898193, 0.022371723076598973, 0.017360165084156678, 0.01388117521087077, 0.012193412697105537, 0.0064317722535114155, 0.019772599269663224, -0.016582844751635805, 0.032143615203812523];

            if (t == 0x328885a3u)
            {
                return 1.5707963705062866f + 2.9802322387695312e-08f;
            }
            if (t == 0x39826222u)
            {
                return 1.5705476999282837f + 2.9802322387695312e-08f;
            }

            double x2 = xs * xs;
            r = (pi2 - xs) - (xs * x2) * poly12(x2, c);
        }
        else
        {
            ReadOnlySpan<double> c = [1.4142135623730947, 0.11785113019794026, 0.026516504277464867, 0.0078918173765064673, 0.0026853981502991025, 0.00098884883690508307, 0.00038253952347123667, 0.00015842231966484147, 5.141249514992934e-05, 5.1002363757431448e-05, -1.6635262387371602e-05, 2.1931983490736225e-05];

            double bx = StrictMath.Abs(xs);
            double z = 1.0 - bx;
            double s = StrictMath.CopySign(StrictMath.Sqrt(z), xs);
            r = o[(int)(t >> 31)] + s * poly12(z, c);
        }

        return (float)r;
    }
}
