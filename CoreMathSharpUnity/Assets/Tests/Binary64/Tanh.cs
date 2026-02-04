using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Tanh")]
    public void BurstLowTanh()
    {
        MeasurePerformance("Tanh", () =>
        {
            Result = BurstMath.TanhLow(X);
        });
    }

    [Test, Performance]
    [Category("Tanh")]
    public void BurstMediumTanh()
    {
        MeasurePerformance("Tanh", () =>
        {
            Result = BurstMath.TanhMedium(X);
        });
    }

    [Test, Performance]
    [Category("Tanh")]
    public void BurstHighTanh()
    {
        MeasurePerformance("Tanh", () =>
        {
            Result = BurstMath.TanhHigh(X);
        });
    }

    [Test, Performance]
    [Category("Tanh")]
    public void CoreTanh()
    {
        MeasurePerformance("Tanh", () =>
        {
            Result = StrictMath.Tanh(X);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Tanh")]
    public void PInvokeTanh()
    {
        MeasurePerformance("Tanh", () =>
        {
            Result = PInvoke.PInvoke.Tanh(X);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Tanh")]
    public void BurstLowTanh()
    {
        MeasurePerformance("Tanh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.TanhLow(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Tanh")]
    public void BurstMediumTanh()
    {
        MeasurePerformance("Tanh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.TanhMedium(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Tanh")]
    public void BurstHighTanh()
    {
        MeasurePerformance("Tanh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += BurstMath.TanhHigh(x);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Tanh")]
    public void CoreTanh()
    {
        MeasurePerformance("Tanh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Tanh(x);
            }

            Result = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Tanh")]
    public void PInvokeTanh()
    {
        MeasurePerformance("Tanh", () =>
        {
            double sum = 0.0;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.Tanh(x);
            }

            Result = sum;
        });
    }
#endif
}
