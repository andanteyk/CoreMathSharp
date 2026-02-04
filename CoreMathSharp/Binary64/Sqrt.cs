using System;

namespace CoreMathSharp;

public static partial class StrictMath
{
    /// <summary>
    /// Computes the square-root of a value.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    /// <remarks>
    /// Mathematically, returns √x.
    /// </remarks>
    public static double Sqrt(double x)
    {
        // Due to the IEEE 754 specification, Math.Sqrt is essentially cr_sqrt (correctly rounded.)
        return Math.Sqrt(x);
    }
}
