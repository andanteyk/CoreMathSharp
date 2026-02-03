using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("AcosPi")]
    public void BurstLowAcosPi()
    {
        MeasurePerformance("AcosPi", () =>
        {
            Result = BurstMath.AcosPiLow(X);
        });
    }

    [Test, Performance]
    [Category("AcosPi")]
    public void BurstMediumAcosPi()
    {
        MeasurePerformance("AcosPi", () =>
        {
            Result = BurstMath.AcosPiMedium(X);
        });
    }

    [Test, Performance]
    [Category("AcosPi")]
    public void BurstHighAcosPi()
    {
        MeasurePerformance("AcosPi", () =>
        {
            Result = BurstMath.AcosPiHigh(X);
        });
    }

    [Test, Performance]
    [Category("AcosPi")]
    public void CoreAcosPi()
    {
        MeasurePerformance("AcosPi", () =>
        {
            Result = StrictMath.AcosPi(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("AcosPi")]
    public void BurstLowAcosPi()
    {
        MeasurePerformance("AcosPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AcosPiLow(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("AcosPi")]
    public void BurstMediumAcosPi()
    {
        MeasurePerformance("AcosPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AcosPiMedium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("AcosPi")]
    public void BurstHighAcosPi()
    {
        MeasurePerformance("AcosPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AcosPiHigh(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("AcosPi")]
    public void CoreAcosPi()
    {
        MeasurePerformance("AcosPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.AcosPi(x);
            }

            Result = sum;
        });
    }
}
