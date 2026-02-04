using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Log")]
    public void BurstLowLog()
    {
        MeasurePerformance("Log", () =>
        {
            Result = BurstMath.LogLow(X);
        });
    }

    [Test, Performance]
    [Category("Log")]
    public void BurstMediumLog()
    {
        MeasurePerformance("Log", () =>
        {
            Result = BurstMath.LogMedium(X);
        });
    }

    [Test, Performance]
    [Category("Log")]
    public void BurstHighLog()
    {
        MeasurePerformance("Log", () =>
        {
            Result = BurstMath.LogHigh(X);
        });
    }

    [Test, Performance]
    [Category("Log")]
    public void CoreLog()
    {
        MeasurePerformance("Log", () =>
        {
            Result = StrictMath.Log(X);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Log")]
    public void PInvokeLog()
    {
        MeasurePerformance("Log", () =>
        {
            Result = PInvoke.PInvoke.Log(X);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Log")]
    public void BurstLowLog()
    {
        MeasurePerformance("Log", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.LogLow(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Log")]
    public void BurstMediumLog()
    {
        MeasurePerformance("Log", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.LogMedium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Log")]
    public void BurstHighLog()
    {
        MeasurePerformance("Log", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.LogHigh(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Log")]
    public void CoreLog()
    {
        MeasurePerformance("Log", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Log(x);
            }

            Result = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Log")]
    public void PInvokeLog()
    {
        MeasurePerformance("Log", () =>
        {
            double sum = 0.0;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.Log(x);
            }

            Result = sum;
        });
    }
#endif
}
