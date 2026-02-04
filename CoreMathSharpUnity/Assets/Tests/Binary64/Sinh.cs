using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Sinh")]
    public void BurstLowSinh()
    {
        MeasurePerformance("Sinh", () =>
        {
            Result = BurstMath.SinhLow(X);
        });
    }

    [Test, Performance]
    [Category("Sinh")]
    public void BurstMediumSinh()
    {
        MeasurePerformance("Sinh", () =>
        {
            Result = BurstMath.SinhMedium(X);
        });
    }

    [Test, Performance]
    [Category("Sinh")]
    public void BurstHighSinh()
    {
        MeasurePerformance("Sinh", () =>
        {
            Result = BurstMath.SinhHigh(X);
        });
    }

    [Test, Performance]
    [Category("Sinh")]
    public void CoreSinh()
    {
        MeasurePerformance("Sinh", () =>
        {
            Result = StrictMath.Sinh(X);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Sinh")]
    public void PInvokeSinh()
    {
        MeasurePerformance("Sinh", () =>
        {
            Result = PInvoke.PInvoke.Sinh(X);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Sinh")]
    public void BurstLowSinh()
    {
        MeasurePerformance("Sinh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.SinhLow(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Sinh")]
    public void BurstMediumSinh()
    {
        MeasurePerformance("Sinh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.SinhMedium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Sinh")]
    public void BurstHighSinh()
    {
        MeasurePerformance("Sinh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.SinhHigh(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Sinh")]
    public void CoreSinh()
    {
        MeasurePerformance("Sinh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Sinh(x);
            }

            Result = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Sinh")]
    public void PInvokeSinh()
    {
        MeasurePerformance("Sinh", () =>
        {
            double sum = 0.0;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.Sinh(x);
            }

            Result = sum;
        });
    }
#endif
}
