using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("ReciprocalSqrt")]
    public void BurstLowReciprocalSqrt()
    {
        MeasurePerformance("ReciprocalSqrt", () =>
        {
            Result = BurstMath.ReciprocalSqrtLow(X);
        });
    }

    [Test, Performance]
    [Category("ReciprocalSqrt")]
    public void BurstMediumReciprocalSqrt()
    {
        MeasurePerformance("ReciprocalSqrt", () =>
        {
            Result = BurstMath.ReciprocalSqrtMedium(X);
        });
    }

    [Test, Performance]
    [Category("ReciprocalSqrt")]
    public void BurstHighReciprocalSqrt()
    {
        MeasurePerformance("ReciprocalSqrt", () =>
        {
            Result = BurstMath.ReciprocalSqrtHigh(X);
        });
    }

    [Test, Performance]
    [Category("ReciprocalSqrt")]
    public void CoreReciprocalSqrt()
    {
        MeasurePerformance("ReciprocalSqrt", () =>
        {
            Result = StrictMath.ReciprocalSqrt(X);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("ReciprocalSqrt")]
    public void PInvokeReciprocalSqrt()
    {
        MeasurePerformance("ReciprocalSqrt", () =>
        {
            Result = PInvoke.PInvoke.ReciprocalSqrt(X);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("ReciprocalSqrt")]
    public void BurstLowReciprocalSqrt()
    {
        MeasurePerformance("ReciprocalSqrt", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.ReciprocalSqrtLow(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("ReciprocalSqrt")]
    public void BurstMediumReciprocalSqrt()
    {
        MeasurePerformance("ReciprocalSqrt", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.ReciprocalSqrtMedium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("ReciprocalSqrt")]
    public void BurstHighReciprocalSqrt()
    {
        MeasurePerformance("ReciprocalSqrt", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.ReciprocalSqrtHigh(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("ReciprocalSqrt")]
    public void CoreReciprocalSqrt()
    {
        MeasurePerformance("ReciprocalSqrt", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.ReciprocalSqrt(x);
            }

            Result = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("ReciprocalSqrt")]
    public void PInvokeReciprocalSqrt()
    {
        MeasurePerformance("ReciprocalSqrt", () =>
        {
            double sum = 0.0;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.ReciprocalSqrt(x);
            }

            Result = sum;
        });
    }
#endif
}
