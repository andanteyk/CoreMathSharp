using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("SinCosF")]
    public void UnitySinCosF()
    {
        MeasurePerformance("SinCosF", () =>
        {
            ResultF = Mathf.Sin(XF) + Mathf.Cos(XF);
        });
    }

    [Test, Performance]
    [Category("SinCosF")]
    public void MathematicsSinCosF()
    {
        MeasurePerformance("SinCosF", () =>
        {
            math.sincos(XF, out float sin, out float cos);
            ResultF = sin + cos;
        });
    }

    [Test, Performance]
    [Category("SinCosF")]
    public void CoreSinCosF()
    {
        MeasurePerformance("SinCosF", () =>
        {
            var (sin, cos) = StrictMathF.SinCos(XF);
            ResultF = sin + cos;
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("SinCosF")]
    public void UnitySinCosF()
    {
        MeasurePerformance("SinCosF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Sin(x) + Mathf.Cos(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("SinCosF")]
    public void MathematicsSinCosF()
    {
        MeasurePerformance("SinCosF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                math.sincos(x, out float sin, out float cos);
                sum += sin + cos;
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("SinCosF")]
    public void CoreSinCosF()
    {
        MeasurePerformance("SinCosF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                var (sin, cos) = StrictMathF.SinCos(x);
                sum += sin + cos;
            }

            ResultF = sum;
        });
    }
}
