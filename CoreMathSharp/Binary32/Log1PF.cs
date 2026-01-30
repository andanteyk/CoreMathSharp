using System;

namespace CoreMathSharp;

public static partial class StrictMathF
{
    public static float Log1P(float x)
    {

        static float asSpecial(float x)
        {
            uint t = Polyfill.SingleToUInt32Bits(x);
            if (t == 0xbf800000u)
            {
                return -1.0f / 0.0f;
            }
            if (t == 0x7f800000u)
            {
                return x;
            }

            uint ax = t << 1;
            if (ax > 0xff000000u)
            {
                return x + x;
            }
            return 0.0f / 0.0f;
        }


        ReadOnlySpan<double> x0 = [0.98461532592773438, 0.95522391796112061, 0.9275362491607666, 0.90140843391418457, 0.87671232223510742, 0.85333335399627686, 0.83116888999938965, 0.81012654304504395, 0.79012346267700195, 0.77108430862426758, 0.75294113159179688, 0.73563218116760254, 0.71910107135772705, 0.70329666137695312, 0.6881721019744873, 0.67368423938751221, 0.65979385375976562, 0.64646470546722412, 0.63366341590881348, 0.62135922908782959, 0.60952377319335938, 0.59813082218170166, 0.58715593814849854, 0.5765765905380249, 0.56637167930603027, 0.55652177333831787, 0.54700851440429688, 0.53781509399414062, 0.52892565727233887, 0.5203251838684082, 0.51199996471405029, 0.5039370059967041];
        ReadOnlySpan<double> lix = [0.015504246151250851, 0.045809496926385883, 0.075223402621775251, 0.10379681231873428, 0.13157636524993893, 0.15860500597289098, 0.18492226772413786, 0.21056481754676373, 0.23556606387282539, 0.2599575617004688, 0.28376823274593022, 0.30702503903084122, 0.32975335902627051, 0.35197648277246379, 0.37371632412254996, 0.39499376541067049, 0.41582783554970704, 0.43623667551594897, 0.45623735526113346, 0.47584589556737728, 0.49507732641313712, 0.51394578277784142, 0.53246484172095021, 0.55064709374891463, 0.56850473908859811, 0.58604898354692925, 0.60329091105336985, 0.62024046936714317, 0.63690739146719511, 0.65330130927628838, 0.66943072287114114, 0.68530400683484882];
        ReadOnlySpan<double> b = [1, -0.5, 0.33333333333370402, -0.25000000000059291, 0.1999999921853749, -0.16666665744658113, 0.14290985945424051, -0.12505271460275799];
        ReadOnlySpan<double> c = [0.9999999964978914, -0.49999999241150506, 0.33339251544971726, -0.25006904941156682];

        double z = x;
        uint t = Polyfill.SingleToUInt32Bits(x);
        uint ux = t;

        if (ux >= 0xbf800000u)
        {
            return asSpecial(x);
        }

        uint ax = ux & (~0u >> 1);
        if (ax >= 0x7f800000u)
        {
            return asSpecial(x);
        }
        if (ax < 0x3c880000u)
        {
            if (ax < 0x33000000u)
            {
                if (ax == 0)
                {
                    return x;
                }

                float res = FusedMultiplyAdd(x, -x, x);
                return res;
            }

            double z2 = z * z, z4 = z2 * z2;
            double f = z2 * ((b[1] + z * b[2]) + z2 * (b[3] + z * b[4]) + z4 * (b[5] + z * (b[6] + z * b[7])));
            ulong r = Polyfill.DoubleToUInt64Bits(z + f);
            if ((r & 0xfffffff) == 0)
            {
                r = Polyfill.DoubleToUInt64Bits(Polyfill.UInt64BitsToDouble(r) + 16384.0 * (z - Polyfill.UInt64BitsToDouble(r)));
            }
            return (float)Polyfill.UInt64BitsToDouble(r);
        }
        else
        {
            ulong tp = Polyfill.DoubleToUInt64Bits(z + 1.0);
            int e = (int)(tp >> 52) - 0x3ff;
            ulong m52 = tp & (~0ul >> 12);
            int j = (int)(tp >> (52 - 5)) & 31;
            ulong xd = m52 | (0x3fful << 52);
            z = Polyfill.UInt64BitsToDouble(xd) * x0[j] - 1.0;

            const double ln2 = 0.69314718055994529;
            double z2 = z * z, r = (ln2 * e + lix[j]) + z * ((c[0] + z * c[1]) + z2 * (c[2] + z * c[3]));

            const double eps = 2.1555e-11;
            float ub = (float)r, lb = (float)(r - eps);
            if (ub != lb)
            {
                double z4 = z2 * z2, f = z2 * ((b[1] + z * b[2]) + z2 * (b[3] + z * b[4]) + z4 * (b[5] + z * (b[6] + z * b[7])));
                double lj = lix[j] - 1.063904520037795e-11;

                const double ln2l = 1.4286068203094173e-06, ln2h = 0.693145751953125;
                double Lh = ln2h * e, Ll = ln2l * e;
                Ll += z;
                double rh = Lh + lj, rl = ((Lh - rh) + lj) + (Ll + f);

                float fh = (float)(rh + rl);
                double Fl = (rh - fh) + rl;
                float fl = (float)Fl, tfl = fl * 2.0f;
                if ((fh + tfl) - fh == tfl)
                {
                    fl += CopySign(0.5f, (float)(Fl - fl)) * Abs(fl);
                }
                ub = fh + fl;
            }
            return ub;
        }

    }
}
