using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("AtanPiF")]
    public void UnityAtanPiF()
    {
        MeasurePerformance("AtanPiF", () =>
        {
            ResultF = Mathf.Atan(XF) / Mathf.PI;
        });
    }

    [Test, Performance]
    [Category("AtanPiF")]
    public void MathematicsAtanPiF()
    {
        MeasurePerformance("AtanPiF", () =>
        {
            ResultF = math.atan(XF) / math.PI;
        });
    }

    [Test, Performance]
    [Category("AtanPiF")]
    public void CoreAtanPiF()
    {
        MeasurePerformance("AtanPiF", () =>
        {
            ResultF = StrictMathF.AtanPi(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("AtanPiF")]
    public void UnityAtanPiF()
    {
        MeasurePerformance("AtanPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Atan(x) / Mathf.PI;
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AtanPiF")]
    public void MathematicsAtanPiF()
    {
        MeasurePerformance("AtanPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.atan(x) / math.PI;
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AtanPiF")]
    public void CoreAtanPiF()
    {
        MeasurePerformance("AtanPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.AtanPi(x);
            }

            ResultF = sum;
        });
    }
}
