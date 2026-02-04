using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Log1P")]
    public void BurstLowLog1P()
    {
        MeasurePerformance("Log1P", () =>
        {
            Result = BurstMath.Log1PLow(X);
        });
    }

    [Test, Performance]
    [Category("Log1P")]
    public void BurstMediumLog1P()
    {
        MeasurePerformance("Log1P", () =>
        {
            Result = BurstMath.Log1PMedium(X);
        });
    }

    [Test, Performance]
    [Category("Log1P")]
    public void BurstHighLog1P()
    {
        MeasurePerformance("Log1P", () =>
        {
            Result = BurstMath.Log1PHigh(X);
        });
    }

    [Test, Performance]
    [Category("Log1P")]
    public void CoreLog1P()
    {
        MeasurePerformance("Log1P", () =>
        {
            Result = StrictMath.Log1P(X);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Log1P")]
    public void PInvokeLog1P()
    {
        MeasurePerformance("Log1P", () =>
        {
            Result = PInvoke.PInvoke.Log1P(X);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Log1P")]
    public void BurstLowLog1P()
    {
        MeasurePerformance("Log1P", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.Log1PLow(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Log1P")]
    public void BurstMediumLog1P()
    {
        MeasurePerformance("Log1P", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.Log1PMedium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Log1P")]
    public void BurstHighLog1P()
    {
        MeasurePerformance("Log1P", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.Log1PHigh(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Log1P")]
    public void CoreLog1P()
    {
        MeasurePerformance("Log1P", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Log1P(x);
            }

            Result = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Log1P")]
    public void PInvokeLog1P()
    {
        MeasurePerformance("Log1P", () =>
        {
            double sum = 0.0;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.Log1P(x);
            }

            Result = sum;
        });
    }
#endif
}
