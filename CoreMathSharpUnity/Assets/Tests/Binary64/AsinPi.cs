using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("AsinPi")]
    public void BurstLowAsinPi()
    {
        MeasurePerformance("AsinPi", () =>
        {
            Result = BurstMath.AsinPiLow(X);
        });
    }

    [Test, Performance]
    [Category("AsinPi")]
    public void BurstMediumAsinPi()
    {
        MeasurePerformance("AsinPi", () =>
        {
            Result = BurstMath.AsinPiMedium(X);
        });
    }

    [Test, Performance]
    [Category("AsinPi")]
    public void BurstHighAsinPi()
    {
        MeasurePerformance("AsinPi", () =>
        {
            Result = BurstMath.AsinPiHigh(X);
        });
    }

    [Test, Performance]
    [Category("AsinPi")]
    public void CoreAsinPi()
    {
        MeasurePerformance("AsinPi", () =>
        {
            Result = StrictMath.AsinPi(X);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("AsinPi")]
    public void PInvokeAsinPi()
    {
        MeasurePerformance("AsinPi", () =>
        {
            Result = PInvoke.PInvoke.AsinPi(X);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("AsinPi")]
    public void BurstLowAsinPi()
    {
        MeasurePerformance("AsinPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AsinPiLow(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("AsinPi")]
    public void BurstMediumAsinPi()
    {
        MeasurePerformance("AsinPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AsinPiMedium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("AsinPi")]
    public void BurstHighAsinPi()
    {
        MeasurePerformance("AsinPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AsinPiHigh(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("AsinPi")]
    public void CoreAsinPi()
    {
        MeasurePerformance("AsinPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.AsinPi(x);
            }

            Result = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("AsinPi")]
    public void PInvokeAsinPi()
    {
        MeasurePerformance("AsinPi", () =>
        {
            double sum = 0.0;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.AsinPi(x);
            }

            Result = sum;
        });
    }
#endif
}
