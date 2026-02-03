using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Log10P1")]
    public void BurstLowLog10P1()
    {
        MeasurePerformance("Log10P1", () =>
        {
            Result = BurstMath.Log10P1Low(X);
        });
    }

    [Test, Performance]
    [Category("Log10P1")]
    public void BurstMediumLog10P1()
    {
        MeasurePerformance("Log10P1", () =>
        {
            Result = BurstMath.Log10P1Medium(X);
        });
    }

    [Test, Performance]
    [Category("Log10P1")]
    public void BurstHighLog10P1()
    {
        MeasurePerformance("Log10P1", () =>
        {
            Result = BurstMath.Log10P1High(X);
        });
    }

    [Test, Performance]
    [Category("Log10P1")]
    public void CoreLog10P1()
    {
        MeasurePerformance("Log10P1", () =>
        {
            Result = StrictMath.Log10P1(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Log10P1")]
    public void BurstLowLog10P1()
    {
        MeasurePerformance("Log10P1", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.Log10P1Low(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Log10P1")]
    public void BurstMediumLog10P1()
    {
        MeasurePerformance("Log10P1", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.Log10P1Medium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Log10P1")]
    public void BurstHighLog10P1()
    {
        MeasurePerformance("Log10P1", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.Log10P1High(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Log10P1")]
    public void CoreLog10P1()
    {
        MeasurePerformance("Log10P1", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Log10P1(x);
            }

            Result = sum;
        });
    }
}
