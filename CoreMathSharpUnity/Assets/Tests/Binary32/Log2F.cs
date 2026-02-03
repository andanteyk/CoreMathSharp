using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Log2F")]
    public void UnityLog2F()
    {
        MeasurePerformance("Log2F", () =>
        {
            ResultF = Mathf.Log(XF) * 1.4426950408889634073599246810019f;
        });
    }

    [Test, Performance]
    [Category("Log2F")]
    public void MathematicsLog2F()
    {
        MeasurePerformance("Log2F", () =>
        {
            ResultF = math.log2(XF);
        });
    }

    [Test, Performance]
    [Category("Log2F")]
    public void BurstLowLog2F()
    {
        MeasurePerformance("Log2F", () =>
        {
            ResultF = BurstMathF.Log2Low(XF);
        });
    }

    [Test, Performance]
    [Category("Log2F")]
    public void BurstMediumLog2F()
    {
        MeasurePerformance("Log2F", () =>
        {
            ResultF = BurstMathF.Log2Medium(XF);
        });
    }

    [Test, Performance]
    [Category("Log2F")]
    public void BurstHighLog2F()
    {
        MeasurePerformance("Log2F", () =>
        {
            ResultF = BurstMathF.Log2High(XF);
        });
    }

    [Test, Performance]
    [Category("Log2F")]
    public void CoreLog2F()
    {
        MeasurePerformance("Log2F", () =>
        {
            ResultF = StrictMathF.Log2(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Log2F")]
    public void UnityLog2F()
    {
        MeasurePerformance("Log2F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Log(x) * 1.4426950408889634073599246810019f;
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log2F")]
    public void MathematicsLog2F()
    {
        MeasurePerformance("Log2F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.log2(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log2F")]
    public void BurstLowLog2F()
    {
        MeasurePerformance("Log2F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Log2Low(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log2F")]
    public void BurstMediumLog2F()
    {
        MeasurePerformance("Log2F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Log2Medium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log2F")]
    public void BurstHighLog2F()
    {
        MeasurePerformance("Log2F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Log2High(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log2F")]
    public void CoreLog2F()
    {
        MeasurePerformance("Log2F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Log2(x);
            }

            ResultF = sum;
        });
    }
}
