using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Atan2Pi")]
    public void BurstLowAtan2Pi()
    {
        MeasurePerformance("Atan2Pi", () =>
        {
            Result = BurstMath.Atan2PiLow(Y, X);
        });
    }

    [Test, Performance]
    [Category("Atan2Pi")]
    public void BurstMediumAtan2Pi()
    {
        MeasurePerformance("Atan2Pi", () =>
        {
            Result = BurstMath.Atan2PiMedium(Y, X);
        });
    }

    [Test, Performance]
    [Category("Atan2Pi")]
    public void BurstHighAtan2Pi()
    {
        MeasurePerformance("Atan2Pi", () =>
        {
            Result = BurstMath.Atan2PiHigh(Y, X);
        });
    }

    [Test, Performance]
    [Category("Atan2Pi")]
    public void CoreAtan2Pi()
    {
        MeasurePerformance("Atan2Pi", () =>
        {
            Result = StrictMath.Atan2Pi(Y, X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Atan2Pi")]
    public void BurstLowAtan2Pi()
    {
        MeasurePerformance("Atan2Pi", () =>
        {
            double sum = 0.0;

            for (int i = 0; i < X.Length; i++)
            {
                sum += BurstMath.Atan2PiLow(Y[i], X[i]);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Atan2Pi")]
    public void BurstMediumAtan2Pi()
    {
        MeasurePerformance("Atan2Pi", () =>
        {
            double sum = 0.0;

            for (int i = 0; i < X.Length; i++)
            {
                sum += BurstMath.Atan2PiMedium(Y[i], X[i]);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Atan2Pi")]
    public void BurstHighAtan2Pi()
    {
        MeasurePerformance("Atan2Pi", () =>
        {
            double sum = 0.0;

            for (int i = 0; i < X.Length; i++)
            {
                sum += BurstMath.Atan2PiHigh(Y[i], X[i]);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Atan2Pi")]
    public void CoreAtan2Pi()
    {
        MeasurePerformance("Atan2Pi", () =>
        {
            double sum = 0.0;

            for (int i = 0; i < X.Length; i++)
            {
                sum += StrictMath.Atan2Pi(Y[i], X[i]);
            }

            Result = sum;
        });
    }
}
