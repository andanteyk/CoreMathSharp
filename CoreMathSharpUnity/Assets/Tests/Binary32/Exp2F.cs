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
            ResultF = math.exp(XF * 0.69314718055994530941723212145818f);
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
                sum += math.exp(x * 0.69314718055994530941723212145818f);
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
