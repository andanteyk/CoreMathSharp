using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("SinF")]
    public void UnitySinF()
    {
        MeasurePerformance("SinF", () =>
        {
            ResultF = Mathf.Sin(XF);
        });
    }

    [Test, Performance]
    [Category("SinF")]
    public void MathematicsSinF()
    {
        MeasurePerformance("SinF", () =>
        {
            ResultF = math.sin(XF);
        });
    }

    [Test, Performance]
    [Category("SinF")]
    public void CoreSinF()
    {
        MeasurePerformance("SinF", () =>
        {
            ResultF = StrictMathF.Sin(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("SinF")]
    public void UnitySinF()
    {
        MeasurePerformance("SinF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Sin(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("SinF")]
    public void MathematicsSinF()
    {
        MeasurePerformance("SinF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.sin(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("SinF")]
    public void CoreSinF()
    {
        MeasurePerformance("SinF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Sin(x);
            }

            ResultF = sum;
        });
    }
}
