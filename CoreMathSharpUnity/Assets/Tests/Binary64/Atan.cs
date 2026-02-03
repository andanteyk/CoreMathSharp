using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Atan")]
    public void BurstLowAtan()
    {
        MeasurePerformance("Atan", () =>
        {
            Result = BurstMath.AtanLow(X);
        });
    }

    [Test, Performance]
    [Category("Atan")]
    public void BurstMediumAtan()
    {
        MeasurePerformance("Atan", () =>
        {
            Result = BurstMath.AtanMedium(X);
        });
    }

    [Test, Performance]
    [Category("Atan")]
    public void BurstHighAtan()
    {
        MeasurePerformance("Atan", () =>
        {
            Result = BurstMath.AtanHigh(X);
        });
    }

    [Test, Performance]
    [Category("Atan")]
    public void CoreAtan()
    {
        MeasurePerformance("Atan", () =>
        {
            Result = StrictMath.Atan(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Atan")]
    public void BurstLowAtan()
    {
        MeasurePerformance("Atan", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AtanLow(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Atan")]
    public void BurstMediumAtan()
    {
        MeasurePerformance("Atan", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AtanMedium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Atan")]
    public void BurstHighAtan()
    {
        MeasurePerformance("Atan", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AtanHigh(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Atan")]
    public void CoreAtan()
    {
        MeasurePerformance("Atan", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Atan(x);
            }

            Result = sum;
        });
    }
}
