using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("AsinPiF")]
    public void UnityAsinPiF()
    {
        MeasurePerformance("AsinPiF", () =>
        {
            ResultF = Mathf.Asin(XF) / Mathf.PI;
        });
    }

    [Test, Performance]
    [Category("AsinPiF")]
    public void MathematicsAsinPiF()
    {
        MeasurePerformance("AsinPiF", () =>
        {
            ResultF = math.asin(XF) / math.PI;
        });
    }

    [Test, Performance]
    [Category("AsinPiF")]
    public void CoreAsinPiF()
    {
        MeasurePerformance("AsinPiF", () =>
        {
            ResultF = StrictMathF.AsinPi(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("AsinPiF")]
    public void UnityAsinPiF()
    {
        MeasurePerformance("AsinPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Asin(x) / Mathf.PI;
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AsinPiF")]
    public void MathematicsAsinPiF()
    {
        MeasurePerformance("AsinPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.asin(x) / math.PI;
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AsinPiF")]
    public void CoreAsinPiF()
    {
        MeasurePerformance("AsinPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.AsinPi(x);
            }

            ResultF = sum;
        });
    }
}
