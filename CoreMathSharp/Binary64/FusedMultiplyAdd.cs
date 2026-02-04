using System;
using System.Runtime.CompilerServices;

#if NETCOREAPP3_0_OR_GREATER
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
#endif

namespace CoreMathSharp;

public static partial class StrictMath
{
    /// <summary>
    /// Computes the fused multiply-add of three values.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns>x * y + z, but the result is rounded only once</returns>
    public static double FusedMultiplyAdd(double x, double y, double z)
    {
#if NETCOREAPP3_0_OR_GREATER
        //return Math.FusedMultiplyAdd(x, y, z);

        if (Fma.IsSupported)
        {
            return Fma.MultiplyAdd(Vector128.CreateScalarUnsafe(x), Vector128.CreateScalarUnsafe(y), Vector128.CreateScalarUnsafe(z)).ToScalar();
        }
        if (AdvSimd.IsSupported)
        {
            return AdvSimd.FusedMultiplyAddScalar(Vector64.CreateScalarUnsafe(z), Vector64.CreateScalarUnsafe(x), Vector64.CreateScalarUnsafe(y)).ToScalar();
        }
#endif


        /*
        // https://hal.science/hal-04575249/document

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool isNot1Or3TimesPowerOf2(double x)
        {
            double delta = (2251799813685249.0 * x) - (2251799813685248.0 * x);
            return delta != x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double l, double h) twoSum(double a, double b)
        {
            double h = a + b;
            double aprime = h - b;
            double l = (a - aprime) + (b - (h - aprime));
            return (l, h);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double l, double h) split(double x)
        {
            double k = 134217729.0;
            double gamma = k * x;
            double h = gamma + (x - gamma);
            double l = x - h;
            return (l, h);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double l, double h) dekkerProd(double a, double b)
        {
            (double al, double ah) = split(a);
            (double bl, double bh) = split(b);

            double h = a * b;
            double l = (((-h + ah * bh) + (ah * bl)) + al * bh) + al * bl;
            return (l, h);
        }


        double xl, xh, sl, sh, vl, vh;
        (xl, xh) = dekkerProd(x, y);
        (sl, sh) = twoSum(xh, z);
        (vl, vh) = twoSum(xl, sl);

        if (!double.IsFinite(sh) || !double.IsFinite(xl))
        {
            if (double.IsFinite(x) && double.IsFinite(y) && !double.IsFinite(z))
            {
                return z;
            }
            return sh;
        }

        if (isNot1Or3TimesPowerOf2(vh) || vl == 0.0)
        {
            return sh + vh;
        }
        if ((vl < 0.0) ^ (vh < 0.0))
        {
            return sh + (0.875 * vh);
        }
        return sh + (1.125 * vh);
        //*/



        // https://drilian.com/posts/2025.01.02-emulating-the-fmadd-instruction-part-2-64-bit-floats/

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double ZeroBottom27BitsOfMantissa(double x)
        {
            return Polyfill.UInt64BitsToDouble(Polyfill.DoubleToUInt64Bits(x) & ~0x7ff_fffful);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double h, double l) Split(double v)
        {
            double h = ZeroBottom27BitsOfMantissa(v);
            double l = v - h;
            return (h, l);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double prod, double err) MulWithError(double x, double y)
        {
            double prod = x * y;

            var (xh, xl) = Split(x);
            var (yh, yl) = Split(y);
            double err = (((xh * yh - prod) + xh * yl) + xl * yh) + xl * yl;
            return (prod, err);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double sum, double err) AddwithError(double x, double y)
        {
            double sum = x + y;
            double intermediate = sum - x;
            double err1 = y - intermediate;
            double err2 = x - (sum - intermediate);
            return (sum, err1 + err2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double RoundToOdd(double value, double errorTerm)
        {
            ulong bits = Polyfill.DoubleToUInt64Bits(value);

            if (errorTerm != 0.0 && (bits & 1) == 0)
            {
                if (errorTerm > 0.0)
                {
                    bits++;
                }
                else
                {
                    bits--;
                }
            }

            return Polyfill.UInt64BitsToDouble(bits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double OddRoundedAdd(double x, double y)
        {
            var (sum, err) = AddwithError(x, y);
            return RoundToOdd(sum, err);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double Pow2(int exponent)
        {
            return Polyfill.UInt64BitsToDouble((ulong)(exponent + 1023) << 52);
        }


        double AvoidSubnormalBias = Pow2(110);
        double bias = 1.0;
        {
            double testResult = Abs(x * y + z);
            if (testResult < Pow2(-500) && Math.Max(Math.Max(x, y), z) < Pow2(800))
            {
                bias = AvoidSubnormalBias;
            }
            else if (double.IsInfinity(testResult))
            {
                bias = Pow2(-55);
            }
        }

        (double ab, double abErr) = MulWithError(x * bias, y);
        (double abc, double abcErr) = AddwithError(ab, z * bias);

        if (!double.IsFinite(abc))
        {
            if (double.IsInfinity(z) && double.IsFinite(x) && double.IsFinite(y))
            {
                return z;
            }

            return x * y + z;
        }

        double err = OddRoundedAdd(abErr, abcErr);

        double SubnormThreshold = Pow2(-1022);

        if (bias == AvoidSubnormalBias && Abs(abc) < SubnormThreshold)
        {
            (double finalSum, double finalSumErr) = AddwithError(abc, err);

            double OneBitSubnormalThreshold = SubnormThreshold * 0.5;

            if (Abs(finalSum) >= OneBitSubnormalThreshold)
            {
                var (rh, rl) = Split(finalSum);

                rl = OddRoundedAdd(rl, finalSumErr);

                rh /= bias;
                rl /= bias;
                return rh + rl;
            }
            else
            {
                finalSum = RoundToOdd(finalSum, finalSumErr);
                return finalSum / bias;
            }
        }
        else
        {
            return (abc + err) / bias;
        }
    }
}
