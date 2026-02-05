using System.Runtime.CompilerServices;

#if NETCOREAPP3_0_OR_GREATER
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
#endif

namespace CoreMathSharp;

public static partial class StrictMathF
{
    /// <inheritdoc cref="StrictMath.FusedMultiplyAdd(double, double, double)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        static (float h, float l) twoSum(float a, float b)
        {
            float h = a + b;
            float aprime = h - b;
            float l = (a - aprime) + (b - (h - aprime));
            return (h, l);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (float h, float l) split(float x)
        {
            float k = 4097.0f;
            float gamma = k * x;
            float h = gamma + (x - gamma);
            float l = x - h;
            return (h, l);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (float h, float l) dekkerProd(float a, float b)
        {
            (float ah, float al) = split(a);
            (float bh, float bl) = split(b);

            float h = a * b;
            float l = (((-h + ah * bh) + (ah * bl)) + al * bh) + al * bl;
            return (h, l);
        }


        static float FastEmulation(float x, float y, float z)
        {
            float xl, xh, sl, sh, vl, vh;
            (xh, xl) = dekkerProd(x, y);

            if (!float.IsNormal(xh))
            {
                return float.NaN;
            }

            (sh, sl) = twoSum(xh, z);
            (vh, vl) = twoSum(xl, sl);

            if (!float.IsNormal(vh))
            {
                return float.NaN;
            }

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
        }
        //*/



        // https://git.musl-libc.org/cgit/musl/tree/src/math/fmaf.c
        static float Fallback(float x, float y, float z)
        {
            double xy, result;
            int e;

            xy = (double)x * y;
            result = xy + z;

            ulong u = Polyfill.DoubleToUInt64Bits(result);
            e = (int)(u >> 52) & 0x7ff;

            if ((u & 0x1fffffff) != 0x10000000 || e == 0x7ff || (result - xy == z && result - z == xy))
            {
                return (float)result;
            }

            double err;
            int neg = (int)(u >> 63);
            if (neg == (z > xy ? 1 : 0))
            {
                err = xy - result + z;
            }
            else
            {
                err = z - result + xy;
            }

            if (neg == (err < 0 ? 1 : 0))
            {
                u++;
            }
            else
            {
                u--;
            }

            return (float)Polyfill.UInt64BitsToDouble(u);
        }


        /*
        // not so fast (about 3x slower)
        float fastPath = FastEmulation(x, y, z);
        if (!float.IsNaN(fastPath))
        {
            return fastPath;
        }
        //*/
        return Fallback(x, y, z);
    }
}
