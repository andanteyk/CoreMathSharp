using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("ReciprocalSqrtF")]
    public void UnityReciprocalSqrtF()
    {
        MeasurePerformance("ReciprocalSqrtF", () =>
        {
            ResultF = 1.0f / Mathf.Sqrt(XF);
        });
    }

    [Test, Performance]
    [Category("ReciprocalSqrtF")]
    public void MathematicsReciprocalSqrtF()
    {
        MeasurePerformance("ReciprocalSqrtF", () =>
        {
            ResultF = 1.0f / math.sqrt(XF);
        });
    }

    [Test, Performance]
    [Category("ReciprocalSqrtF")]
    public void CoreReciprocalSqrtF()
    {
        MeasurePerformance("ReciprocalSqrtF", () =>
        {
            ResultF = StrictMathF.ReciprocalSqrt(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("ReciprocalSqrtF")]
    public void UnityReciprocalSqrtF()
    {
        MeasurePerformance("ReciprocalSqrtF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += 1.0f / Mathf.Sqrt(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("ReciprocalSqrtF")]
    public void MathematicsReciprocalSqrtF()
    {
        MeasurePerformance("ReciprocalSqrtF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += 1.0f / math.sqrt(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("ReciprocalSqrtF")]
    public void CoreReciprocalSqrtF()
    {
        MeasurePerformance("ReciprocalSqrtF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.ReciprocalSqrt(x);
            }

            ResultF = sum;
        });
    }
}
