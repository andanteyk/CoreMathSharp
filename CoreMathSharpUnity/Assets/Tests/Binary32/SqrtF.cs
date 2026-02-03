using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("SqrtF")]
    public void UnitySqrtF()
    {
        MeasurePerformance("SqrtF", () =>
        {
            ResultF = Mathf.Sqrt(XF);
        });
    }

    [Test, Performance]
    [Category("SqrtF")]
    public void MathematicsSqrtF()
    {
        MeasurePerformance("SqrtF", () =>
        {
            ResultF = math.sqrt(XF);
        });
    }

    [Test, Performance]
    [Category("SqrtF")]
    public void BurstLowSqrtF()
    {
        MeasurePerformance("SqrtF", () =>
        {
            ResultF = BurstMathF.SqrtLow(XF);
        });
    }

    [Test, Performance]
    [Category("SqrtF")]
    public void BurstMediumSqrtF()
    {
        MeasurePerformance("SqrtF", () =>
        {
            ResultF = BurstMathF.SqrtMedium(XF);
        });
    }

    [Test, Performance]
    [Category("SqrtF")]
    public void BurstHighSqrtF()
    {
        MeasurePerformance("SqrtF", () =>
        {
            ResultF = BurstMathF.SqrtHigh(XF);
        });
    }

    [Test, Performance]
    [Category("SqrtF")]
    public void CoreSqrtF()
    {
        MeasurePerformance("SqrtF", () =>
        {
            ResultF = StrictMathF.Sqrt(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("SqrtF")]
    public void UnitySqrtF()
    {
        MeasurePerformance("SqrtF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Sqrt(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("SqrtF")]
    public void MathematicsSqrtF()
    {
        MeasurePerformance("SqrtF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.sqrt(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("SqrtF")]
    public void BurstLowSqrtF()
    {
        MeasurePerformance("SqrtF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.SqrtLow(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("SqrtF")]
    public void BurstMediumSqrtF()
    {
        MeasurePerformance("SqrtF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.SqrtMedium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("SqrtF")]
    public void BurstHighSqrtF()
    {
        MeasurePerformance("SqrtF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.SqrtHigh(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("SqrtF")]
    public void CoreSqrtF()
    {
        MeasurePerformance("SqrtF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Sqrt(x);
            }

            ResultF = sum;
        });
    }
}
