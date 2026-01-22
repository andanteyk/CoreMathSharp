#if NETCOREAPP3_0_OR_GREATER
using System.Runtime.Intrinsics;
#endif

namespace CoreMathSharp;

public static partial class StrictMathF
{
    public static float Abs(float x)
    {
#if NETCOREAPP3_0_OR_GREATER
        if (Vector128.IsHardwareAccelerated)
        {
            return Vector128.BitwiseAnd(Vector128.CreateScalarUnsafe(x), Vector128.CreateScalarUnsafe(StrictMath.UInt32BitsToSingle(~(1u << 31)))).ToScalar();
        }
#endif

        return StrictMath.UInt32BitsToSingle(StrictMath.SingleToUInt32Bits(x) & ~(1u << 31));
    }
}
