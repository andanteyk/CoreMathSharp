using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Atanh")]
    public void BurstLowAtanh()
    {
        MeasurePerformance("Atanh", () =>
        {
            Result = BurstMath.AtanhLow(X);
        });
    }

    [Test, Performance]
    [Category("Atanh")]
    public void BurstMediumAtanh()
    {
        MeasurePerformance("Atanh", () =>
        {
            Result = BurstMath.AtanhMedium(X);
        });
    }

    [Test, Performance]
    [Category("Atanh")]
    public void BurstHighAtanh()
    {
        MeasurePerformance("Atanh", () =>
        {
            Result = BurstMath.AtanhHigh(X);
        });
    }

    [Test, Performance]
    [Category("Atanh")]
    public void CoreAtanh()
    {
        MeasurePerformance("Atanh", () =>
        {
            Result = StrictMath.Atanh(X);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Atanh")]
    public void PInvokeAtanh()
    {
        MeasurePerformance("Atanh", () =>
        {
            Result = PInvoke.PInvoke.Atanh(X);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Atanh")]
    public void BurstLowAtanh()
    {
        MeasurePerformance("Atanh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AtanhLow(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Atanh")]
    public void BurstMediumAtanh()
    {
        MeasurePerformance("Atanh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AtanhMedium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Atanh")]
    public void BurstHighAtanh()
    {
        MeasurePerformance("Atanh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AtanhHigh(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Atanh")]
    public void CoreAtanh()
    {
        MeasurePerformance("Atanh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Atanh(x);
            }

            Result = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Atanh")]
    public void PInvokeAtanh()
    {
        MeasurePerformance("Atanh", () =>
        {
            double sum = 0.0;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.Atanh(x);
            }

            Result = sum;
        });
    }
#endif
}
