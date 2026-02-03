using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("TanPiF")]
    public void UnityTanPiF()
    {
        MeasurePerformance("TanPiF", () =>
        {
            ResultF = Mathf.Tan(XF * Mathf.PI);
        });
    }

    [Test, Performance]
    [Category("TanPiF")]
    public void MathematicsTanPiF()
    {
        MeasurePerformance("TanPiF", () =>
        {
            ResultF = math.tan(XF * math.PI);
        });
    }

    [Test, Performance]
    [Category("TanPiF")]
    public void CoreTanPiF()
    {
        MeasurePerformance("TanPiF", () =>
        {
            ResultF = StrictMathF.TanPi(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("TanPiF")]
    public void UnityTanPiF()
    {
        MeasurePerformance("TanPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Tan(x * Mathf.PI);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("TanPiF")]
    public void MathematicsTanPiF()
    {
        MeasurePerformance("TanPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.tan(x * math.PI);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("TanPiF")]
    public void CoreTanPiF()
    {
        MeasurePerformance("TanPiF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.TanPi(x);
            }

            ResultF = sum;
        });
    }
}
