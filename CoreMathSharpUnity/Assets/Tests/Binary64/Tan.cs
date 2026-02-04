using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Tan")]
    public void BurstLowTan()
    {
        MeasurePerformance("Tan", () =>
        {
            Result = BurstMath.TanLow(X);
        });
    }

    [Test, Performance]
    [Category("Tan")]
    public void BurstMediumTan()
    {
        MeasurePerformance("Tan", () =>
        {
            Result = BurstMath.TanMedium(X);
        });
    }

    [Test, Performance]
    [Category("Tan")]
    public void BurstHighTan()
    {
        MeasurePerformance("Tan", () =>
        {
            Result = BurstMath.TanHigh(X);
        });
    }

    [Test, Performance]
    [Category("Tan")]
    public void CoreTan()
    {
        MeasurePerformance("Tan", () =>
        {
            Result = StrictMath.Tan(X);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Tan")]
    public void PInvokeTan()
    {
        MeasurePerformance("Tan", () =>
        {
            Result = PInvoke.PInvoke.Tan(X);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Tan")]
    public void BurstLowTan()
    {
        MeasurePerformance("Tan", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.TanLow(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Tan")]
    public void BurstMediumTan()
    {
        MeasurePerformance("Tan", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.TanMedium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Tan")]
    public void BurstHighTan()
    {
        MeasurePerformance("Tan", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.TanHigh(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Tan")]
    public void CoreTan()
    {
        MeasurePerformance("Tan", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Tan(x);
            }

            Result = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Tan")]
    public void PInvokeTan()
    {
        MeasurePerformance("Tan", () =>
        {
            double sum = 0.0;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.Tan(x);
            }

            Result = sum;
        });
    }
#endif
}
