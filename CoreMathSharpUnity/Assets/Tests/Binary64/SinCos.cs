using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("SinCos")]
    public void BurstLowSinCos()
    {
        MeasurePerformance("SinCos", () =>
        {
            BurstMath.SinCosLow(X, out var sin, out var cos);
            Result = sin + cos;
        });
    }

    [Test, Performance]
    [Category("SinCos")]
    public void BurstMediumSinCos()
    {
        MeasurePerformance("SinCos", () =>
        {
            BurstMath.SinCosMedium(X, out var sin, out var cos);
            Result = sin + cos;
        });
    }

    [Test, Performance]
    [Category("SinCos")]
    public void BurstHighSinCos()
    {
        MeasurePerformance("SinCos", () =>
        {
            BurstMath.SinCosHigh(X, out var sin, out var cos);
            Result = sin + cos;
        });
    }

    [Test, Performance]
    [Category("SinCos")]
    public void CoreSinCos()
    {
        MeasurePerformance("SinCos", () =>
        {
            var (sin, cos) = StrictMath.SinCos(X);
            Result = sin + cos;
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("SinCos")]
    public void BurstLowSinCos()
    {
        MeasurePerformance("SinCos", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                BurstMath.SinCosLow(x, out var sin, out var cos);
                sum += sin + cos;
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("SinCos")]
    public void BurstMediumSinCos()
    {
        MeasurePerformance("SinCos", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                BurstMath.SinCosMedium(x, out var sin, out var cos);
                sum += sin + cos;
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("SinCos")]
    public void BurstHighSinCos()
    {
        MeasurePerformance("SinCos", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                BurstMath.SinCosHigh(x, out var sin, out var cos);
                sum += sin + cos;
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("SinCos")]
    public void CoreSinCos()
    {
        MeasurePerformance("SinCos", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                var (sin, cos) = StrictMath.SinCos(x);
                sum += sin + cos;
            }

            Result = sum;
        });
    }
}
