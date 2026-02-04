using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Exp2M1")]
    public void BurstLowExp2M1()
    {
        MeasurePerformance("Exp2M1", () =>
        {
            Result = BurstMath.Exp2M1Low(X);
        });
    }

    [Test, Performance]
    [Category("Exp2M1")]
    public void BurstMediumExp2M1()
    {
        MeasurePerformance("Exp2M1", () =>
        {
            Result = BurstMath.Exp2M1Medium(X);
        });
    }

    [Test, Performance]
    [Category("Exp2M1")]
    public void BurstHighExp2M1()
    {
        MeasurePerformance("Exp2M1", () =>
        {
            Result = BurstMath.Exp2M1High(X);
        });
    }

    [Test, Performance]
    [Category("Exp2M1")]
    public void CoreExp2M1()
    {
        MeasurePerformance("Exp2M1", () =>
        {
            Result = StrictMath.Exp2M1(X);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Exp2M1")]
    public void PInvokeExp2M1()
    {
        MeasurePerformance("Exp2M1", () =>
        {
            Result = PInvoke.PInvoke.Exp2M1(X);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Exp2M1")]
    public void BurstLowExp2M1()
    {
        MeasurePerformance("Exp2M1", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.Exp2M1Low(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Exp2M1")]
    public void BurstMediumExp2M1()
    {
        MeasurePerformance("Exp2M1", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.Exp2M1Medium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Exp2M1")]
    public void BurstHighExp2M1()
    {
        MeasurePerformance("Exp2M1", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.Exp2M1High(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Exp2M1")]
    public void CoreExp2M1()
    {
        MeasurePerformance("Exp2M1", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Exp2M1(x);
            }

            Result = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Exp2M1")]
    public void PInvokeExp2M1()
    {
        MeasurePerformance("Exp2M1", () =>
        {
            double sum = 0.0;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.Exp2M1(x);
            }

            Result = sum;
        });
    }
#endif
}
