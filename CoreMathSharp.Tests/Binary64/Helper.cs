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

    public static ReadOnlySpan<float> TestFloats => [
        float.E,
        float.Epsilon,
        float.MaxValue,
        float.MinValue,
        float.NaN,
        float.NegativeInfinity,
        float.NegativeZero,
        float.Pi,
        float.PositiveInfinity,
        float.Tau,
        -1.0f,
        0.0f,
        1.0f
    ];
}
