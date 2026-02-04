using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Log1PF")]
    public void UnityLog1PF()
    {
        MeasurePerformance("Log1PF", () =>
        {
            ResultF = Mathf.Log(XF + 1f);
        });
    }

    [Test, Performance]
    [Category("Log1PF")]
    public void MathematicsLog1PF()
    {
        MeasurePerformance("Log1PF", () =>
        {
            ResultF = math.log(XF + 1f);
        });
    }

    [Test, Performance]
    [Category("Log1PF")]
    public void BurstLowLog1PF()
    {
        MeasurePerformance("Log1PF", () =>
        {
            ResultF = BurstMathF.Log1PLow(XF);
        });
    }

    [Test, Performance]
    [Category("Log1PF")]
    public void BurstMediumLog1PF()
    {
        MeasurePerformance("Log1PF", () =>
        {
            ResultF = BurstMathF.Log1PMedium(XF);
        });
    }

    [Test, Performance]
    [Category("Log1PF")]
    public void BurstHighLog1PF()
    {
        MeasurePerformance("Log1PF", () =>
        {
            ResultF = BurstMathF.Log1PHigh(XF);
        });
    }

    [Test, Performance]
    [Category("Log1PF")]
    public void CoreLog1PF()
    {
        MeasurePerformance("Log1PF", () =>
        {
            ResultF = StrictMathF.Log1P(XF);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Log1PF")]
    public void PInvokeLog1PF()
    {
        MeasurePerformance("Log1PF", () =>
        {
            ResultF = PInvoke.PInvoke.Log1PF(XF);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Log1PF")]
    public void UnityLog1PF()
    {
        MeasurePerformance("Log1PF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Log(x + 1f);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log1PF")]
    public void MathematicsLog1PF()
    {
        MeasurePerformance("Log1PF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.log(x + 1f);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log1PF")]
    public void BurstLowLog1PF()
    {
        MeasurePerformance("Log1PF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Log1PLow(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log1PF")]
    public void BurstMediumLog1PF()
    {
        MeasurePerformance("Log1PF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Log1PMedium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log1PF")]
    public void BurstHighLog1PF()
    {
        MeasurePerformance("Log1PF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Log1PHigh(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log1PF")]
    public void CoreLog1PF()
    {
        MeasurePerformance("Log1PF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Log1P(x);
            }

            ResultF = sum;
        });
    }


#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Log1PF")]
    public void PInvokeLog1PF()
    {
        MeasurePerformance("Log1PF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.Log1PF(x);
            }

            ResultF = sum;
        });
    }
#endif
}
