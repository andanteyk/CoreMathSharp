using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Exp10F")]
    public void UnityExp10F()
    {
        MeasurePerformance("Exp10F", () =>
        {
            ResultF = Mathf.Exp(XF * 2.3025850929940456840179914546844f);
        });
    }

    [Test, Performance]
    [Category("Exp10F")]
    public void MathematicsExp10F()
    {
        MeasurePerformance("Exp10F", () =>
        {
            ResultF = math.exp(XF * 2.3025850929940456840179914546844f);
        });
    }

    [Test, Performance]
    [Category("Exp10F")]
    public void CoreExp10F()
    {
        MeasurePerformance("Exp10F", () =>
        {
            ResultF = StrictMathF.Exp10(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Exp10F")]
    public void UnityExp10F()
    {
        MeasurePerformance("Exp10F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Exp(x * 2.3025850929940456840179914546844f);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Exp10F")]
    public void MathematicsExp10F()
    {
        MeasurePerformance("Exp10F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.exp(x * 2.3025850929940456840179914546844f);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Exp10F")]
    public void CoreExp10F()
    {
        MeasurePerformance("Exp10F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Exp10(x);
            }

            ResultF = sum;
        });
    }
}
