using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Log10P1F")]
    public void UnityLog10P1F()
    {
        MeasurePerformance("Log10P1F", () =>
        {
            ResultF = Mathf.Log10(XF + 1f);
        });
    }

    [Test, Performance]
    [Category("Log10P1F")]
    public void MathematicsLog10P1F()
    {
        MeasurePerformance("Log10P1F", () =>
        {
            ResultF = math.log10(XF + 1f);
        });
    }

    [Test, Performance]
    [Category("Log10P1F")]
    public void BurstLowLog10P1F()
    {
        MeasurePerformance("Log10P1F", () =>
        {
            ResultF = BurstMathF.Log10P1Low(XF);
        });
    }

    [Test, Performance]
    [Category("Log10P1F")]
    public void BurstMediumLog10P1F()
    {
        MeasurePerformance("Log10P1F", () =>
        {
            ResultF = BurstMathF.Log10P1Medium(XF);
        });
    }

    [Test, Performance]
    [Category("Log10P1F")]
    public void BurstHighLog10P1F()
    {
        MeasurePerformance("Log10P1F", () =>
        {
            ResultF = BurstMathF.Log10P1High(XF);
        });
    }

    [Test, Performance]
    [Category("Log10P1F")]
    public void CoreLog10P1F()
    {
        MeasurePerformance("Log10P1F", () =>
        {
            ResultF = StrictMathF.Log10P1(XF);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Log10P1F")]
    public void PInvokeLog10P1F()
    {
        MeasurePerformance("Log10P1F", () =>
        {
            ResultF = PInvoke.PInvoke.Log10P1F(XF);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Log10P1F")]
    public void UnityLog10P1F()
    {
        MeasurePerformance("Log10P1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Log10(x + 1f);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log10P1F")]
    public void MathematicsLog10P1F()
    {
        MeasurePerformance("Log10P1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.log10(x + 1f);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log10P1F")]
    public void BurstLowLog10P1F()
    {
        MeasurePerformance("Log10P1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Log10P1Low(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log10P1F")]
    public void BurstMediumLog10P1F()
    {
        MeasurePerformance("Log10P1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Log10P1Medium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log10P1F")]
    public void BurstHighLog10P1F()
    {
        MeasurePerformance("Log10P1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Log10P1High(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log10P1F")]
    public void CoreLog10P1F()
    {
        MeasurePerformance("Log10P1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Log10P1(x);
            }

            ResultF = sum;
        });
    }


#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Log10P1F")]
    public void PInvokeLog10P1F()
    {
        MeasurePerformance("Log10P1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.Log10P1F(x);
            }

            ResultF = sum;
        });
    }
#endif
}
