using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("TanF")]
    public void UnityTanF()
    {
        MeasurePerformance("TanF", () =>
        {
            ResultF = Mathf.Tan(XF);
        });
    }

    [Test, Performance]
    [Category("TanF")]
    public void MathematicsTanF()
    {
        MeasurePerformance("TanF", () =>
        {
            ResultF = math.tan(XF);
        });
    }

    [Test, Performance]
    [Category("TanF")]
    public void CoreTanF()
    {
        MeasurePerformance("TanF", () =>
        {
            ResultF = StrictMathF.Tan(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("TanF")]
    public void UnityTanF()
    {
        MeasurePerformance("TanF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Tan(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("TanF")]
    public void MathematicsTanF()
    {
        MeasurePerformance("TanF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.tan(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("TanF")]
    public void CoreTanF()
    {
        MeasurePerformance("TanF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Tan(x);
            }

            ResultF = sum;
        });
    }
}
