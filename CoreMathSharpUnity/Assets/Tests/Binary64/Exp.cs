using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Exp")]
    public void BurstLowExp()
    {
        MeasurePerformance("Exp", () =>
        {
            Result = BurstMath.ExpLow(X);
        });
    }

    [Test, Performance]
    [Category("Exp")]
    public void BurstMediumExp()
    {
        MeasurePerformance("Exp", () =>
        {
            Result = BurstMath.ExpMedium(X);
        });
    }

    [Test, Performance]
    [Category("Exp")]
    public void BurstHighExp()
    {
        MeasurePerformance("Exp", () =>
        {
            Result = BurstMath.ExpHigh(X);
        });
    }

    [Test, Performance]
    [Category("Exp")]
    public void CoreExp()
    {
        MeasurePerformance("Exp", () =>
        {
            Result = StrictMath.Exp(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Exp")]
    public void BurstLowExp()
    {
        MeasurePerformance("Exp", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.ExpLow(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Exp")]
    public void BurstMediumExp()
    {
        MeasurePerformance("Exp", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.ExpMedium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Exp")]
    public void BurstHighExp()
    {
        MeasurePerformance("Exp", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.ExpHigh(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Exp")]
    public void CoreExp()
    {
        MeasurePerformance("Exp", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Exp(x);
            }

            Result = sum;
        });
    }
}
