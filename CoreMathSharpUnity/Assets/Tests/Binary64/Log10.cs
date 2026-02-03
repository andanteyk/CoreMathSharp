using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Log10")]
    public void BurstLowLog10()
    {
        MeasurePerformance("Log10", () =>
        {
            Result = BurstMath.Log10Low(X);
        });
    }

    [Test, Performance]
    [Category("Log10")]
    public void BurstMediumLog10()
    {
        MeasurePerformance("Log10", () =>
        {
            Result = BurstMath.Log10Medium(X);
        });
    }

    [Test, Performance]
    [Category("Log10")]
    public void BurstHighLog10()
    {
        MeasurePerformance("Log10", () =>
        {
            Result = BurstMath.Log10High(X);
        });
    }

    [Test, Performance]
    [Category("Log10")]
    public void CoreLog10()
    {
        MeasurePerformance("Log10", () =>
        {
            Result = StrictMath.Log10(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Log10")]
    public void BurstLowLog10()
    {
        MeasurePerformance("Log10", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.Log10Low(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Log10")]
    public void BurstMediumLog10()
    {
        MeasurePerformance("Log10", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.Log10Medium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Log10")]
    public void BurstHighLog10()
    {
        MeasurePerformance("Log10", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.Log10High(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Log10")]
    public void CoreLog10()
    {
        MeasurePerformance("Log10", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Log10(x);
            }

            Result = sum;
        });
    }
}
