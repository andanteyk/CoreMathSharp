using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Exp2F")]
    public void UnityExp2F()
    {
        MeasurePerformance("Exp2F", () =>
        {
            ResultF = Mathf.Exp(XF * 0.69314718055994530941723212145818f);
        });
    }

    [Test, Performance]
    [Category("Exp2F")]
    public void MathematicsExp2F()
    {
        MeasurePerformance("Exp2F", () =>
        {
            ResultF = math.exp2(XF);
        });
    }

    [Test, Performance]
    [Category("Exp2F")]
    public void BurstLowExp2F()
    {
        MeasurePerformance("Exp2F", () =>
        {
            ResultF = BurstMathF.Exp2Low(XF);
        });
    }

    [Test, Performance]
    [Category("Exp2F")]
    public void BurstMediumExp2F()
    {
        MeasurePerformance("Exp2F", () =>
        {
            ResultF = BurstMathF.Exp2Medium(XF);
        });
    }

    [Test, Performance]
    [Category("Exp2F")]
    public void BurstHighExp2F()
    {
        MeasurePerformance("Exp2F", () =>
        {
            ResultF = BurstMathF.Exp2High(XF);
        });
    }

    [Test, Performance]
    [Category("Exp2F")]
    public void CoreExp2F()
    {
        MeasurePerformance("Exp2F", () =>
        {
            ResultF = StrictMathF.Exp2(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Exp2F")]
    public void UnityExp2F()
    {
        MeasurePerformance("Exp2F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Exp(x * 0.69314718055994530941723212145818f);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Exp2F")]
    public void MathematicsExp2F()
    {
        MeasurePerformance("Exp2F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.exp2(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Exp2F")]
    public void BurstLowExp2F()
    {
        MeasurePerformance("Exp2F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Exp2Low(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Exp2F")]
    public void BurstMediumExp2F()
    {
        MeasurePerformance("Exp2F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Exp2Medium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Exp2F")]
    public void BurstHighExp2F()
    {
        MeasurePerformance("Exp2F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Exp2High(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Exp2F")]
    public void CoreExp2F()
    {
        MeasurePerformance("Exp2F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Exp2(x);
            }

            ResultF = sum;
        });
    }
}
