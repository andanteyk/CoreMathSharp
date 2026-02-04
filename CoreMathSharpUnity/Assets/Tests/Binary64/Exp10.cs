using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Exp10")]
    public void BurstLowExp10()
    {
        MeasurePerformance("Exp10", () =>
        {
            Result = BurstMath.Exp10Low(X);
        });
    }

    [Test, Performance]
    [Category("Exp10")]
    public void BurstMediumExp10()
    {
        MeasurePerformance("Exp10", () =>
        {
            Result = BurstMath.Exp10Medium(X);
        });
    }

    [Test, Performance]
    [Category("Exp10")]
    public void BurstHighExp10()
    {
        MeasurePerformance("Exp10", () =>
        {
            Result = BurstMath.Exp10High(X);
        });
    }

    [Test, Performance]
    [Category("Exp10")]
    public void CoreExp10()
    {
        MeasurePerformance("Exp10", () =>
        {
            Result = StrictMath.Exp10(X);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Exp10")]
    public void PInvokeExp10()
    {
        MeasurePerformance("Exp10", () =>
        {
            Result = PInvoke.PInvoke.Exp10(X);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Exp10")]
    public void BurstLowExp10()
    {
        MeasurePerformance("Exp10", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.Exp10Low(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Exp10")]
    public void BurstMediumExp10()
    {
        MeasurePerformance("Exp10", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.Exp10Medium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Exp10")]
    public void BurstHighExp10()
    {
        MeasurePerformance("Exp10", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.Exp10High(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Exp10")]
    public void CoreExp10()
    {
        MeasurePerformance("Exp10", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Exp10(x);
            }

            Result = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Exp10")]
    public void PInvokeExp10()
    {
        MeasurePerformance("Exp10", () =>
        {
            double sum = 0.0;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.Exp10(x);
            }

            Result = sum;
        });
    }
#endif
}
