using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("AtanPiF")]
    public void UnityAtanPiF()
    {
        MeasurePerformance("AtanPiF", () =>
        {
            ResultF = Mathf.Atan(XF * Mathf.PI);
        });
    }

    [Test, Performance]
    [Category("AtanPiF")]
    public void MathematicsAtanPiF()
    {
        MeasurePerformance("AtanPiF", () =>
        {
            ResultF = math.atan(XF * math.PI);
        });
    }

    [Test, Performance]
    [Category("AtanPiF")]
    public void BurstLowAtanPiF()
    {
        MeasurePerformance("AtanPiF", () =>
        {
            ResultF = BurstMathF.AtanPiLow(XF);
        });
    }

    [Test, Performance]
    [Category("AtanPiF")]
    public void BurstMediumAtanPiF()
    {
        MeasurePerformance("AtanPiF", () =>
        {
            ResultF = BurstMathF.AtanPiMedium(XF);
        });
    }

    [Test, Performance]
    [Category("AtanPiF")]
    public void BurstHighAtanPiF()
    {
        MeasurePerformance("AtanPiF", () =>
        {
            ResultF = BurstMathF.AtanPiHigh(XF);
        });
    }

    [Test, Performance]
    [Category("AtanPiF")]
    public void CoreAtanPiF()
    {
        MeasurePerformance("AtanPiF", () =>
        {
            ResultF = StrictMathF.AtanPi(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("AtanPiF")]
    public void UnityAtanPiF()
    {
        MeasurePerformance("AtanPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Atan(x * Mathf.PI);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AtanPiF")]
    public void MathematicsAtanPiF()
    {
        MeasurePerformance("AtanPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.atan(x * math.PI);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AtanPiF")]
    public void BurstLowAtanPiF()
    {
        MeasurePerformance("AtanPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AtanPiLow(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AtanPiF")]
    public void BurstMediumAtanPiF()
    {
        MeasurePerformance("AtanPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AtanPiMedium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AtanPiF")]
    public void BurstHighAtanPiF()
    {
        MeasurePerformance("AtanPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AtanPiHigh(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AtanPiF")]
    public void CoreAtanPiF()
    {
        MeasurePerformance("AtanPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.AtanPi(x);
            }

            ResultF = sum;
        });
    }
}
