#if NETCOREAPP3_0_OR_GREATER
using System.Runtime.Intrinsics;
#endif

namespace CoreMathSharp;

public static partial class StrictMathF
{
    /// <inheritdoc cref="StrictMath.CopySign(double, double)"/>
    public static float CopySign(float magnitude, float sign)
    {
#if NETCOREAPP3_0_OR_GREATER
        if (Vector128.IsHardwareAccelerated)
        {
            return Vector128.ConditionalSelect(Vector128.CreateScalarUnsafe(-0.0f), Vector128.CreateScalarUnsafe(sign), Vector128.CreateScalarUnsafe(magnitude)).ToScalar();
        }
#endif

        uint magnitudeBits = Polyfill.SingleToUInt32Bits(magnitude);
        uint signBits = Polyfill.SingleToUInt32Bits(sign);

        return Polyfill.UInt32BitsToSingle((magnitudeBits & ((1u << 31) - 1)) | (signBits & (1u << 31)));
    }
}
