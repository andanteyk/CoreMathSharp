using System;

namespace CoreMathSharp;

public static partial class StrictMathF
{
    /// <inheritdoc cref="StrictMath.Asin(double)"/>
    public static float Asin(float x)
    {
        static float asSpecial(float x)
        {
            uint ax = Polyfill.SingleToUInt32Bits(x) << 1;
            if (ax > (0xffu << 24))
            {
                return x + x;
            }
            return float.NaN;
        }

        static double poly12(double z, ReadOnlySpan<double> c)
        {
            double z2 = z * z, z4 = z2 * z2;
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


        const double pi2 = 1.5707963267948966;
        double xs = x, r;
        uint t = Polyfill.SingleToUInt32Bits(x);
        uint ax = t << 1;

        if (ax > (0x7f << 24))
        {
            return asSpecial(x);
        }
        if (ax < 0x7ec29000u)
        {
            if (ax < 115 << 24)
            {
                return FusedMultiplyAdd(x, 2.9802322387695312e-08f, x);
            }

            ReadOnlySpan<double> b = [1.0000000000000011, 0.16666694674143204, 0.074971125427954172, 0.045817957533670697, 0.0053310089004139846, 0.34410258152367046, -2.6809300420995639, 15.541270760972983, -63.173298334050159, 184.79515144873312, -390.01981668037752, 589.27907809507678, -621.89777643639002, 435.8403729646551, -182.48552714860514, 34.637053328737558];

            double z = xs, z2 = z * z, z4 = z2 * z2, z8 = z4 * z4, z16 = z8 * z8;
            r = z * ((((b[0] + z2 * b[1]) + z4 * (b[2] + z2 * b[3])) + z8 * ((b[4] + z2 * b[5]) + z4 * (b[6] + z2 * b[7]))) +
                z16 * (((b[8] + z2 * b[9]) + z4 * (b[10] + z2 * b[11])) + z8 * ((b[12] + z2 * b[13]) + z4 * (b[14] + z2 * b[15]))));
            float ub = (float)r, lb = (float)(r - z * 9.015999891115456e-10);
            if (ub == lb)
            {
                return ub;
            }
        }
        if (ax < (0x7eu << 24))
        {
            ReadOnlySpan<double> c = [0.16666666666664731, 0.075000000004254955, 0.044642856775806136, 0.030381960865898193, 0.022371723076598973, 0.017360165084156678, 0.01388117521087077, 0.012193412697105537, 0.0064317722535114155, 0.019772599269663224, -0.016582844751635805, 0.032143615203812523];
            double z = xs, z2 = z * z, c0 = poly12(z2, c);
            r = z + (z * z2) * c0;
        }
        else
        {
            if (ax == 0x7e55688au)
            {
                return CopySign(0.72992426156997681f, x) + CopySign(1.4901161193847656e-08f, x);
            }
            if (ax == 0x7e107434u)
            {
                return CopySign(0.56112205982208252f, x) + CopySign(1.4901161193847656e-08f, x);
            }

            double bx = StrictMath.Abs(xs);
            double z = 1.0 - bx;
            double s = StrictMath.Sqrt(z);

            ReadOnlySpan<double> c = [1.4142135623730947, 0.11785113019794026, 0.026516504277464867, 0.0078918173765064673, 0.0026853981502991025, 0.00098884883690508307, 0.00038253952347123667, 0.00015842231966484147, 5.141249514992934e-05, 5.1002363757431448e-05, -1.6635262387371602e-05, 2.1931983490736225e-05];
            r = pi2 - s * poly12(z, c);
            r = StrictMath.CopySign(r, xs);
        }

        return (float)r;
    }
}
