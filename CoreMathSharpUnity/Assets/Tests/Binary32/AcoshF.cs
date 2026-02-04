using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("AcoshF")]
    public void BurstLowAcoshF()
    {
        MeasurePerformance("AcoshF", () =>
        {
            ResultF = BurstMathF.AcoshLow(XF);
        });
    }

    [Test, Performance]
    [Category("AcoshF")]
    public void BurstMediumAcoshF()
    {
        MeasurePerformance("AcoshF", () =>
        {
            ResultF = BurstMathF.AcoshMedium(XF);
        });
    }

    [Test, Performance]
    [Category("AcoshF")]
    public void BurstHighAcoshF()
    {
        MeasurePerformance("AcoshF", () =>
        {
            ResultF = BurstMathF.AcoshHigh(XF);
        });
    }

    [Test, Performance]
    [Category("AcoshF")]
    public void CoreAcoshF()
    {
        MeasurePerformance("AcoshF", () =>
        {
            ResultF = StrictMathF.Acosh(XF);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("AcoshF")]
    public void PInvokeAcoshF()
    {
        MeasurePerformance("AcoshF", () =>
        {
            ResultF = PInvoke.PInvoke.AcoshF(XF);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("AcoshF")]
    public void BurstLowAcoshF()
    {
        MeasurePerformance("AcoshF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AcoshLow(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AcoshF")]
    public void BurstMediumAcoshF()
    {
        MeasurePerformance("AcoshF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AcoshMedium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AcoshF")]
    public void BurstHighAcoshF()
    {
        MeasurePerformance("AcoshF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AcoshHigh(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AcoshF")]
    public void CoreAcoshF()
    {
        MeasurePerformance("AcoshF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Acosh(x);
            }

            ResultF = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("AcoshF")]
    public void PInvokeAcoshF()
    {
        MeasurePerformance("AcoshF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.AcoshF(x);
            }

            ResultF = sum;
        });
    }
#endif
}
