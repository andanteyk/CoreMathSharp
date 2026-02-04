using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("AtanPi")]
    public void BurstLowAtanPi()
    {
        MeasurePerformance("AtanPi", () =>
        {
            Result = BurstMath.AtanPiLow(X);
        });
    }

    [Test, Performance]
    [Category("AtanPi")]
    public void BurstMediumAtanPi()
    {
        MeasurePerformance("AtanPi", () =>
        {
            Result = BurstMath.AtanPiMedium(X);
        });
    }

    [Test, Performance]
    [Category("AtanPi")]
    public void BurstHighAtanPi()
    {
        MeasurePerformance("AtanPi", () =>
        {
            Result = BurstMath.AtanPiHigh(X);
        });
    }

    [Test, Performance]
    [Category("AtanPi")]
    public void CoreAtanPi()
    {
        MeasurePerformance("AtanPi", () =>
        {
            Result = StrictMath.AtanPi(X);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("AtanPi")]
    public void PInvokeAtanPi()
    {
        MeasurePerformance("AtanPi", () =>
        {
            Result = PInvoke.PInvoke.AtanPi(X);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("AtanPi")]
    public void BurstLowAtanPi()
    {
        MeasurePerformance("AtanPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AtanPiLow(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("AtanPi")]
    public void BurstMediumAtanPi()
    {
        MeasurePerformance("AtanPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AtanPiMedium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("AtanPi")]
    public void BurstHighAtanPi()
    {
        MeasurePerformance("AtanPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AtanPiHigh(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("AtanPi")]
    public void CoreAtanPi()
    {
        MeasurePerformance("AtanPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.AtanPi(x);
            }

            Result = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("AtanPi")]
    public void PInvokeAtanPi()
    {
        MeasurePerformance("AtanPi", () =>
        {
            double sum = 0.0;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.AtanPi(x);
            }

            Result = sum;
        });
    }
#endif
}
