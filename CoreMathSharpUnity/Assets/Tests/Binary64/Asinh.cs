using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Asinh")]
    public void BurstLowAsinh()
    {
        MeasurePerformance("Asinh", () =>
        {
            Result = BurstMath.AsinhLow(X);
        });
    }

    [Test, Performance]
    [Category("Asinh")]
    public void BurstMediumAsinh()
    {
        MeasurePerformance("Asinh", () =>
        {
            Result = BurstMath.AsinhMedium(X);
        });
    }

    [Test, Performance]
    [Category("Asinh")]
    public void BurstHighAsinh()
    {
        MeasurePerformance("Asinh", () =>
        {
            Result = BurstMath.AsinhHigh(X);
        });
    }

    [Test, Performance]
    [Category("Asinh")]
    public void CoreAsinh()
    {
        MeasurePerformance("Asinh", () =>
        {
            Result = StrictMath.Asinh(X);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Asinh")]
    public void PInvokeAsinh()
    {
        MeasurePerformance("Asinh", () =>
        {
            Result = PInvoke.PInvoke.Asinh(X);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Asinh")]
    public void BurstLowAsinh()
    {
        MeasurePerformance("Asinh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AsinhLow(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Asinh")]
    public void BurstMediumAsinh()
    {
        MeasurePerformance("Asinh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AsinhMedium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Asinh")]
    public void BurstHighAsinh()
    {
        MeasurePerformance("Asinh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.AsinhHigh(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Asinh")]
    public void CoreAsinh()
    {
        MeasurePerformance("Asinh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Asinh(x);
            }

            Result = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Asinh")]
    public void PInvokeAsinh()
    {
        MeasurePerformance("Asinh", () =>
        {
            double sum = 0.0;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.Asinh(x);
            }

            Result = sum;
        });
    }
#endif
}
