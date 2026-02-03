using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Log10F")]
    public void UnityLog10F()
    {
        MeasurePerformance("Log10F", () =>
        {
            ResultF = Mathf.Log10(XF);
        });
    }

    [Test, Performance]
    [Category("Log10F")]
    public void MathematicsLog10F()
    {
        MeasurePerformance("Log10F", () =>
        {
            ResultF = math.log10(XF);
        });
    }

    [Test, Performance]
    [Category("Log10F")]
    public void CoreLog10F()
    {
        MeasurePerformance("Log10F", () =>
        {
            ResultF = StrictMathF.Log10(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Log10F")]
    public void UnityLog10F()
    {
        MeasurePerformance("Log10F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Log10(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log10F")]
    public void MathematicsLog10F()
    {
        MeasurePerformance("Log10F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.log10(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log10F")]
    public void CoreLog10F()
    {
        MeasurePerformance("Log10F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Log10(x);
            }

            ResultF = sum;
        });
    }
}
