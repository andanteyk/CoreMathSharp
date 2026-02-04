using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("AtanhF")]
    public void BurstLowAtanhF()
    {
        MeasurePerformance("AtanhF", () =>
        {
            ResultF = BurstMathF.AtanhLow(XF);
        });
    }

    [Test, Performance]
    [Category("AtanhF")]
    public void BurstMediumAtanhF()
    {
        MeasurePerformance("AtanhF", () =>
        {
            ResultF = BurstMathF.AtanhMedium(XF);
        });
    }

    [Test, Performance]
    [Category("AtanhF")]
    public void BurstHighAtanhF()
    {
        MeasurePerformance("AtanhF", () =>
        {
            ResultF = BurstMathF.AtanhHigh(XF);
        });
    }

    [Test, Performance]
    [Category("AtanhF")]
    public void CoreAtanhF()
    {
        MeasurePerformance("AtanhF", () =>
        {
            ResultF = StrictMathF.Atanh(XF);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("AtanhF")]
    public void PInvokeAtanhF()
    {
        MeasurePerformance("AtanhF", () =>
        {
            ResultF = PInvoke.PInvoke.AtanhF(XF);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("AtanhF")]
    public void BurstLowAtanhF()
    {
        MeasurePerformance("AtanhF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AtanhLow(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AtanhF")]
    public void BurstMediumAtanhF()
    {
        MeasurePerformance("AtanhF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AtanhMedium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AtanhF")]
    public void BurstHighAtanhF()
    {
        MeasurePerformance("AtanhF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AtanhHigh(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AtanhF")]
    public void CoreAtanhF()
    {
        MeasurePerformance("AtanhF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Atanh(x);
            }

            ResultF = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("AtanhF")]
    public void PInvokeAtanhF()
    {
        MeasurePerformance("AtanhF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.AtanhF(x);
            }

            ResultF = sum;
        });
    }
#endif
}
