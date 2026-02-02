using System;
using System.Runtime.CompilerServices;


#if NETCOREAPP3_0_OR_GREATER
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
#endif

namespace CoreMathSharp;

public static partial class StrictMathF
{
    public static float TGamma(float x)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double RoundEvenFinite(double x)
        {
#if NETCOREAPP3_0_OR_GREATER
            if (Avx.IsSupported)
            {
                return Avx.RoundToNearestInteger(Vector256.CreateScalarUnsafe(x)).ToScalar();
            }
            if (AdvSimd.IsSupported)
            {
                return AdvSimd.RoundToNearestScalar(Vector64.CreateScalarUnsafe(x)).ToScalar();
            }
            if (Sse41.IsSupported)
            {
                return Sse41.RoundToNearestIntegerScalar(Vector128.CreateScalarUnsafe(x)).ToScalar();
            }
#endif

            double ix = StrictMath.BuiltinRound(x);
            if (StrictMath.Abs(ix - x) == 0.5)
            {
                double u = ix;
                double v = ix - StrictMath.CopySign(1.0, x);
                if (Polyfill.TrailingZeroCount(Polyfill.DoubleToUInt64Bits(v)) > Polyfill.TrailingZeroCount(Polyfill.DoubleToUInt64Bits(u)))
                {
                    ix = v;
                }
            }

            return ix;
        }





        ReadOnlySpan<uint> tbx = [0x27de86a9u, 0x27e05475u, 0xb63befb3u, 0x3c7bb570u, 0x41e886d1u, 0xc067d177u, 0xbd99da31u, 0xbf54c45au, 0x41ee77feu, 0x3f843a64u,];
        ReadOnlySpan<float> tbf = [161908237795328f, 160606292279296f, -357083.5625f, 64.52886962890625f, 3.8014147270629119e+29f, 0.24537095427513123f, -13.968554496765137f, -6.604792594909668f, 4.6287780950070885e+30f, 0.98198103904724121f];
        ReadOnlySpan<float> tbdf = [4194304f, 4194304f, 0.0078125f, 1.9073486328125e-06f, 9.4447329657392904e+21f, 3.7252902984619141e-09f, -2.384185791015625e-07f, 1.1920928955078125e-07f, -1.3234889800848443e-23f, 1.4901161193847656e-08f];

        uint t = Polyfill.SingleToUInt32Bits(x);
        uint ax = t << 1;
        if (ax >= 0xffu << 24)
        {
            if (ax == 0xffu << 24)
            {
                if (t >> 31 != 0)
                {
                    return x / x;
                }
                return x;
            }
            return x + x;
        }

        double z = x;
        if (ax < 0x6d000000u)
        {
            double d = (0.9890559953279725 - 0.90747907608088629 * z) * z - 0.57721566490153287;
            double f = 1.0 / z + d;
            float r = (float)f;

            ulong rt = Polyfill.DoubleToUInt64Bits(f);
            if (((rt + 2) & 0xfffffff) < 4)
            {
                for (int i = 0; i < tbx.Length; i++)
                {
                    if (t == tbx[i])
                    {
                        return tbf[i] + tbdf[i];
                    }
                }
            }

            return r;
        }

        float fx = BuiltinFloor(x);
        if (x >= 35.04010009765625f)
        {
            return 1.7014118346046923e+38f * 1.7014118346046923e+38f;
        }

        int k;
        if (x <= -2147483648f)
        {
            k = ~0;
        }
        else
        {
            k = (int)fx;
        }

        if (fx == x)
        {
            if (x == 0.0f)
            {
                return 1.0f / x;
            }
            if (x < 0.0f)
            {
                return 0.0f / 0.0f;
            }

            double t0 = 1.0, x0 = 1.0;
            for (int i = 1; i < k; i++, x0 += 1.0)
            {
                t0 *= x0;
            }
            return (float)t0;
        }

        if (x < -42.0f)
        {
            ReadOnlySpan<float> sgn = [5.8774717541114375e-39f, -5.8774717541114375e-39f];
            return 5.8774717541114375e-39f * sgn[k & 1];
        }


        ReadOnlySpan<double> c = [1.7877108988969403, 1.5591939012079508, 1.0510493266811867, 0.47065801829337245, 0.18881863831977497, 0.058831548411746724, 0.017825943652294146, 0.0042287581489297722, 0.0010979178537172871, 0.00019456568933897892, 5.1971317596743149e-05, 4.9141441406102183e-06, 2.4371734717106688e-06, -1.4461519623063317e-07, 1.8260131876052383e-07, -4.9199488956189672e-08];

        {
            double m = z - 2.875, i = RoundEvenFinite(m), step = StrictMath.CopySign(1.0, i);
            double d = m - i, d2 = d * d, d4 = d2 * d2, d8 = d4 * d4;

            double f = (c[0] + d * c[1]) + d2 * (c[2] + d * c[3]) + d4 * ((c[4] + d * c[5]) + d2 * (c[6] + d * c[7])) +
                d8 * ((c[8] + d * c[9]) + d2 * (c[10] + d * c[11]) + d4 * ((c[12] + d * c[13]) + d2 * (c[14] + d * c[15])));

            int jm = (int)StrictMath.Abs(i);
            double w = 1.0;

            if (jm != 0)
            {
                z -= 0.5 + step * 0.5;
                w = z;
                for (int j = jm - 1; j != 0; j--)
                {
                    z -= step;
                    w *= z;
                }
            }

            if (i <= -0.5)
            {
                w = 1.0 / w;
            }
            f *= w;

            ulong rt = Polyfill.DoubleToUInt64Bits(f);
            float r = (float)f;

            if (((rt + 2) & 0xfffffff) < 8)
            {
                for (int j = 0; j < tbx.Length; j++)
                {
                    if (t == tbx[j])
                    {
                        return tbf[j] + tbdf[j];
                    }
                }
            }

            return r;
        }
    }
}
