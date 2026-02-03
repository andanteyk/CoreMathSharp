using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("HypotF")]
    public void UnityHypotF()
    {
        MeasurePerformance("HypotF", () =>
        {
            ResultF = Mathf.Sqrt(XF * XF + YF * YF);
        });
    }

    [Test, Performance]
    [Category("HypotF")]
    public void MathematicsHypotF()
    {
        MeasurePerformance("HypotF", () =>
        {
            ResultF = math.length(new float2(XF, YF));
        });
    }

    [Test, Performance]
    [Category("HypotF")]
    public void BurstLowHypotF()
    {
        MeasurePerformance("HypotF", () =>
        {
            ResultF = BurstMathF.HypotLow(XF, YF);
        });
    }

    [Test, Performance]
    [Category("HypotF")]
    public void BurstMediumHypotF()
    {
        MeasurePerformance("HypotF", () =>
        {
            ResultF = BurstMathF.HypotMedium(XF, YF);
        });
    }

    [Test, Performance]
    [Category("HypotF")]
    public void BurstHighHypotF()
    {
        MeasurePerformance("HypotF", () =>
        {
            ResultF = BurstMathF.HypotHigh(XF, YF);
        });
    }

    [Test, Performance]
    [Category("HypotF")]
    public void CoreHypotF()
    {
        MeasurePerformance("HypotF", () =>
        {
            ResultF = StrictMathF.Hypot(XF, YF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("HypotF")]
    public void UnityHypotF()
    {
        MeasurePerformance("HypotF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += Mathf.Sqrt(XF[i] * XF[i] + YF[i] * YF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("HypotF")]
    public void MathematicsHypotF()
    {
        MeasurePerformance("HypotF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += math.length(new float2(XF[i], YF[i]));
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("HypotF")]
    public void BurstLowHypotF()
    {
        MeasurePerformance("HypotF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += BurstMathF.HypotLow(XF[i], YF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("HypotF")]
    public void BurstMediumHypotF()
    {
        MeasurePerformance("HypotF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += BurstMathF.HypotMedium(XF[i], YF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("HypotF")]
    public void BurstHighHypotF()
    {
        MeasurePerformance("HypotF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += BurstMathF.HypotHigh(XF[i], YF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("HypotF")]
    public void CoreHypotF()
    {
        MeasurePerformance("HypotF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += StrictMathF.Hypot(XF[i], YF[i]);
            }

            ResultF = sum;
        });
    }
}
