using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("SinF")]
    public void UnitySinF()
    {
        MeasurePerformance("SinF", () =>
        {
            ResultF = Mathf.Sin(XF);
        });
    }

    [Test, Performance]
    [Category("SinF")]
    public void MathematicsSinF()
    {
        MeasurePerformance("SinF", () =>
        {
            ResultF = math.sin(XF);
        });
    }

    [Test, Performance]
    [Category("SinF")]
    public void BurstLowSinF()
    {
        MeasurePerformance("SinF", () =>
        {
            ResultF = BurstMathF.SinLow(XF);
        });
    }

    [Test, Performance]
    [Category("SinF")]
    public void BurstMediumSinF()
    {
        MeasurePerformance("SinF", () =>
        {
            ResultF = BurstMathF.SinMedium(XF);
        });
    }

    [Test, Performance]
    [Category("SinF")]
    public void BurstHighSinF()
    {
        MeasurePerformance("SinF", () =>
        {
            ResultF = BurstMathF.SinHigh(XF);
        });
    }

    [Test, Performance]
    [Category("SinF")]
    public void CoreSinF()
    {
        MeasurePerformance("SinF", () =>
        {
            ResultF = StrictMathF.Sin(XF);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("SinF")]
    public void PInvokeSinF()
    {
        MeasurePerformance("SinF", () =>
        {
            ResultF = PInvoke.PInvoke.SinF(XF);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("SinF")]
    public void UnitySinF()
    {
        MeasurePerformance("SinF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Sin(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("SinF")]
    public void MathematicsSinF()
    {
        MeasurePerformance("SinF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.sin(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("SinF")]
    public void BurstLowSinF()
    {
        MeasurePerformance("SinF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.SinLow(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("SinF")]
    public void BurstMediumSinF()
    {
        MeasurePerformance("SinF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.SinMedium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("SinF")]
    public void BurstHighSinF()
    {
        MeasurePerformance("SinF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.SinHigh(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("SinF")]
    public void CoreSinF()
    {
        MeasurePerformance("SinF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Sin(x);
            }

            ResultF = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("SinF")]
    public void PInvokeSinF()
    {
        MeasurePerformance("SinF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.SinF(x);
            }

            ResultF = sum;
        });
    }
#endif
}
