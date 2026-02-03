using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Cos")]
    public void BurstLowCos()
    {
        MeasurePerformance("Cos", () =>
        {
            Result = BurstMath.CosLow(X);
        });
    }

    [Test, Performance]
    [Category("Cos")]
    public void BurstMediumCos()
    {
        MeasurePerformance("Cos", () =>
        {
            Result = BurstMath.CosMedium(X);
        });
    }

    [Test, Performance]
    [Category("Cos")]
    public void BurstHighCos()
    {
        MeasurePerformance("Cos", () =>
        {
            Result = BurstMath.CosHigh(X);
        });
    }

    [Test, Performance]
    [Category("Cos")]
    public void CoreCos()
    {
        MeasurePerformance("Cos", () =>
        {
            Result = StrictMath.Cos(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Cos")]
    public void BurstLowCos()
    {
        MeasurePerformance("Cos", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.CosLow(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Cos")]
    public void BurstMediumCos()
    {
        MeasurePerformance("Cos", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.CosMedium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Cos")]
    public void BurstHighCos()
    {
        MeasurePerformance("Cos", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.CosHigh(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Cos")]
    public void CoreCos()
    {
        MeasurePerformance("Cos", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Cos(x);
            }

            Result = sum;
        });
    }
}
