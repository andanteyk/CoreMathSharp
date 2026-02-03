using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("CbrtF")]
    public void UnityCbrtF()
    {
        MeasurePerformance("CbrtF", () =>
        {
            ResultF = Mathf.Pow(XF, 1f / 3f);
        });
    }

    [Test, Performance]
    [Category("CbrtF")]
    public void MathematicsCbrtF()
    {
        MeasurePerformance("CbrtF", () =>
        {
            ResultF = math.pow(XF, 1f / 3f);
        });
    }

    [Test, Performance]
    [Category("CbrtF")]
    public void CoreCbrtF()
    {
        MeasurePerformance("CbrtF", () =>
        {
            ResultF = StrictMathF.Cbrt(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("CbrtF")]
    public void UnityCbrtF()
    {
        MeasurePerformance("CbrtF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Pow(x, 1f / 3f);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CbrtF")]
    public void MathematicsCbrtF()
    {
        MeasurePerformance("CbrtF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.pow(x, 1f / 3f);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CbrtF")]
    public void CoreCbrtF()
    {
        MeasurePerformance("CbrtF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Cbrt(x);
            }

            ResultF = sum;
        });
    }
}
