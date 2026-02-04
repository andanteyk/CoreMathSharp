using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Sqrt")]
    public void BurstLowSqrt()
    {
        MeasurePerformance("Sqrt", () =>
        {
            Result = BurstMath.SqrtLow(X);
        });
    }

    [Test, Performance]
    [Category("Sqrt")]
    public void BurstMediumSqrt()
    {
        MeasurePerformance("Sqrt", () =>
        {
            Result = BurstMath.SqrtMedium(X);
        });
    }

    [Test, Performance]
    [Category("Sqrt")]
    public void BurstHighSqrt()
    {
        MeasurePerformance("Sqrt", () =>
        {
            Result = BurstMath.SqrtHigh(X);
        });
    }

    [Test, Performance]
    [Category("Sqrt")]
    public void CoreSqrt()
    {
        MeasurePerformance("Sqrt", () =>
        {
            Result = StrictMath.Sqrt(X);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Sqrt")]
    public void PInvokeSqrt()
    {
        MeasurePerformance("Sqrt", () =>
        {
            Result = PInvoke.PInvoke.Sqrt(X);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Sqrt")]
    public void BurstLowSqrt()
    {
        MeasurePerformance("Sqrt", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.SqrtLow(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Sqrt")]
    public void BurstMediumSqrt()
    {
        MeasurePerformance("Sqrt", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.SqrtMedium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Sqrt")]
    public void BurstHighSqrt()
    {
        MeasurePerformance("Sqrt", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.SqrtHigh(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Sqrt")]
    public void CoreSqrt()
    {
        MeasurePerformance("Sqrt", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Sqrt(x);
            }

            Result = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Sqrt")]
    public void PInvokeSqrt()
    {
        MeasurePerformance("Sqrt", () =>
        {
            double sum = 0.0;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.Sqrt(x);
            }

            Result = sum;
        });
    }
#endif
}
