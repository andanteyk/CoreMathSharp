using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("AtanF")]
    public void UnityAtanF()
    {
        MeasurePerformance("AtanF", () =>
        {
            ResultF = Mathf.Atan(XF);
        });
    }

    [Test, Performance]
    [Category("AtanF")]
    public void MathematicsAtanF()
    {
        MeasurePerformance("AtanF", () =>
        {
            ResultF = math.atan(XF);
        });
    }

    [Test, Performance]
    [Category("AtanF")]
    public void BurstLowAtanF()
    {
        MeasurePerformance("AtanF", () =>
        {
            ResultF = BurstMathF.AtanLow(XF);
        });
    }

    [Test, Performance]
    [Category("AtanF")]
    public void BurstMediumAtanF()
    {
        MeasurePerformance("AtanF", () =>
        {
            ResultF = BurstMathF.AtanMedium(XF);
        });
    }

    [Test, Performance]
    [Category("AtanF")]
    public void BurstHighAtanF()
    {
        MeasurePerformance("AtanF", () =>
        {
            ResultF = BurstMathF.AtanHigh(XF);
        });
    }

    [Test, Performance]
    [Category("AtanF")]
    public void CoreAtanF()
    {
        MeasurePerformance("AtanF", () =>
        {
            ResultF = StrictMathF.Atan(XF);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("AtanF")]
    public void PInvokeAtanF()
    {
        MeasurePerformance("AtanF", () =>
        {
            ResultF = PInvoke.PInvoke.AtanF(XF);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("AtanF")]
    public void UnityAtanF()
    {
        MeasurePerformance("AtanF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Atan(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AtanF")]
    public void MathematicsAtanF()
    {
        MeasurePerformance("AtanF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.atan(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AtanF")]
    public void BurstLowAtanF()
    {
        MeasurePerformance("AtanF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AtanLow(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AtanF")]
    public void BurstMediumAtanF()
    {
        MeasurePerformance("AtanF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AtanMedium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AtanF")]
    public void BurstHighAtanF()
    {
        MeasurePerformance("AtanF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AtanHigh(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AtanF")]
    public void CoreAtanF()
    {
        MeasurePerformance("AtanF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Atan(x);
            }

            ResultF = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("AtanF")]
    public void PInvokeAtanF()
    {
        MeasurePerformance("AtanF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.AtanF(x);
            }

            ResultF = sum;
        });
    }
#endif
}
