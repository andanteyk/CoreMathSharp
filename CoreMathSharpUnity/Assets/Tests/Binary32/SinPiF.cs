using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("SinPiF")]
    public void UnitySinPiF()
    {
        MeasurePerformance("SinPiF", () =>
        {
            ResultF = Mathf.Sin(XF * Mathf.PI);
        });
    }

    [Test, Performance]
    [Category("SinPiF")]
    public void MathematicsSinPiF()
    {
        MeasurePerformance("SinPiF", () =>
        {
            ResultF = math.sin(XF * math.PI);
        });
    }

    [Test, Performance]
    [Category("SinPiF")]
    public void BurstLowSinPiF()
    {
        MeasurePerformance("SinPiF", () =>
        {
            ResultF = BurstMathF.SinPiLow(XF);
        });
    }

    [Test, Performance]
    [Category("SinPiF")]
    public void BurstMediumSinPiF()
    {
        MeasurePerformance("SinPiF", () =>
        {
            ResultF = BurstMathF.SinPiMedium(XF);
        });
    }

    [Test, Performance]
    [Category("SinPiF")]
    public void BurstHighSinPiF()
    {
        MeasurePerformance("SinPiF", () =>
        {
            ResultF = BurstMathF.SinPiHigh(XF);
        });
    }

    [Test, Performance]
    [Category("SinPiF")]
    public void CoreSinPiF()
    {
        MeasurePerformance("SinPiF", () =>
        {
            ResultF = StrictMathF.SinPi(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("SinPiF")]
    public void UnitySinPiF()
    {
        MeasurePerformance("SinPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Sin(x * Mathf.PI);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("SinPiF")]
    public void MathematicsSinPiF()
    {
        MeasurePerformance("SinPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.sin(x * math.PI);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("SinPiF")]
    public void BurstLowSinPiF()
    {
        MeasurePerformance("SinPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.SinPiLow(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("SinPiF")]
    public void BurstMediumSinPiF()
    {
        MeasurePerformance("SinPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.SinPiMedium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("SinPiF")]
    public void BurstHighSinPiF()
    {
        MeasurePerformance("SinPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.SinPiHigh(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("SinPiF")]
    public void CoreSinPiF()
    {
        MeasurePerformance("SinPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.SinPi(x);
            }

            ResultF = sum;
        });
    }
}
