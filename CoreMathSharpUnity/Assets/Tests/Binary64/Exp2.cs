using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Exp2")]
    public void BurstLowExp2()
    {
        MeasurePerformance("Exp2", () =>
        {
            Result = BurstMath.Exp2Low(X);
        });
    }

    [Test, Performance]
    [Category("Exp2")]
    public void BurstMediumExp2()
    {
        MeasurePerformance("Exp2", () =>
        {
            Result = BurstMath.Exp2Medium(X);
        });
    }

    [Test, Performance]
    [Category("Exp2")]
    public void BurstHighExp2()
    {
        MeasurePerformance("Exp2", () =>
        {
            Result = BurstMath.Exp2High(X);
        });
    }

    [Test, Performance]
    [Category("Exp2")]
    public void CoreExp2()
    {
        MeasurePerformance("Exp2", () =>
        {
            Result = StrictMath.Exp2(X);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Exp2")]
    public void PInvokeExp2()
    {
        MeasurePerformance("Exp2", () =>
        {
            Result = PInvoke.PInvoke.Exp2(X);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Exp2")]
    public void BurstLowExp2()
    {
        MeasurePerformance("Exp2", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.Exp2Low(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Exp2")]
    public void BurstMediumExp2()
    {
        MeasurePerformance("Exp2", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.Exp2Medium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Exp2")]
    public void BurstHighExp2()
    {
        MeasurePerformance("Exp2", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.Exp2High(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Exp2")]
    public void CoreExp2()
    {
        MeasurePerformance("Exp2", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Exp2(x);
            }

            Result = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Exp2")]
    public void PInvokeExp2()
    {
        MeasurePerformance("Exp2", () =>
        {
            double sum = 0.0;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.Exp2(x);
            }

            Result = sum;
        });
    }
#endif
}
