using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Log1PF")]
    public void UnityLog1PF()
    {
        MeasurePerformance("Log1PF", () =>
        {
            ResultF = Mathf.Log(XF + 1.0f);
        });
    }

    [Test, Performance]
    [Category("Log1PF")]
    public void MathematicsLog1PF()
    {
        MeasurePerformance("Log1PF", () =>
        {
            ResultF = math.log(XF + 1.0f);
        });
    }

    [Test, Performance]
    [Category("Log1PF")]
    public void CoreLog1PF()
    {
        MeasurePerformance("Log1PF", () =>
        {
            ResultF = StrictMathF.Log1P(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Log1PF")]
    public void UnityLog1PF()
    {
        MeasurePerformance("Log1PF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Log(x + 1.0f);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log1PF")]
    public void MathematicsLog1PF()
    {
        MeasurePerformance("Log1PF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.log(x + 1.0f);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log1PF")]
    public void CoreLog1PF()
    {
        MeasurePerformance("Log1PF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Log1P(x);
            }

            ResultF = sum;
        });
    }
}
