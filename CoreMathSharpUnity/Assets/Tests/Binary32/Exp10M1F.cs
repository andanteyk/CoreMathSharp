using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Exp10M1F")]
    public void UnityExp10M1F()
    {
        MeasurePerformance("Exp10M1F", () =>
        {
            ResultF = Mathf.Exp(XF * 2.3025850929940456840179914546844f) - 1.0f;
        });
    }

    [Test, Performance]
    [Category("Exp10M1F")]
    public void MathematicsExp10M1F()
    {
        MeasurePerformance("Exp10M1F", () =>
        {
            ResultF = math.exp10(XF) - 1.0f;
        });
    }

    [Test, Performance]
    [Category("Exp10M1F")]
    public void BurstLowExp10M1F()
    {
        MeasurePerformance("Exp10M1F", () =>
        {
            ResultF = BurstMathF.Exp10M1Low(XF);
        });
    }

    [Test, Performance]
    [Category("Exp10M1F")]
    public void BurstMediumExp10M1F()
    {
        MeasurePerformance("Exp10M1F", () =>
        {
            ResultF = BurstMathF.Exp10M1Medium(XF);
        });
    }

    [Test, Performance]
    [Category("Exp10M1F")]
    public void BurstHighExp10M1F()
    {
        MeasurePerformance("Exp10M1F", () =>
        {
            ResultF = BurstMathF.Exp10M1High(XF);
        });
    }

    [Test, Performance]
    [Category("Exp10M1F")]
    public void CoreExp10M1F()
    {
        MeasurePerformance("Exp10M1F", () =>
        {
            ResultF = StrictMathF.Exp10M1(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Exp10M1F")]
    public void UnityExp10M1F()
    {
        MeasurePerformance("Exp10M1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Exp(x * 2.3025850929940456840179914546844f) - 1.0f;
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Exp10M1F")]
    public void MathematicsExp10M1F()
    {
        MeasurePerformance("Exp10M1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.exp10(x) - 1.0f;
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Exp10M1F")]
    public void BurstLowExp10M1F()
    {
        MeasurePerformance("Exp10M1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Exp10M1Low(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Exp10M1F")]
    public void BurstMediumExp10M1F()
    {
        MeasurePerformance("Exp10M1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Exp10M1Medium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Exp10M1F")]
    public void BurstHighExp10M1F()
    {
        MeasurePerformance("Exp10M1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Exp10M1High(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Exp10M1F")]
    public void CoreExp10M1F()
    {
        MeasurePerformance("Exp10M1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Exp10M1(x);
            }

            ResultF = sum;
        });
    }
}
