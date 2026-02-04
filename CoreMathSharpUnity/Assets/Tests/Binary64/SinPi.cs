using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("SinPi")]
    public void BurstLowSinPi()
    {
        MeasurePerformance("SinPi", () =>
        {
            Result = BurstMath.SinPiLow(X);
        });
    }

    [Test, Performance]
    [Category("SinPi")]
    public void BurstMediumSinPi()
    {
        MeasurePerformance("SinPi", () =>
        {
            Result = BurstMath.SinPiMedium(X);
        });
    }

    [Test, Performance]
    [Category("SinPi")]
    public void BurstHighSinPi()
    {
        MeasurePerformance("SinPi", () =>
        {
            Result = BurstMath.SinPiHigh(X);
        });
    }

    [Test, Performance]
    [Category("SinPi")]
    public void CoreSinPi()
    {
        MeasurePerformance("SinPi", () =>
        {
            Result = StrictMath.SinPi(X);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("SinPi")]
    public void PInvokeSinPi()
    {
        MeasurePerformance("SinPi", () =>
        {
            Result = PInvoke.PInvoke.SinPi(X);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("SinPi")]
    public void BurstLowSinPi()
    {
        MeasurePerformance("SinPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.SinPiLow(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("SinPi")]
    public void BurstMediumSinPi()
    {
        MeasurePerformance("SinPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.SinPiMedium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("SinPi")]
    public void BurstHighSinPi()
    {
        MeasurePerformance("SinPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.SinPiHigh(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("SinPi")]
    public void CoreSinPi()
    {
        MeasurePerformance("SinPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.SinPi(x);
            }

            Result = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("SinPi")]
    public void PInvokeSinPi()
    {
        MeasurePerformance("SinPi", () =>
        {
            double sum = 0.0;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.SinPi(x);
            }

            Result = sum;
        });
    }
#endif
}
