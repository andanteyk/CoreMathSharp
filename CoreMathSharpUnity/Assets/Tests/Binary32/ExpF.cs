using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("ExpF")]
    public void UnityExpF()
    {
        MeasurePerformance("ExpF", () =>
        {
            ResultF = Mathf.Exp(XF);
        });
    }

    [Test, Performance]
    [Category("ExpF")]
    public void MathematicsExpF()
    {
        MeasurePerformance("ExpF", () =>
        {
            ResultF = math.exp(XF);
        });
    }

    [Test, Performance]
    [Category("ExpF")]
    public void BurstLowExpF()
    {
        MeasurePerformance("ExpF", () =>
        {
            ResultF = BurstMathF.ExpLow(XF);
        });
    }

    [Test, Performance]
    [Category("ExpF")]
    public void BurstMediumExpF()
    {
        MeasurePerformance("ExpF", () =>
        {
            ResultF = BurstMathF.ExpMedium(XF);
        });
    }

    [Test, Performance]
    [Category("ExpF")]
    public void BurstHighExpF()
    {
        MeasurePerformance("ExpF", () =>
        {
            ResultF = BurstMathF.ExpHigh(XF);
        });
    }

    [Test, Performance]
    [Category("ExpF")]
    public void CoreExpF()
    {
        MeasurePerformance("ExpF", () =>
        {
            ResultF = StrictMathF.Exp(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("ExpF")]
    public void UnityExpF()
    {
        MeasurePerformance("ExpF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Exp(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("ExpF")]
    public void MathematicsExpF()
    {
        MeasurePerformance("ExpF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.exp(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("ExpF")]
    public void BurstLowExpF()
    {
        MeasurePerformance("ExpF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.ExpLow(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("ExpF")]
    public void BurstMediumExpF()
    {
        MeasurePerformance("ExpF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.ExpMedium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("ExpF")]
    public void BurstHighExpF()
    {
        MeasurePerformance("ExpF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.ExpHigh(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("ExpF")]
    public void CoreExpF()
    {
        MeasurePerformance("ExpF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Exp(x);
            }

            ResultF = sum;
        });
    }
}
