using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Log10F")]
    public void UnityLog10F()
    {
        MeasurePerformance("Log10F", () =>
        {
            ResultF = Mathf.Log10(XF);
        });
    }

    [Test, Performance]
    [Category("Log10F")]
    public void MathematicsLog10F()
    {
        MeasurePerformance("Log10F", () =>
        {
            ResultF = math.log10(XF);
        });
    }

    [Test, Performance]
    [Category("Log10F")]
    public void BurstLowLog10F()
    {
        MeasurePerformance("Log10F", () =>
        {
            ResultF = BurstMathF.Log10Low(XF);
        });
    }

    [Test, Performance]
    [Category("Log10F")]
    public void BurstMediumLog10F()
    {
        MeasurePerformance("Log10F", () =>
        {
            ResultF = BurstMathF.Log10Medium(XF);
        });
    }

    [Test, Performance]
    [Category("Log10F")]
    public void BurstHighLog10F()
    {
        MeasurePerformance("Log10F", () =>
        {
            ResultF = BurstMathF.Log10High(XF);
        });
    }

    [Test, Performance]
    [Category("Log10F")]
    public void CoreLog10F()
    {
        MeasurePerformance("Log10F", () =>
        {
            ResultF = StrictMathF.Log10(XF);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Log10F")]
    public void PInvokeLog10F()
    {
        MeasurePerformance("Log10F", () =>
        {
            ResultF = PInvoke.PInvoke.Log10F(XF);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Log10F")]
    public void UnityLog10F()
    {
        MeasurePerformance("Log10F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Log10(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log10F")]
    public void MathematicsLog10F()
    {
        MeasurePerformance("Log10F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.log10(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log10F")]
    public void BurstLowLog10F()
    {
        MeasurePerformance("Log10F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Log10Low(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log10F")]
    public void BurstMediumLog10F()
    {
        MeasurePerformance("Log10F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Log10Medium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log10F")]
    public void BurstHighLog10F()
    {
        MeasurePerformance("Log10F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Log10High(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log10F")]
    public void CoreLog10F()
    {
        MeasurePerformance("Log10F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Log10(x);
            }

            ResultF = sum;
        });
    }


#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Log10F")]
    public void PInvokeLog10F()
    {
        MeasurePerformance("Log10F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.Log10F(x);
            }

            ResultF = sum;
        });
    }
#endif
}
