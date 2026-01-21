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
    public static float FusedMultiplyAdd(float x, float y, float z)
    {
#if NETCOREAPP3_0_OR_GREATER
        //return MathF.FusedMultiplyAdd(x, y, z);

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
        static bool isNot1Or3TimesPowerOf2(float x)
        {
            float delta = (4194305.0f * x) - (4194304.0f * x);
            return delta != x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (float l, float h) twoSum(float a, float b)
        {
            float h = a + b;
            float aprime = h - b;
            float l = (a - aprime) + (b - (h - aprime));
            return (l, h);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (float l, float h) split(float x)
        {
            float k = 4097.0f;
            float gamma = k * x;
            float h = gamma + (x - gamma);
            float l = x - h;
            return (l, h);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (float l, float h) dekkerProd(float a, float b)
        {
            (float al, float ah) = split(a);
            (float bl, float bh) = split(b);

            float h = a * b;
            float l = (((-h + ah * bh) + (ah * bl)) + al * bh) + al * bl;
            return (l, h);
        }


        float xl, xh, sl, sh, vl, vh;
        (xl, xh) = dekkerProd(x, y);
        (sl, sh) = twoSum(xh, z);
        (vl, vh) = twoSum(xl, sl);

        if (!float.IsFinite(sh) || !float.IsFinite(xl))
        {
            if (float.IsFinite(x) && float.IsFinite(y) && !float.IsFinite(z))
            {
                return z;
            }
            return sh;
        }

        if (isNot1Or3TimesPowerOf2(vh) || vl == 0.0f)
        {
            return sh + vh;
        }
        if ((vl < 0.0f) ^ (vh < 0.0f))
        {
            return sh + (0.875f * vh);
        }
        return sh + (1.125f * vh);
        //*/




        // https://drilian.com/posts/2025.01.02-emulating-the-fmadd-instruction-part-2-64-bit-floats/

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
            ulong bits = StrictMath.DoubleToUInt64Bits(value);

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

            return StrictMath.UInt64BitsToDouble(bits);
        }

        double product = (double)x * (double)y;
        (double sum, double err) = AddwithError(product, z);
        sum = RoundToOdd(sum, err);
        return (float)sum;
    }
}
