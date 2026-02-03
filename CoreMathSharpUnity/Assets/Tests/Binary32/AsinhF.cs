using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("AsinhF")]
    public void BurstLowAsinhF()
    {
        MeasurePerformance("AsinhF", () =>
        {
            ResultF = BurstMathF.AsinhLow(XF);
        });
    }

    [Test, Performance]
    [Category("AsinhF")]
    public void BurstMediumAsinhF()
    {
        MeasurePerformance("AsinhF", () =>
        {
            ResultF = BurstMathF.AsinhMedium(XF);
        });
    }

    [Test, Performance]
    [Category("AsinhF")]
    public void BurstHighAsinhF()
    {
        MeasurePerformance("AsinhF", () =>
        {
            ResultF = BurstMathF.AsinhHigh(XF);
        });
    }

    [Test, Performance]
    [Category("AsinhF")]
    public void CoreAsinhF()
    {
        MeasurePerformance("AsinhF", () =>
        {
            ResultF = StrictMathF.Asinh(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("AsinhF")]
    public void BurstLowAsinhF()
    {
        MeasurePerformance("AsinhF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AsinhLow(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AsinhF")]
    public void BurstMediumAsinhF()
    {
        MeasurePerformance("AsinhF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AsinhMedium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AsinhF")]
    public void BurstHighAsinhF()
    {
        MeasurePerformance("AsinhF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AsinhHigh(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AsinhF")]
    public void CoreAsinhF()
    {
        MeasurePerformance("AsinhF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Asinh(x);
            }

            ResultF = sum;
        });
    }
}
