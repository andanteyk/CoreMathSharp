using System;

namespace CoreMathSharp;

public static partial class StrictMathF
{
    public static float Exp10M1(float x)
    {
        ReadOnlySpan<double> c = [0.043321698784995886, 0.00093838479282008368, 1.3550807712983854e-05, 1.4676119301623784e-07, 1.2713094157155389e-09, 9.3824389539780747e-12];
        ReadOnlySpan<double> tb = [1, 1.0442737824274138, 1.0905077326652577, 1.1387886347566916, 1.189207115002721, 1.241857812073484, 1.2968395546510096, 1.3542555469368927, 1.4142135623730951, 1.4768261459394993, 1.5422108254079405, 1.6104903319492543, 1.681792830507429, 1.7562521603732995, 1.8340080864093424, 1.9152065613971474];
        ReadOnlySpan<float> q = [3.4028234663852886e+38f, 3.4028234663852886e+38f, -1f, 1.4901161193847656e-08f];

        const double iln10h = 3.3219280913472176 * 16, iln10l = 3.5401447880558664e-09 * 16;


        uint t = Polyfill.SingleToUInt32Bits(x);
        double z = x;
        uint ux = t, ax = ux & (~0u >> 1);

        if (ux > 0xc0f0d2f1u)
        {
            if (ax > 0xffu << 23)
            {
                return x + x;
            }
            return ux == 0xff800000 ? q[1 * 2 + 0] : q[1 * 2 + 0] + q[1 * 2 + 1];
        }
        else if (ax > 0x421a209au)
        {
            if (ax > 0xffu << 23)
            {
                return x + x;
            }
            return q[0 * 2 + 0] + q[0 * 2 + 1];
        }
        else if (ax < 0x3d89c604u)
        {
            double z2 = z * z, r;

            if (ax < 0x3d1622fbu)
            {
                if (ax < 0x3c8b76a3u)
                {
                    if (ax < 0x3bcced04u)
                    {
                        if (ax < 0x3acf33ebu)
                        {
                            if (ax < 0x395a966bu)
                            {
                                if (ax < 0x36fe4a4bu)
                                {
                                    if (ax < 0x32407f39u)
                                    {
                                        if (ax < 0x245e5bd9u)
                                        {
                                            r = 2.3025850929940459;
                                        }
                                        else
                                        {
                                            if (ux == 0x2c994b7bu)
                                            {
                                                return 1.003213657979618e-11f - 8.0779356694631609e-28f;
                                            }
                                            r = 2.3025850929940459 + z * 2.6509490552391992;
                                        }
                                    }
                                    else
                                    {
                                        if (ux == 0xb6fa215bu)
                                        {
                                            return -1.7164389646495692e-05f + 3.3881317890172014e-21f;
                                        }
                                        r = 2.3025850929940459 + z * (2.6509490552896504 + z * 2.0346785922934552);
                                    }
                                }
                                else
                                {
                                    ReadOnlySpan<double> cp = [2.3025850929940455, 2.6509490552391992, 2.0346786157329868, 1.1712551489193503];
                                    r = (cp[0] + z * cp[1]) + z2 * (cp[2] + z * cp[3]);
                                }
                            }
                            else
                            {
                                ReadOnlySpan<double> cp = [2.3025850929940459, 2.6509490552387951, 2.0346785922938739, 1.1712557955234444, 0.53938292940865262];
                                r = (cp[0] + z * cp[1]) + z2 * (cp[2] + z * (cp[3] + z * cp[4]));
                            }
                        }
                        else
                        {
                            ReadOnlySpan<double> cp = [2.3025850929940459, 2.6509490552391983, 2.0346785922348913, 1.1712551489516381, 0.53938692370821983, 0.20699584816918598];
                            r = (cp[0] + z * cp[1]) + z2 * ((cp[2] + z * cp[3]) + z2 * (cp[4] + z * cp[5]));
                        }
                    }
                    else
                    {
                        ReadOnlySpan<double> cp = [2.3025850929940459, 2.6509490552392512, 2.0346785922933694, 1.1712551474718793, 0.53938292993265813, 0.20700578860031116, 0.068089364982424655];
                        r = (cp[0] + z * cp[1]) + z2 * ((cp[2] + z * cp[3]) + z2 * (cp[4] + z * (cp[5] + z * cp[6])));
                    }
                }
                else
                {
                    ReadOnlySpan<double> cp = [2.3025850929940455, 2.6509490552391997, 2.0346785922965154, 1.1712551489080671, 0.53938291788367909, 0.20699585338612078, 0.068102837768701199, 0.019597694483460711];
                    r = ((cp[0] + z * cp[1]) + z2 * (cp[2] + z * cp[3])) + (z2 * z2) * ((cp[4] + z * cp[5]) + z2 * (cp[6] + z * cp[7]));
                }
            }
            else
            {
                ReadOnlySpan<double> cp = [2.3025850929940455, 2.6509490552391819, 2.0346785922935298, 1.1712551489623777, 0.53938292914310315, 0.20699580881200672, 0.068089378992517491, 0.019609449708105794, 0.0050139289122738354];
                r = ((cp[0] + z * cp[1]) + z2 * (cp[2] + z * cp[3])) + (z2 * z2) * ((cp[4] + z * cp[5]) + z2 * (cp[6] + z * (cp[7] + z * cp[8])));
            }

            r *= z;
            return (float)r;
        }
        else
        {
            if ((ux << 11) == 0)
            {
                uint k = (ux >> 21) - 0x1fc;
                if (k <= 0xb)
                {
                    if (k == 0)
                    {
                        return 10.0f - 1.0f;
                    }
                    if (k == 4)
                    {
                        return 100.0f - 1.0f;
                    }
                    if (k == 6)
                    {
                        return 1000.0f - 1.0f;
                    }
                    if (k == 8)
                    {
                        return 10000.0f - 1.0f;
                    }
                    if (k == 9)
                    {
                        return 100000.0f - 1.0f;
                    }
                    if (k == 10)
                    {
                        return 1000000.0f - 1.0f;
                    }
                    if (k == 11)
                    {
                        return 10000000.0f - 1.0f;
                    }
                }
            }

            double a = iln10h * z, ia = StrictMath.BuiltinFloor(a), h = (a - ia) + iln10l * z;
            long i = (long)ia;
            int j = (int)i & 0xf;
            long e = i - j;
            e >>= 4;

            double s = tb[j];
            ulong su = (ulong)(e + 0x3ff) << 52;
            s *= Polyfill.UInt64BitsToDouble(su);

            double h2 = h * h;
            double c0 = c[0] + h * c[1];
            double c2 = c[2] + h * c[3];
            double c4 = c[4] + h * c[5];

            c0 += h2 * (c2 + h2 * c4);

            double w = s * h;
            return (float)((s - 1.0) + w * c0);
        }
    }
}
