using System;

namespace CoreMathSharp;

public static partial class StrictMathF
{
    public static float Cbrt(float x)
    {
        ReadOnlySpan<double> escale = [1.0, 1.2599210498948732, 1.5874010519681996];

        uint t = Polyfill.SingleToUInt32Bits(x);
        uint u = t, au = u << 1, sgn = u >> 31;
        int e = (int)(au >> 24);

        if (au < 1u << 24 || au >= 0xffu << 24)
        {
            if (au >= 0xffu << 24)
            {
                return x + x;
            }
            if (au == 0)
            {
                return x;
            }
            int nz = Polyfill.LeadingZeroCount(au) - 7;
            au <<= nz;
            e -= nz - 1;
        }

        uint mant = au & 0xffffff;
        ulong cvt1 = (ulong)mant << 28 | 0x3fful << 52;
        e += 899;
        int et = e / 3, it = e % 3;
        ulong isc = Polyfill.DoubleToUInt64Bits(escale[it]);
        isc += (ulong)(et - 342) << 52;
        isc |= (ulong)sgn << 63;
        ulong cvt2 = isc;

        ReadOnlySpan<double> c = [0.56855640780593808, 0.70249601853393817, -0.39381000363475277, 0.21397507019181075, -0.085939665639323634, 0.023134567971640832, -0.0037028623664396819, 0.00026571366637555694];
        double z = Polyfill.UInt64BitsToDouble(cvt1), r0 = -0.024975246527242426 / z, z2 = z * z, z4 = z2 * z2;
        double f = ((c[0] + z * c[1]) + z2 * (c[2] + z * c[3])) + z4 * ((c[4] + z * c[5]) + z2 * (c[6] + z * c[7])) + r0;
        double r = f * Polyfill.UInt64BitsToDouble(cvt2);

        float ub = (float)r, lb = (float)(r - Polyfill.UInt64BitsToDouble(cvt2) * 1.4182e-9);
        if (ub == lb)
        {
            return ub;
        }

        const double u0 = -13.346548270093789;
        double h = f * f * f - z;
        f -= (f * r0 * u0) * h;
        r = f * Polyfill.UInt64BitsToDouble(cvt2);
        cvt1 = Polyfill.DoubleToUInt64Bits(r);
        ub = (float)r;
        long m0 = (long)cvt1 << 19, m1 = m0 >> 63;
        if ((m0 ^ m1) < (1L << 31))
        {
            cvt1 = (cvt1 + (1L << 31)) & 0xffffffff00000000ul;
            ub = (float)Polyfill.UInt64BitsToDouble(cvt1);
        }
        return ub;
    }
}
