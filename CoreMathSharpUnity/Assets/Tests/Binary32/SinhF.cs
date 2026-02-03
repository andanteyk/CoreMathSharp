using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("SinhF")]
    public void MathematicsSinhF()
    {
        MeasurePerformance("SinhF", () =>
        {
            ResultF = math.sinh(XF);
        });
    }

    [Test, Performance]
    [Category("SinhF")]
    public void BurstLowSinhF()
    {
        MeasurePerformance("SinhF", () =>
        {
            ResultF = BurstMathF.SinhLow(XF);
        });
    }

    [Test, Performance]
    [Category("SinhF")]
    public void BurstMediumSinhF()
    {
        MeasurePerformance("SinhF", () =>
        {
            ResultF = BurstMathF.SinhMedium(XF);
        });
    }

    [Test, Performance]
    [Category("SinhF")]
    public void BurstHighSinhF()
    {
        MeasurePerformance("SinhF", () =>
        {
            ResultF = BurstMathF.SinhHigh(XF);
        });
    }

    [Test, Performance]
    [Category("SinhF")]
    public void CoreSinhF()
    {
        MeasurePerformance("SinhF", () =>
        {
            ResultF = StrictMathF.Sinh(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("SinhF")]
    public void MathematicsSinhF()
    {
        MeasurePerformance("SinhF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.sinh(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("SinhF")]
    public void BurstLowSinhF()
    {
        MeasurePerformance("SinhF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.SinhLow(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("SinhF")]
    public void BurstMediumSinhF()
    {
        MeasurePerformance("SinhF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.SinhMedium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("SinhF")]
    public void BurstHighSinhF()
    {
        MeasurePerformance("SinhF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.SinhHigh(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("SinhF")]
    public void CoreSinhF()
    {
        MeasurePerformance("SinhF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Sinh(x);
            }

            ResultF = sum;
        });
    }
}
