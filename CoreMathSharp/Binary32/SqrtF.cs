using System;

namespace CoreMathSharp;

public static partial class StrictMathF
{
    /// <inheritdoc cref="StrictMath.Sqrt(double)"/>
    public static float Sqrt(float x)
    {
        // Due to the IEEE 754 specification, MathF.Sqrt is essentially cr_sqrtf (correctly rounded.)
        return MathF.Sqrt(x);
    }
}
