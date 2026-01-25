using System;

namespace CoreMathSharp;

public static partial class StrictMath
{
    public static double Cbrt(double x)
    {
        ReadOnlySpan<double> escale = [1.0, 1.2599210498948732, 1.5874010519681996];
        ReadOnlySpan<double> c = [0.55282341840164717, 0.58711429182669816, -0.16296967194987905, 0.023104964110781469];
        const double u0 = 0.33333333333333331, u1 = 0.22222222222222221;
        ReadOnlySpan<double> rsc = [1.0, -1.0, 0.5, -0.5, 0.25, -0.25];
        ReadOnlySpan<double> off = [1.1102230246251565e-16, 0.0, 0.0, 0.0];

        const int rm = 0;

        ulong cvt0 = Polyfill.DoubleToUInt64Bits(x);
        ulong hx = cvt0, mant = hx & (~0ul >> 12), sign = hx >> 63;
        int e = (int)(hx >> 52) & 0x7ff;

        if (((e + 1) & 0x7ff) < 2)
        {
            ulong ix = hx & (~0ul >> 1);
            if (e == 0x7ff || ix == 0)
            {
                return x + x;
            }

            int nz = Polyfill.LeadingZeroCount(ix) - 11;
            mant <<= nz;
            mant &= (~0ul >> 12);
            e -= nz - 1;
        }

        e += 3072;
        ulong cvt1 = mant | 0x3fful << 52, cvt5 = cvt1;

        int et = e / 3, it = e % 3;
        cvt5 += (ulong)it << 52;
        cvt5 |= sign << 63;
        double zz = Polyfill.UInt64BitsToDouble(cvt5);
        ulong isc = Polyfill.DoubleToUInt64Bits(escale[it]);
        isc |= sign << 63;
        ulong cvt2 = isc;
        double z = Polyfill.UInt64BitsToDouble(cvt1);

        double r = 1.0 / z, rr = r * rsc[it << 1 | (int)sign], z2 = z * z;
        double c0 = c[0] + z * c[1], c2 = c[2] + z * c[3];
        double y = c0 + z2 * c2, y2 = y * y;

        double h = y2 * (y * r) - 1.0;
        y -= (h * y) * (u0 - u1 * h);
        y *= Polyfill.UInt64BitsToDouble(cvt2);
        y2 = y * y;
        double y2l = FusedMultiplyAdd(y, y, -y2);
        double y3 = y2 * y, y3l = FusedMultiplyAdd(y, y2, -y3) + y * y2l;
        h = ((y3 - zz) + y3l) * rr;
        double dy = h * (y * u0);
        double y1 = y - dy;
        dy = (y - y1) - dy;

        double ady = Abs(dy);

        double ady0 = Abs(ady - off[rm]);
        double ady1 = Abs(ady - (2.2204460492503131e-16 + off[rm]));
        if (ady < 3.1554436208840472e-30 || ady1 < 3.1554436208840472e-30)
        {
            double azz = Abs(zz);
            if (azz == 3.2146036897957497)
            {
                y1 = CopySign(1.4758508835342132, zz);
            }
            if (azz == 6.5314177950999683)
            {
                y1 = CopySign(1.86925759992312, zz);
            }

            /*
            if (rm > 0)
            {
                ReadOnlySpan<double> wlist = [1.228955119617402, 1.0711377886270597, 1.4698154317852068, 1.1369837758814247, 1.8200608370206306, 1.2209427535680875, 2.0791040613140108, 1.276317553562075, 3.9851265273269978, 1.5854310944237691, 6.6051970722083935, 1.8762696772465612, 6.6956311764144489, 1.8847937846083997];
                for (int i = 0; i < 7; i++)
                {
                    if (azz == wlist[i * 2 + 0])
                    {
                        y1 = CopySign(wlist[i * 2 + 1] + ((rm + sign == 2) ? 2.2204460492503131e-16 : 0.0), zz);
                    }
                }
            }
            //*/
        }

        ulong cvt3 = Polyfill.DoubleToUInt64Bits(y1);
        cvt3 += (ulong)(et - 342 - 1023) << 52;
        long m0 = (long)cvt3 << 30, m1 = m0 >> 63;
        if ((ulong)(m0 ^ m1) <= 1ul << 30)
        {
            ulong cvt4 = Polyfill.DoubleToUInt64Bits(y1);
            cvt4 = (cvt4 + (1ul << 15)) & 0xffffffffffff0000ul;
            if (Abs((Polyfill.UInt64BitsToDouble(cvt4) - y1) - dy) < 8.6736173798840355e-19 || Abs(zz) == 1.0)
            {
                cvt3 = (cvt3 + (1ul << 15)) & 0xffffffffffff0000ul;
            }
        }

        return Polyfill.UInt64BitsToDouble(cvt3);
    }
}
