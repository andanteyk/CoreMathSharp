using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("TanPi")]
    public void BurstLowTanPi()
    {
        MeasurePerformance("TanPi", () =>
        {
            Result = BurstMath.TanPiLow(X);
        });
    }

    [Test, Performance]
    [Category("TanPi")]
    public void BurstMediumTanPi()
    {
        MeasurePerformance("TanPi", () =>
        {
            Result = BurstMath.TanPiMedium(X);
        });
    }

    [Test, Performance]
    [Category("TanPi")]
    public void BurstHighTanPi()
    {
        MeasurePerformance("TanPi", () =>
        {
            Result = BurstMath.TanPiHigh(X);
        });
    }

    [Test, Performance]
    [Category("TanPi")]
    public void CoreTanPi()
    {
        MeasurePerformance("TanPi", () =>
        {
            Result = StrictMath.TanPi(X);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("TanPi")]
    public void PInvokeTanPi()
    {
        MeasurePerformance("TanPi", () =>
        {
            Result = PInvoke.PInvoke.TanPi(X);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("TanPi")]
    public void BurstLowTanPi()
    {
        MeasurePerformance("TanPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.TanPiLow(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("TanPi")]
    public void BurstMediumTanPi()
    {
        MeasurePerformance("TanPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.TanPiMedium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("TanPi")]
    public void BurstHighTanPi()
    {
        MeasurePerformance("TanPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.TanPiHigh(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("TanPi")]
    public void CoreTanPi()
    {
        MeasurePerformance("TanPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.TanPi(x);
            }

            Result = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("TanPi")]
    public void PInvokeTanPi()
    {
        MeasurePerformance("TanPi", () =>
        {
            double sum = 0.0;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.TanPi(x);
            }

            Result = sum;
        });
    }
#endif
}
