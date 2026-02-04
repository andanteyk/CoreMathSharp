#if NETCOREAPP3_0_OR_GREATER
using System.Runtime.Intrinsics;
#endif

namespace CoreMathSharp;

public static partial class StrictMath
{
    /// <summary>
    /// Copies the sign of a value to the sign of another value.
    /// </summary>
    /// <param name="magnitude">The value whose magnitude is used in the result.</param>
    /// <param name="sign">The value whose sign is used in the result.</param>
    /// <returns>±magnitude, sign is the same as sign</returns>
    public static double CopySign(double magnitude, double sign)
    {
#if NETCOREAPP3_0_OR_GREATER
        if (Vector128.IsHardwareAccelerated)
        {
            return Vector128.ConditionalSelect(Vector128.CreateScalarUnsafe(-0.0), Vector128.CreateScalarUnsafe(sign), Vector128.CreateScalarUnsafe(magnitude)).ToScalar();
        }
#endif

        ulong magnitudeBits = Polyfill.DoubleToUInt64Bits(magnitude);
        ulong signBits = Polyfill.DoubleToUInt64Bits(sign);

        return Polyfill.UInt64BitsToDouble((magnitudeBits & ((1ul << 63) - 1)) | (signBits & (1ul << 63)));
    }
}
