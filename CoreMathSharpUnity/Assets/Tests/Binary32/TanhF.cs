using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("TanhF")]
    public void MathematicsTanhF()
    {
        MeasurePerformance("TanhF", () =>
        {
            ResultF = math.tanh(XF);
        });
    }

    [Test, Performance]
    [Category("TanhF")]
    public void BurstLowTanhF()
    {
        MeasurePerformance("TanhF", () =>
        {
            ResultF = BurstMathF.TanhLow(XF);
        });
    }

    [Test, Performance]
    [Category("TanhF")]
    public void BurstMediumTanhF()
    {
        MeasurePerformance("TanhF", () =>
        {
            ResultF = BurstMathF.TanhMedium(XF);
        });
    }

    [Test, Performance]
    [Category("TanhF")]
    public void BurstHighTanhF()
    {
        MeasurePerformance("TanhF", () =>
        {
            ResultF = BurstMathF.TanhHigh(XF);
        });
    }

    [Test, Performance]
    [Category("TanhF")]
    public void CoreTanhF()
    {
        MeasurePerformance("TanhF", () =>
        {
            ResultF = StrictMathF.Tanh(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("TanhF")]
    public void MathematicsTanhF()
    {
        MeasurePerformance("TanhF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.tanh(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("TanhF")]
    public void BurstLowTanhF()
    {
        MeasurePerformance("TanhF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.TanhLow(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("TanhF")]
    public void BurstMediumTanhF()
    {
        MeasurePerformance("TanhF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.TanhMedium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("TanhF")]
    public void BurstHighTanhF()
    {
        MeasurePerformance("TanhF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.TanhHigh(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("TanhF")]
    public void CoreTanhF()
    {
        MeasurePerformance("TanhF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Tanh(x);
            }

            ResultF = sum;
        });
    }
}
