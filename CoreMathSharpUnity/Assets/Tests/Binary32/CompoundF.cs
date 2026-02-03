using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("CompoundF")]
    public void UnityCompoundF()
    {
        MeasurePerformance("CompoundF", () =>
        {
            ResultF = Mathf.Pow(XF + 1.0f, YF);
        });
    }

    [Test, Performance]
    [Category("CompoundF")]
    public void MathematicsCompoundF()
    {
        MeasurePerformance("CompoundF", () =>
        {
            ResultF = math.pow(XF + 1.0f, YF);
        });
    }

    [Test, Performance]
    [Category("CompoundF")]
    public void CoreCompoundF()
    {
        MeasurePerformance("CompoundF", () =>
        {
            ResultF = StrictMathF.Compound(XF, YF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("CompoundF")]
    public void UnityCompoundF()
    {
        MeasurePerformance("CompoundF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += Mathf.Pow(XF[i] + 1.0f, YF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CompoundF")]
    public void MathematicsCompoundF()
    {
        MeasurePerformance("CompoundF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += math.pow(XF[i] + 1.0f, YF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CompoundF")]
    public void CoreCompoundF()
    {
        MeasurePerformance("CompoundF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += StrictMathF.Compound(XF[i], YF[i]);
            }

            ResultF = sum;
        });
    }
}
