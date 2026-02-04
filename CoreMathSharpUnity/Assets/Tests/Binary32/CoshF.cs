using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("CoshF")]
    public void MathematicsCoshF()
    {
        MeasurePerformance("CoshF", () =>
        {
            ResultF = math.cosh(XF);
        });
    }

    [Test, Performance]
    [Category("CoshF")]
    public void BurstLowCoshF()
    {
        MeasurePerformance("CoshF", () =>
        {
            ResultF = BurstMathF.CoshLow(XF);
        });
    }

    [Test, Performance]
    [Category("CoshF")]
    public void BurstMediumCoshF()
    {
        MeasurePerformance("CoshF", () =>
        {
            ResultF = BurstMathF.CoshMedium(XF);
        });
    }

    [Test, Performance]
    [Category("CoshF")]
    public void BurstHighCoshF()
    {
        MeasurePerformance("CoshF", () =>
        {
            ResultF = BurstMathF.CoshHigh(XF);
        });
    }

    [Test, Performance]
    [Category("CoshF")]
    public void CoreCoshF()
    {
        MeasurePerformance("CoshF", () =>
        {
            ResultF = StrictMathF.Cosh(XF);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("CoshF")]
    public void PInvokeCoshF()
    {
        MeasurePerformance("CoshF", () =>
        {
            ResultF = PInvoke.PInvoke.CoshF(XF);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("CoshF")]
    public void MathematicsCoshF()
    {
        MeasurePerformance("CoshF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.cosh(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CoshF")]
    public void BurstLowCoshF()
    {
        MeasurePerformance("CoshF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.CoshLow(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CoshF")]
    public void BurstMediumCoshF()
    {
        MeasurePerformance("CoshF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.CoshMedium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CoshF")]
    public void BurstHighCoshF()
    {
        MeasurePerformance("CoshF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.CoshHigh(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CoshF")]
    public void CoreCoshF()
    {
        MeasurePerformance("CoshF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Cosh(x);
            }

            ResultF = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("CoshF")]
    public void PInvokeCoshF()
    {
        MeasurePerformance("CoshF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.CoshF(x);
            }

            ResultF = sum;
        });
    }
#endif
}
