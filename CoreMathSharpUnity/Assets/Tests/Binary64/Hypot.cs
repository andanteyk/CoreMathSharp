using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Hypot")]
    public void BurstLowHypot()
    {
        MeasurePerformance("Hypot", () =>
        {
            Result = BurstMath.HypotLow(X, Y);
        });
    }

    [Test, Performance]
    [Category("Hypot")]
    public void BurstMediumHypot()
    {
        MeasurePerformance("Hypot", () =>
        {
            Result = BurstMath.HypotMedium(X, Y);
        });
    }

    [Test, Performance]
    [Category("Hypot")]
    public void BurstHighHypot()
    {
        MeasurePerformance("Hypot", () =>
        {
            Result = BurstMath.HypotHigh(X, Y);
        });
    }

    [Test, Performance]
    [Category("Hypot")]
    public void CoreHypot()
    {
        MeasurePerformance("Hypot", () =>
        {
            Result = StrictMath.Hypot(X, Y);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Hypot")]
    public void BurstLowHypot()
    {
        MeasurePerformance("Hypot", () =>
        {
            double sum = 0.0;

            for (int i = 0; i < X.Length; i++)
            {
                sum += BurstMath.HypotLow(X[i], Y[i]);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Hypot")]
    public void BurstMediumHypot()
    {
        MeasurePerformance("Hypot", () =>
        {
            double sum = 0.0;

            for (int i = 0; i < X.Length; i++)
            {
                sum += BurstMath.HypotMedium(X[i], Y[i]);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Hypot")]
    public void BurstHighHypot()
    {
        MeasurePerformance("Hypot", () =>
        {
            double sum = 0.0;

            for (int i = 0; i < X.Length; i++)
            {
                sum += BurstMath.HypotHigh(X[i], Y[i]);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Hypot")]
    public void CoreHypot()
    {
        MeasurePerformance("Hypot", () =>
        {
            double sum = 0.0;

            for (int i = 0; i < X.Length; i++)
            {
                sum += StrictMath.Hypot(X[i], Y[i]);
            }

            Result = sum;
        });
    }
}
