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
            ResultF = math.exp10(XF);
        });
    }

    [Test, Performance]
    [Category("Exp10F")]
    public void BurstLowExp10F()
    {
        MeasurePerformance("Exp10F", () =>
        {
            ResultF = BurstMathF.Exp10Low(XF);
        });
    }

    [Test, Performance]
    [Category("Exp10F")]
    public void BurstMediumExp10F()
    {
        MeasurePerformance("Exp10F", () =>
        {
            ResultF = BurstMathF.Exp10Medium(XF);
        });
    }

    [Test, Performance]
    [Category("Exp10F")]
    public void BurstHighExp10F()
    {
        MeasurePerformance("Exp10F", () =>
        {
            ResultF = BurstMathF.Exp10High(XF);
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

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Exp10F")]
    public void PInvokeExp10F()
    {
        MeasurePerformance("Exp10F", () =>
        {
            ResultF = PInvoke.PInvoke.Exp10F(XF);
        });
    }
#endif
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
                sum += math.exp10(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Exp10F")]
    public void BurstLowExp10F()
    {
        MeasurePerformance("Exp10F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Exp10Low(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Exp10F")]
    public void BurstMediumExp10F()
    {
        MeasurePerformance("Exp10F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Exp10Medium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Exp10F")]
    public void BurstHighExp10F()
    {
        MeasurePerformance("Exp10F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Exp10High(x);
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

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Exp10F")]
    public void PInvokeExp10F()
    {
        MeasurePerformance("Exp10F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.Exp10F(x);
            }

            ResultF = sum;
        });
    }
#endif
}
