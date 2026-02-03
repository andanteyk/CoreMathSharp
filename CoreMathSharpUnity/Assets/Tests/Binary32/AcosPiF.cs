using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("AcosPiF")]
    public void UnityAcosPiF()
    {
        MeasurePerformance("AcosPiF", () =>
        {
            ResultF = Mathf.Acos(XF * Mathf.PI);
        });
    }

    [Test, Performance]
    [Category("AcosPiF")]
    public void MathematicsAcosPiF()
    {
        MeasurePerformance("AcosPiF", () =>
        {
            ResultF = math.acos(XF * math.PI);
        });
    }

    [Test, Performance]
    [Category("AcosPiF")]
    public void BurstLowAcosPiF()
    {
        MeasurePerformance("AcosPiF", () =>
        {
            ResultF = BurstMathF.AcosPiLow(XF);
        });
    }

    [Test, Performance]
    [Category("AcosPiF")]
    public void BurstMediumAcosPiF()
    {
        MeasurePerformance("AcosPiF", () =>
        {
            ResultF = BurstMathF.AcosPiMedium(XF);
        });
    }

    [Test, Performance]
    [Category("AcosPiF")]
    public void BurstHighAcosPiF()
    {
        MeasurePerformance("AcosPiF", () =>
        {
            ResultF = BurstMathF.AcosPiHigh(XF);
        });
    }

    [Test, Performance]
    [Category("AcosPiF")]
    public void CoreAcosPiF()
    {
        MeasurePerformance("AcosPiF", () =>
        {
            ResultF = StrictMathF.AcosPi(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("AcosPiF")]
    public void UnityAcosPiF()
    {
        MeasurePerformance("AcosPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Acos(x * Mathf.PI);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AcosPiF")]
    public void MathematicsAcosPiF()
    {
        MeasurePerformance("AcosPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.acos(x * math.PI);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AcosPiF")]
    public void BurstLowAcosPiF()
    {
        MeasurePerformance("AcosPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AcosPiLow(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AcosPiF")]
    public void BurstMediumAcosPiF()
    {
        MeasurePerformance("AcosPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AcosPiMedium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AcosPiF")]
    public void BurstHighAcosPiF()
    {
        MeasurePerformance("AcosPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AcosPiHigh(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AcosPiF")]
    public void CoreAcosPiF()
    {
        MeasurePerformance("AcosPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.AcosPi(x);
            }

            ResultF = sum;
        });
    }
}
