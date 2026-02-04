using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Asin")]
    public void BurstLowAsin()
    {
        MeasurePerformance("Asin", () =>
        {
            Result = BurstMath.AsinLow(X);
        });
    }

    [Test, Performance]
    [Category("Asin")]
    public void BurstMediumAsin()
    {
        MeasurePerformance("Asin", () =>
        {
            Result = BurstMath.AsinMedium(X);
        });
    }

    [Test, Performance]
    [Category("Asin")]
    public void BurstHighAsin()
    {
        MeasurePerformance("Asin", () =>
        {
            Result = BurstMath.AsinHigh(X);
        });
    }

    [Test, Performance]
    [Category("Asin")]
    public void CoreAsin()
    {
        MeasurePerformance("Asin", () =>
        {
            Result = StrictMath.Asin(X);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Asin")]
    public void PInvokeAsin()
    {
        MeasurePerformance("Asin", () =>
        {
            Result = PInvoke.PInvoke.Asin(X);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Asin")]
    public void BurstLowAsin()
    {
        MeasurePerformance("Asin", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AsinLow(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Asin")]
    public void BurstMediumAsin()
    {
        MeasurePerformance("Asin", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AsinMedium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Asin")]
    public void BurstHighAsin()
    {
        MeasurePerformance("Asin", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AsinHigh(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Asin")]
    public void CoreAsin()
    {
        MeasurePerformance("Asin", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Asin(x);
            }

            Result = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Asin")]
    public void PInvokeAsin()
    {
        MeasurePerformance("Asin", () =>
        {
            double sum = 0.0;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.Asin(x);
            }

            Result = sum;
        });
    }
#endif
}
