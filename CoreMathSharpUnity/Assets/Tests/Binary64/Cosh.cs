using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Cosh")]
    public void BurstLowCosh()
    {
        MeasurePerformance("Cosh", () =>
        {
            Result = BurstMath.CoshLow(X);
        });
    }

    [Test, Performance]
    [Category("Cosh")]
    public void BurstMediumCosh()
    {
        MeasurePerformance("Cosh", () =>
        {
            Result = BurstMath.CoshMedium(X);
        });
    }

    [Test, Performance]
    [Category("Cosh")]
    public void BurstHighCosh()
    {
        MeasurePerformance("Cosh", () =>
        {
            Result = BurstMath.CoshHigh(X);
        });
    }

    [Test, Performance]
    [Category("Cosh")]
    public void CoreCosh()
    {
        MeasurePerformance("Cosh", () =>
        {
            Result = StrictMath.Cosh(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Cosh")]
    public void BurstLowCosh()
    {
        MeasurePerformance("Cosh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.CoshLow(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Cosh")]
    public void BurstMediumCosh()
    {
        MeasurePerformance("Cosh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.CoshMedium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Cosh")]
    public void BurstHighCosh()
    {
        MeasurePerformance("Cosh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.CoshHigh(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Cosh")]
    public void CoreCosh()
    {
        MeasurePerformance("Cosh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Cosh(x);
            }

            Result = sum;
        });
    }
}
