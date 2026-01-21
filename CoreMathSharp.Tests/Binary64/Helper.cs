namespace CoreMathSharp.Tests;

public class Helper
{
    public static ReadOnlySpan<double> TestDoubles => [
        double.E,
        double.Epsilon,
        double.MaxValue,
        double.MinValue,
        double.NaN,
        double.NegativeInfinity,
        double.NegativeZero,
        double.Pi,
        double.PositiveInfinity,
        double.Tau,
        -1.0,
        0.0,
        1.0
    ];
}
