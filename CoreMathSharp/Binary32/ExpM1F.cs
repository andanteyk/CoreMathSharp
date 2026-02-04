using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if NETCOREAPP3_0_OR_GREATER
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
#endif

namespace CoreMathSharp;

public static partial class StrictMathF
{
    /// <inheritdoc cref="StrictMath.ExpM1(double)"/>
    public static float ExpM1(float x)
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


        ReadOnlySpan<double> c = [1.0, 0.021660849391257477, 0.0002345984913513542, 1.6938658699950235e-06];
        ReadOnlySpan<double> ch = [0.02166084939249829, 0.0002345961982022468, 1.6938509724129055e-06, 9.1725627017026289e-09, 3.973729405780548e-11, 1.4345723178374038e-13];
        ReadOnlySpan<double> td = [1, 1.0218971486541166, 1.0442737824274138, 1.0671404006768237, 1.0905077326652577, 1.1143867425958924, 1.1387886347566916, 1.1637248587775775, 1.189207115002721, 1.215247359980469, 1.241857812073484, 1.2690509571917332, 1.2968395546510096, 1.3252366431597413, 1.3542555469368927, 1.383909881963832, 1.4142135623730951, 1.4451808069770467, 1.4768261459394993, 1.5091644275934228, 1.5422108254079407, 1.5759808451078865, 1.6104903319492543, 1.6457554781539649, 1.681792830507429, 1.7186192981224779, 1.7562521603732995, 1.7947090750031072, 1.8340080864093424, 1.8741676341103, 1.9152065613971474, 1.9571441241754002];

        const double iln2 = 46.166241308446828, big = 6755399441055744.0;


        uint t = Polyfill.SingleToUInt32Bits(x);
        double z = x;
        uint ux = t, ax = ux << 1;

        if (ax < 0x7c400000u)
        {
            if (ax < 0x676a09e8u)
            {
                if (ax == 0u)
                {
                    return x;
                }
                float res = FusedMultiplyAdd(Abs(x), 2.9802322387695312e-08f, x);
                return res;
            }

            ReadOnlySpan<double> b = [0.49999999999999656, 0.16666666666667135, 0.041666666668544565, 0.0083333333324792109, 0.0013888886118215516, 0.00019841274040338812, 2.4816724201894197e-05, 2.755731951095977e-06];
            double z2 = z * z, z4 = z2 * z2;
            double r1 = z + z2 * ((b[0] + z * b[1]) + z2 * (b[2] + z * b[3]) + z4 * ((b[4] + z * b[5]) + z2 * (b[6] + z * b[7])));
            return (float)r1;
        }
        if (ax >= 0x8562e430u)
        {
            if (ax > 0xffu << 24)
            {
                return x + x;
            }
            if (ux >> 31 != 0)
            {
                if (ax == 0xffu << 24)
                {
                    return -1.0f;
                }
                return -1.0f + 1.4901161193847656e-08f;
            }
            if (ax == 0xffu << 24)
            {
                return x * x;
            }
            float r1 = (float)(3.4028234663852886e+38 * z);
            return r1;
        }

        double a = iln2 * z, ia = RoundEvenFinite(a), h = a - ia, h2 = h * h;
        ulong u = Polyfill.DoubleToUInt64Bits(ia + big);

        double c2 = c[2] + h * c[3], c0 = c[0] + h * c[1];
        ReadOnlySpan<ulong> tdl = MemoryMarshal.Cast<double, ulong>(td);
        ulong sv = tdl[(int)u & 0x1f] + ((u >> 5) << 52);

        double r = (c0 + h2 * c2) * Polyfill.UInt64BitsToDouble(sv) - 1.0;
        float ub = (float)r, lb = (float)(r - Polyfill.UInt64BitsToDouble(sv) * 1.4333068065752741e-10);
        if (ub != lb)
        {
            if (ux > 0xc18aa123u)
            {
                return -1.0f + 1.4901161193847656e-08f;
            }
            const double iln2h = 46.16624128818512, iln2l = 2.026170940661134e-08;
            double s = Polyfill.UInt64BitsToDouble(sv);
            h = (iln2h * z - ia) + iln2l * z;
            h2 = h * h;
            double w = s * h;

            r = (s - 1.0) + w * ((ch[0] + h * ch[1]) + h2 * ((ch[2] + h * ch[3]) + h2 * (ch[4] + h * ch[5])));
            ub = (float)r;
        }

        return ub;
    }
}
