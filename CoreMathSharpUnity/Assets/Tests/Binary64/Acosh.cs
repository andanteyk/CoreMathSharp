using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Acosh")]
    public void BurstLowAcosh()
    {
        MeasurePerformance("Acosh", () =>
        {
            Result = BurstMath.AcoshLow(X);
        });
    }

    [Test, Performance]
    [Category("Acosh")]
    public void BurstMediumAcosh()
    {
        MeasurePerformance("Acosh", () =>
        {
            Result = BurstMath.AcoshMedium(X);
        });
    }

    [Test, Performance]
    [Category("Acosh")]
    public void BurstHighAcosh()
    {
        MeasurePerformance("Acosh", () =>
        {
            Result = BurstMath.AcoshHigh(X);
        });
    }

    [Test, Performance]
    [Category("Acosh")]
    public void CoreAcosh()
    {
        MeasurePerformance("Acosh", () =>
        {
            Result = StrictMath.Acosh(X);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Acosh")]
    public void PInvokeAcosh()
    {
        MeasurePerformance("Acosh", () =>
        {
            Result = PInvoke.PInvoke.Acosh(X);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Acosh")]
    public void BurstLowAcosh()
    {
        MeasurePerformance("Acosh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AcoshLow(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Acosh")]
    public void BurstMediumAcosh()
    {
        MeasurePerformance("Acosh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AcoshMedium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Acosh")]
    public void BurstHighAcosh()
    {
        MeasurePerformance("Acosh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AcoshHigh(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Acosh")]
    public void CoreAcosh()
    {
        MeasurePerformance("Acosh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Acosh(x);
            }

            Result = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Acosh")]
    public void PInvokeAcosh()
    {
        MeasurePerformance("Acosh", () =>
        {
            double sum = 0.0;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.Acosh(x);
            }

            Result = sum;
        });
    }
#endif
}
