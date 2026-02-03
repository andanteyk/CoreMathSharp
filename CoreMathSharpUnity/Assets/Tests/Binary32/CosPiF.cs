using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("CosPiF")]
    public void UnityCosPiF()
    {
        MeasurePerformance("CosPiF", () =>
        {
            ResultF = Mathf.Cos(XF * Mathf.PI);
        });
    }

    [Test, Performance]
    [Category("CosPiF")]
    public void MathematicsCosPiF()
    {
        MeasurePerformance("CosPiF", () =>
        {
            ResultF = math.cos(XF * Mathf.PI);
        });
    }

    [Test, Performance]
    [Category("CosPiF")]
    public void CoreCosPiF()
    {
        MeasurePerformance("CosPiF", () =>
        {
            ResultF = StrictMathF.CosPi(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("CosPiF")]
    public void UnityCosPiF()
    {
        MeasurePerformance("CosPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Cos(x * Mathf.PI);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CosPiF")]
    public void MathematicsCosPiF()
    {
        MeasurePerformance("CosPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.cos(x * math.PI);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CosPiF")]
    public void CoreCosPiF()
    {
        MeasurePerformance("CosPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.CosPi(x);
            }

            ResultF = sum;
        });
    }
}
