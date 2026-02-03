using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("ExpM1F")]
    public void UnityExpM1F()
    {
        MeasurePerformance("ExpM1F", () =>
        {
            ResultF = Mathf.Exp(XF) - 1.0f;
        });
    }

    [Test, Performance]
    [Category("ExpM1F")]
    public void MathematicsExpM1F()
    {
        MeasurePerformance("ExpM1F", () =>
        {
            ResultF = math.exp(XF) - 1.0f;
        });
    }

    [Test, Performance]
    [Category("ExpM1F")]
    public void CoreExpM1F()
    {
        MeasurePerformance("ExpM1F", () =>
        {
            ResultF = StrictMathF.ExpM1(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("ExpM1F")]
    public void UnityExpM1F()
    {
        MeasurePerformance("ExpM1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Exp(x) - 1.0f;
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("ExpM1F")]
    public void MathematicsExpM1F()
    {
        MeasurePerformance("ExpM1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.exp(x) - 1.0f;
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("ExpM1F")]
    public void CoreExpM1F()
    {
        MeasurePerformance("ExpM1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.ExpM1(x);
            }

            ResultF = sum;
        });
    }
}
