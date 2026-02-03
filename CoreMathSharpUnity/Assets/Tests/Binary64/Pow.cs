using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Pow")]
    public void BurstLowPow()
    {
        MeasurePerformance("Pow", () =>
        {
            Result = BurstMath.PowLow(X, Y);
        });
    }

    [Test, Performance]
    [Category("Pow")]
    public void BurstMediumPow()
    {
        MeasurePerformance("Pow", () =>
        {
            Result = BurstMath.PowMedium(X, Y);
        });
    }

    [Test, Performance]
    [Category("Pow")]
    public void BurstHighPow()
    {
        MeasurePerformance("Pow", () =>
        {
            Result = BurstMath.PowHigh(X, Y);
        });
    }

    [Test, Performance]
    [Category("Pow")]
    public void CorePow()
    {
        MeasurePerformance("Pow", () =>
        {
            Result = StrictMath.Pow(X, Y);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Pow")]
    public void BurstLowPow()
    {
        MeasurePerformance("Pow", () =>
        {
            double sum = 0.0;

            for (int i = 0; i < X.Length; i++)
            {
                sum += BurstMath.PowLow(X[i], Y[i]);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Pow")]
    public void BurstMediumPow()
    {
        MeasurePerformance("Pow", () =>
        {
            double sum = 0.0;

            for (int i = 0; i < X.Length; i++)
            {
                sum += BurstMath.PowMedium(X[i], Y[i]);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Pow")]
    public void BurstHighPow()
    {
        MeasurePerformance("Pow", () =>
        {
            double sum = 0.0;

            for (int i = 0; i < X.Length; i++)
            {
                sum += BurstMath.PowHigh(X[i], Y[i]);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Pow")]
    public void CorePow()
    {
        MeasurePerformance("Pow", () =>
        {
            double sum = 0.0;

            for (int i = 0; i < X.Length; i++)
            {
                sum += StrictMath.Pow(X[i], Y[i]);
            }

            Result = sum;
        });
    }
}
