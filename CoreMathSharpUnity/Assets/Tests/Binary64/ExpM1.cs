using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("ExpM1")]
    public void BurstLowExpM1()
    {
        MeasurePerformance("ExpM1", () =>
        {
            Result = BurstMath.ExpM1Low(X);
        });
    }

    [Test, Performance]
    [Category("ExpM1")]
    public void BurstMediumExpM1()
    {
        MeasurePerformance("ExpM1", () =>
        {
            Result = BurstMath.ExpM1Medium(X);
        });
    }

    [Test, Performance]
    [Category("ExpM1")]
    public void BurstHighExpM1()
    {
        MeasurePerformance("ExpM1", () =>
        {
            Result = BurstMath.ExpM1High(X);
        });
    }

    [Test, Performance]
    [Category("ExpM1")]
    public void CoreExpM1()
    {
        MeasurePerformance("ExpM1", () =>
        {
            Result = StrictMath.ExpM1(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("ExpM1")]
    public void BurstLowExpM1()
    {
        MeasurePerformance("ExpM1", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.ExpM1Low(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("ExpM1")]
    public void BurstMediumExpM1()
    {
        MeasurePerformance("ExpM1", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.ExpM1Medium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("ExpM1")]
    public void BurstHighExpM1()
    {
        MeasurePerformance("ExpM1", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.ExpM1High(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("ExpM1")]
    public void CoreExpM1()
    {
        MeasurePerformance("ExpM1", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.ExpM1(x);
            }

            Result = sum;
        });
    }
}
