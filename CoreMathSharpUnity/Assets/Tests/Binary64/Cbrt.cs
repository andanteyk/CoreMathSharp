using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Cbrt")]
    public void BurstLowCbrt()
    {
        MeasurePerformance("Cbrt", () =>
        {
            Result = BurstMath.CbrtLow(X);
        });
    }

    [Test, Performance]
    [Category("Cbrt")]
    public void BurstMediumCbrt()
    {
        MeasurePerformance("Cbrt", () =>
        {
            Result = BurstMath.CbrtMedium(X);
        });
    }

    [Test, Performance]
    [Category("Cbrt")]
    public void BurstHighCbrt()
    {
        MeasurePerformance("Cbrt", () =>
        {
            Result = BurstMath.CbrtHigh(X);
        });
    }

    [Test, Performance]
    [Category("Cbrt")]
    public void CoreCbrt()
    {
        MeasurePerformance("Cbrt", () =>
        {
            Result = StrictMath.Cbrt(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Cbrt")]
    public void BurstLowCbrt()
    {
        MeasurePerformance("Cbrt", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.CbrtLow(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Cbrt")]
    public void BurstMediumCbrt()
    {
        MeasurePerformance("Cbrt", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.CbrtMedium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Cbrt")]
    public void BurstHighCbrt()
    {
        MeasurePerformance("Cbrt", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.CbrtHigh(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Cbrt")]
    public void CoreCbrt()
    {
        MeasurePerformance("Cbrt", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Cbrt(x);
            }

            Result = sum;
        });
    }
}
