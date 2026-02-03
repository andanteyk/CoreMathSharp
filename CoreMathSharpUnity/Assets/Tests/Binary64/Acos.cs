using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Acos")]
    public void BurstLowAcos()
    {
        MeasurePerformance("Acos", () =>
        {
            Result = BurstMath.AcosLow(X);
        });
    }

    [Test, Performance]
    [Category("Acos")]
    public void BurstMediumAcos()
    {
        MeasurePerformance("Acos", () =>
        {
            Result = BurstMath.AcosMedium(X);
        });
    }

    [Test, Performance]
    [Category("Acos")]
    public void BurstHighAcos()
    {
        MeasurePerformance("Acos", () =>
        {
            Result = BurstMath.AcosHigh(X);
        });
    }

    [Test, Performance]
    [Category("Acos")]
    public void CoreAcos()
    {
        MeasurePerformance("Acos", () =>
        {
            Result = StrictMath.Acos(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Acos")]
    public void BurstLowAcos()
    {
        MeasurePerformance("Acos", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AcosLow(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Acos")]
    public void BurstMediumAcos()
    {
        MeasurePerformance("Acos", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AcosMedium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Acos")]
    public void BurstHighAcos()
    {
        MeasurePerformance("Acos", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AcosHigh(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Acos")]
    public void CoreAcos()
    {
        MeasurePerformance("Acos", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Acos(x);
            }

            Result = sum;
        });
    }
}
