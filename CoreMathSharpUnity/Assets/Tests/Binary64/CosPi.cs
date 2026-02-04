using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("CosPi")]
    public void BurstLowCosPi()
    {
        MeasurePerformance("CosPi", () =>
        {
            Result = BurstMath.CosPiLow(X);
        });
    }

    [Test, Performance]
    [Category("CosPi")]
    public void BurstMediumCosPi()
    {
        MeasurePerformance("CosPi", () =>
        {
            Result = BurstMath.CosPiMedium(X);
        });
    }

    [Test, Performance]
    [Category("CosPi")]
    public void BurstHighCosPi()
    {
        MeasurePerformance("CosPi", () =>
        {
            Result = BurstMath.CosPiHigh(X);
        });
    }

    [Test, Performance]
    [Category("CosPi")]
    public void CoreCosPi()
    {
        MeasurePerformance("CosPi", () =>
        {
            Result = StrictMath.CosPi(X);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("CosPi")]
    public void PInvokeCosPi()
    {
        MeasurePerformance("CosPi", () =>
        {
            Result = PInvoke.PInvoke.CosPi(X);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("CosPi")]
    public void BurstLowCosPi()
    {
        MeasurePerformance("CosPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.CosPiLow(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("CosPi")]
    public void BurstMediumCosPi()
    {
        MeasurePerformance("CosPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.CosPiMedium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("CosPi")]
    public void BurstHighCosPi()
    {
        MeasurePerformance("CosPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.CosPiHigh(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("CosPi")]
    public void CoreCosPi()
    {
        MeasurePerformance("CosPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.CosPi(x);
            }

            Result = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("CosPi")]
    public void PInvokeCosPi()
    {
        MeasurePerformance("CosPi", () =>
        {
            double sum = 0.0;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.CosPi(x);
            }

            Result = sum;
        });
    }
#endif
}
