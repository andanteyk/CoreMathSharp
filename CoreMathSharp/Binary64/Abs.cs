#if NETCOREAPP3_0_OR_GREATER
using System.Runtime.Intrinsics;
#endif

namespace CoreMathSharp;

public static partial class StrictMath
{
    public static double Abs(double x)
    {
#if NETCOREAPP3_0_OR_GREATER
        if (Vector128.IsHardwareAccelerated)
        {
            return Vector128.BitwiseAnd(Vector128.CreateScalarUnsafe(x), Vector128.CreateScalarUnsafe(UInt64BitsToDouble(~(1ul << 63)))).ToScalar();
        }
#endif

        return UInt64BitsToDouble(DoubleToUInt64Bits(x) & ~(1ul << 63));
    }
}
