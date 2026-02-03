using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("ReciprocalSqrtF")]
    public void UnityReciprocalSqrtF()
    {
        MeasurePerformance("ReciprocalSqrtF", () =>
        {
            ResultF = 1.0f / Mathf.Sqrt(XF);
        });
    }

    [Test, Performance]
    [Category("ReciprocalSqrtF")]
    public void MathematicsReciprocalSqrtF()
    {
        MeasurePerformance("ReciprocalSqrtF", () =>
        {
            ResultF = math.rsqrt(XF);
        });
    }

    [Test, Performance]
    [Category("ReciprocalSqrtF")]
    public void BurstLowReciprocalSqrtF()
    {
        MeasurePerformance("ReciprocalSqrtF", () =>
        {
            ResultF = BurstMathF.ReciprocalSqrtLow(XF);
        });
    }

    [Test, Performance]
    [Category("ReciprocalSqrtF")]
    public void BurstMediumReciprocalSqrtF()
    {
        MeasurePerformance("ReciprocalSqrtF", () =>
        {
            ResultF = BurstMathF.ReciprocalSqrtMedium(XF);
        });
    }

    [Test, Performance]
    [Category("ReciprocalSqrtF")]
    public void BurstHighReciprocalSqrtF()
    {
        MeasurePerformance("ReciprocalSqrtF", () =>
        {
            ResultF = BurstMathF.ReciprocalSqrtHigh(XF);
        });
    }

    [Test, Performance]
    [Category("ReciprocalSqrtF")]
    public void CoreReciprocalSqrtF()
    {
        MeasurePerformance("ReciprocalSqrtF", () =>
        {
            ResultF = StrictMathF.ReciprocalSqrt(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("ReciprocalSqrtF")]
    public void UnityReciprocalSqrtF()
    {
        MeasurePerformance("ReciprocalSqrtF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += 1.0f / Mathf.Sqrt(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("ReciprocalSqrtF")]
    public void MathematicsReciprocalSqrtF()
    {
        MeasurePerformance("ReciprocalSqrtF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.rsqrt(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("ReciprocalSqrtF")]
    public void BurstLowReciprocalSqrtF()
    {
        MeasurePerformance("ReciprocalSqrtF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.ReciprocalSqrtLow(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("ReciprocalSqrtF")]
    public void BurstMediumReciprocalSqrtF()
    {
        MeasurePerformance("ReciprocalSqrtF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.ReciprocalSqrtMedium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("ReciprocalSqrtF")]
    public void BurstHighReciprocalSqrtF()
    {
        MeasurePerformance("ReciprocalSqrtF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.ReciprocalSqrtHigh(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("ReciprocalSqrtF")]
    public void CoreReciprocalSqrtF()
    {
        MeasurePerformance("ReciprocalSqrtF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.ReciprocalSqrt(x);
            }

            ResultF = sum;
        });
    }
}
