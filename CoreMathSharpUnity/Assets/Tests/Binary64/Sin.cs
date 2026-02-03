using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Sin")]
    public void BurstLowSin()
    {
        MeasurePerformance("Sin", () =>
        {
            Result = BurstMath.SinLow(X);
        });
    }

    [Test, Performance]
    [Category("Sin")]
    public void BurstMediumSin()
    {
        MeasurePerformance("Sin", () =>
        {
            Result = BurstMath.SinMedium(X);
        });
    }

    [Test, Performance]
    [Category("Sin")]
    public void BurstHighSin()
    {
        MeasurePerformance("Sin", () =>
        {
            Result = BurstMath.SinHigh(X);
        });
    }

    [Test, Performance]
    [Category("Sin")]
    public void CoreSin()
    {
        MeasurePerformance("Sin", () =>
        {
            Result = StrictMath.Sin(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Sin")]
    public void BurstLowSin()
    {
        MeasurePerformance("Sin", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.SinLow(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Sin")]
    public void BurstMediumSin()
    {
        MeasurePerformance("Sin", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.SinMedium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Sin")]
    public void BurstHighSin()
    {
        MeasurePerformance("Sin", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.SinHigh(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Sin")]
    public void CoreSin()
    {
        MeasurePerformance("Sin", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Sin(x);
            }

            Result = sum;
        });
    }
}
