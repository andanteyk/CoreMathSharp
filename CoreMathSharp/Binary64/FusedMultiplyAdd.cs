using System.Runtime.CompilerServices;

#if NETCOREAPP3_0_OR_GREATER
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
#endif

namespace CoreMathSharp;

public static partial class StrictMath
{
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
            double k = 134217729;
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

        if (isNot1Or3TimesPowerOf2(vh) || vl == 0)
        {
            return sh + vh;
        }
        if ((vl < 0) ^ (vh < 0))
        {
            return sh + (0.875 * vh);
        }
        return sh + (1.125 * vh);
    }
}
