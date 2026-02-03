using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("AsinPiF")]
    public void UnityAsinPiF()
    {
        MeasurePerformance("AsinPiF", () =>
        {
            ResultF = Mathf.Asin(XF * Mathf.PI);
        });
    }

    [Test, Performance]
    [Category("AsinPiF")]
    public void MathematicsAsinPiF()
    {
        MeasurePerformance("AsinPiF", () =>
        {
            ResultF = math.asin(XF * math.PI);
        });
    }

    [Test, Performance]
    [Category("AsinPiF")]
    public void BurstLowAsinPiF()
    {
        MeasurePerformance("AsinPiF", () =>
        {
            ResultF = BurstMathF.AsinPiLow(XF);
        });
    }

    [Test, Performance]
    [Category("AsinPiF")]
    public void BurstMediumAsinPiF()
    {
        MeasurePerformance("AsinPiF", () =>
        {
            ResultF = BurstMathF.AsinPiMedium(XF);
        });
    }

    [Test, Performance]
    [Category("AsinPiF")]
    public void BurstHighAsinPiF()
    {
        MeasurePerformance("AsinPiF", () =>
        {
            ResultF = BurstMathF.AsinPiHigh(XF);
        });
    }

    [Test, Performance]
    [Category("AsinPiF")]
    public void CoreAsinPiF()
    {
        MeasurePerformance("AsinPiF", () =>
        {
            ResultF = StrictMathF.AsinPi(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("AsinPiF")]
    public void UnityAsinPiF()
    {
        MeasurePerformance("AsinPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Asin(x * Mathf.PI);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AsinPiF")]
    public void MathematicsAsinPiF()
    {
        MeasurePerformance("AsinPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.asin(x * math.PI);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AsinPiF")]
    public void BurstLowAsinPiF()
    {
        MeasurePerformance("AsinPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AsinPiLow(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AsinPiF")]
    public void BurstMediumAsinPiF()
    {
        MeasurePerformance("AsinPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AsinPiMedium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AsinPiF")]
    public void BurstHighAsinPiF()
    {
        MeasurePerformance("AsinPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AsinPiHigh(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AsinPiF")]
    public void CoreAsinPiF()
    {
        MeasurePerformance("AsinPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.AsinPi(x);
            }

            ResultF = sum;
        });
    }
}
