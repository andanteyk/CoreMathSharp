using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("AtanF")]
    public void UnityAtanF()
    {
        MeasurePerformance("AtanF", () =>
        {
            ResultF = Mathf.Atan(XF);
        });
    }

    [Test, Performance]
    [Category("AtanF")]
    public void MathematicsAtanF()
    {
        MeasurePerformance("AtanF", () =>
        {
            ResultF = math.atan(XF);
        });
    }

    [Test, Performance]
    [Category("AtanF")]
    public void CoreAtanF()
    {
        MeasurePerformance("AtanF", () =>
        {
            ResultF = StrictMathF.Atan(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("AtanF")]
    public void UnityAtanF()
    {
        MeasurePerformance("AtanF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Atan(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AtanF")]
    public void MathematicsAtanF()
    {
        MeasurePerformance("AtanF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.atan(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AtanF")]
    public void CoreAtanF()
    {
        MeasurePerformance("AtanF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Atan(x);
            }

            ResultF = sum;
        });
    }
}
