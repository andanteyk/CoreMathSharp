using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("CbrtF")]
    public void UnityCbrtF()
    {
        MeasurePerformance("CbrtF", () =>
        {
            ResultF = Mathf.Pow(XF, 1f / 3f);
        });
    }

    [Test, Performance]
    [Category("CbrtF")]
    public void MathematicsCbrtF()
    {
        MeasurePerformance("CbrtF", () =>
        {
            ResultF = math.pow(XF, 1f / 3f);
        });
    }

    [Test, Performance]
    [Category("CbrtF")]
    public void BurstLowCbrtF()
    {
        MeasurePerformance("CbrtF", () =>
        {
            ResultF = BurstMathF.CbrtLow(XF);
        });
    }

    [Test, Performance]
    [Category("CbrtF")]
    public void BurstMediumCbrtF()
    {
        MeasurePerformance("CbrtF", () =>
        {
            ResultF = BurstMathF.CbrtMedium(XF);
        });
    }

    [Test, Performance]
    [Category("CbrtF")]
    public void BurstHighCbrtF()
    {
        MeasurePerformance("CbrtF", () =>
        {
            ResultF = BurstMathF.CbrtHigh(XF);
        });
    }

    [Test, Performance]
    [Category("CbrtF")]
    public void CoreCbrtF()
    {
        MeasurePerformance("CbrtF", () =>
        {
            ResultF = StrictMathF.Cbrt(XF);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("CbrtF")]
    public void PInvokeCbrtF()
    {
        MeasurePerformance("CbrtF", () =>
        {
            ResultF = PInvoke.PInvoke.CbrtF(XF);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("CbrtF")]
    public void UnityCbrtF()
    {
        MeasurePerformance("CbrtF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Pow(x, 1f / 3f);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CbrtF")]
    public void MathematicsCbrtF()
    {
        MeasurePerformance("CbrtF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.pow(x, 1f / 3f);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CbrtF")]
    public void BurstLowCbrtF()
    {
        MeasurePerformance("CbrtF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.CbrtLow(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CbrtF")]
    public void BurstMediumCbrtF()
    {
        MeasurePerformance("CbrtF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.CbrtMedium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CbrtF")]
    public void BurstHighCbrtF()
    {
        MeasurePerformance("CbrtF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.CbrtHigh(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CbrtF")]
    public void CoreCbrtF()
    {
        MeasurePerformance("CbrtF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Cbrt(x);
            }

            ResultF = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("CbrtF")]
    public void PInvokeCbrtF()
    {
        MeasurePerformance("CbrtF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.CbrtF(x);
            }

            ResultF = sum;
        });
    }
#endif
}
