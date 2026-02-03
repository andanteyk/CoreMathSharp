using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("AsinF")]
    public void UnityAsinF()
    {
        MeasurePerformance("AsinF", () =>
        {
            ResultF = Mathf.Asin(XF);
        });
    }

    [Test, Performance]
    [Category("AsinF")]
    public void MathematicsAsinF()
    {
        MeasurePerformance("AsinF", () =>
        {
            ResultF = math.asin(XF);
        });
    }

    [Test, Performance]
    [Category("AsinF")]
    public void BurstLowAsinF()
    {
        MeasurePerformance("AsinF", () =>
        {
            ResultF = BurstMathF.AsinLow(XF);
        });
    }

    [Test, Performance]
    [Category("AsinF")]
    public void BurstMediumAsinF()
    {
        MeasurePerformance("AsinF", () =>
        {
            ResultF = BurstMathF.AsinMedium(XF);
        });
    }

    [Test, Performance]
    [Category("AsinF")]
    public void BurstHighAsinF()
    {
        MeasurePerformance("AsinF", () =>
        {
            ResultF = BurstMathF.AsinHigh(XF);
        });
    }

    [Test, Performance]
    [Category("AsinF")]
    public void CoreAsinF()
    {
        MeasurePerformance("AsinF", () =>
        {
            ResultF = StrictMathF.Asin(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("AsinF")]
    public void UnityAsinF()
    {
        MeasurePerformance("AsinF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Asin(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AsinF")]
    public void MathematicsAsinF()
    {
        MeasurePerformance("AsinF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.asin(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AsinF")]
    public void BurstLowAsinF()
    {
        MeasurePerformance("AsinF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AsinLow(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AsinF")]
    public void BurstMediumAsinF()
    {
        MeasurePerformance("AsinF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AsinMedium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AsinF")]
    public void BurstHighAsinF()
    {
        MeasurePerformance("AsinF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AsinHigh(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AsinF")]
    public void CoreAsinF()
    {
        MeasurePerformance("AsinF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Asin(x);
            }

            ResultF = sum;
        });
    }
}
