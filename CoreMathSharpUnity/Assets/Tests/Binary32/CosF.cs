using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("CosF")]
    public void UnityCosF()
    {
        MeasurePerformance("CosF", () =>
        {
            ResultF = Mathf.Cos(XF);
        });
    }

    [Test, Performance]
    [Category("CosF")]
    public void MathematicsCosF()
    {
        MeasurePerformance("CosF", () =>
        {
            ResultF = math.cos(XF);
        });
    }

    [Test, Performance]
    [Category("CosF")]
    public void BurstLowCosF()
    {
        MeasurePerformance("CosF", () =>
        {
            ResultF = BurstMathF.CosLow(XF);
        });
    }

    [Test, Performance]
    [Category("CosF")]
    public void BurstMediumCosF()
    {
        MeasurePerformance("CosF", () =>
        {
            ResultF = BurstMathF.CosMedium(XF);
        });
    }

    [Test, Performance]
    [Category("CosF")]
    public void BurstHighCosF()
    {
        MeasurePerformance("CosF", () =>
        {
            ResultF = BurstMathF.CosHigh(XF);
        });
    }

    [Test, Performance]
    [Category("CosF")]
    public void CoreCosF()
    {
        MeasurePerformance("CosF", () =>
        {
            ResultF = StrictMathF.Cos(XF);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("CosF")]
    public void PInvokeCosF()
    {
        MeasurePerformance("CosF", () =>
        {
            ResultF = PInvoke.PInvoke.CosF(XF);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("CosF")]
    public void UnityCosF()
    {
        MeasurePerformance("CosF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Cos(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CosF")]
    public void MathematicsCosF()
    {
        MeasurePerformance("CosF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.cos(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CosF")]
    public void BurstLowCosF()
    {
        MeasurePerformance("CosF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.CosLow(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CosF")]
    public void BurstMediumCosF()
    {
        MeasurePerformance("CosF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.CosMedium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CosF")]
    public void BurstHighCosF()
    {
        MeasurePerformance("CosF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.CosHigh(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CosF")]
    public void CoreCosF()
    {
        MeasurePerformance("CosF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Cos(x);
            }

            ResultF = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("CosF")]
    public void PInvokeCosF()
    {
        MeasurePerformance("CosF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.CosF(x);
            }

            ResultF = sum;
        });
    }
#endif
}
