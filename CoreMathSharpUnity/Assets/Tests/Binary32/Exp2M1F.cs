using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Exp2M1F")]
    public void UnityExp2M1F()
    {
        MeasurePerformance("Exp2M1F", () =>
        {
            ResultF = Mathf.Exp(XF * 0.69314718055994530941723212145818f) - 1.0f;
        });
    }

    [Test, Performance]
    [Category("Exp2M1F")]
    public void MathematicsExp2M1F()
    {
        MeasurePerformance("Exp2M1F", () =>
        {
            ResultF = math.exp2(XF) - 1.0f;
        });
    }

    [Test, Performance]
    [Category("Exp2M1F")]
    public void BurstLowExp2M1F()
    {
        MeasurePerformance("Exp2M1F", () =>
        {
            ResultF = BurstMathF.Exp2M1Low(XF);
        });
    }

    [Test, Performance]
    [Category("Exp2M1F")]
    public void BurstMediumExp2M1F()
    {
        MeasurePerformance("Exp2M1F", () =>
        {
            ResultF = BurstMathF.Exp2M1Medium(XF);
        });
    }

    [Test, Performance]
    [Category("Exp2M1F")]
    public void BurstHighExp2M1F()
    {
        MeasurePerformance("Exp2M1F", () =>
        {
            ResultF = BurstMathF.Exp2M1High(XF);
        });
    }

    [Test, Performance]
    [Category("Exp2M1F")]
    public void CoreExp2M1F()
    {
        MeasurePerformance("Exp2M1F", () =>
        {
            ResultF = StrictMathF.Exp2M1(XF);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Exp2M1F")]
    public void PInvokeExp2M1F()
    {
        MeasurePerformance("Exp2M1F", () =>
        {
            ResultF = PInvoke.PInvoke.Exp2M1F(XF);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Exp2M1F")]
    public void UnityExp2M1F()
    {
        MeasurePerformance("Exp2M1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Exp(x * 0.69314718055994530941723212145818f) - 1.0f;
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Exp2M1F")]
    public void MathematicsExp2M1F()
    {
        MeasurePerformance("Exp2M1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.exp2(x) - 1.0f;
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Exp2M1F")]
    public void BurstLowExp2M1F()
    {
        MeasurePerformance("Exp2M1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Exp2M1Low(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Exp2M1F")]
    public void BurstMediumExp2M1F()
    {
        MeasurePerformance("Exp2M1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Exp2M1Medium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Exp2M1F")]
    public void BurstHighExp2M1F()
    {
        MeasurePerformance("Exp2M1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Exp2M1High(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Exp2M1F")]
    public void CoreExp2M1F()
    {
        MeasurePerformance("Exp2M1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Exp2M1(x);
            }

            ResultF = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Exp2M1F")]
    public void PInvokeExp2M1F()
    {
        MeasurePerformance("Exp2M1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.Exp2M1F(x);
            }

            ResultF = sum;
        });
    }
#endif
}
