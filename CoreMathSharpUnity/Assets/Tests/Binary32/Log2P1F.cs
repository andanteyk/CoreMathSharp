using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Log2P1F")]
    public void UnityLog2P1F()
    {
        MeasurePerformance("Log2P1F", () =>
        {
            ResultF = Mathf.Log(XF + 1.0f) * 1.4426950408889634073599246810019f;
        });
    }

    [Test, Performance]
    [Category("Log2P1F")]
    public void MathematicsLog2P1F()
    {
        MeasurePerformance("Log2P1F", () =>
        {
            ResultF = math.log2(XF + 1.0f);
        });
    }

    [Test, Performance]
    [Category("Log2P1F")]
    public void BurstLowLog2P1F()
    {
        MeasurePerformance("Log2P1F", () =>
        {
            ResultF = BurstMathF.Log2P1Low(XF);
        });
    }

    [Test, Performance]
    [Category("Log2P1F")]
    public void BurstMediumLog2P1F()
    {
        MeasurePerformance("Log2P1F", () =>
        {
            ResultF = BurstMathF.Log2P1Medium(XF);
        });
    }

    [Test, Performance]
    [Category("Log2P1F")]
    public void BurstHighLog2P1F()
    {
        MeasurePerformance("Log2P1F", () =>
        {
            ResultF = BurstMathF.Log2P1High(XF);
        });
    }

    [Test, Performance]
    [Category("Log2P1F")]
    public void CoreLog2P1F()
    {
        MeasurePerformance("Log2P1F", () =>
        {
            ResultF = StrictMathF.Log2P1(XF);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Log2P1F")]
    public void PInvokeLog2P1F()
    {
        MeasurePerformance("Log2P1F", () =>
        {
            ResultF = PInvoke.PInvoke.Log2P1F(XF);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Log2P1F")]
    public void UnityLog2P1F()
    {
        MeasurePerformance("Log2P1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Log(x + 1.0f) * 1.4426950408889634073599246810019f;
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log2P1F")]
    public void MathematicsLog2P1F()
    {
        MeasurePerformance("Log2P1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.log2(x + 1.0f);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log2P1F")]
    public void BurstLowLog2P1F()
    {
        MeasurePerformance("Log2P1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Log2P1Low(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log2P1F")]
    public void BurstMediumLog2P1F()
    {
        MeasurePerformance("Log2P1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Log2P1Medium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log2P1F")]
    public void BurstHighLog2P1F()
    {
        MeasurePerformance("Log2P1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.Log2P1High(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Log2P1F")]
    public void CoreLog2P1F()
    {
        MeasurePerformance("Log2P1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Log2P1(x);
            }

            ResultF = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Log2P1F")]
    public void PInvokeLog2P1F()
    {
        MeasurePerformance("Log2P1F", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.Log2P1F(x);
            }

            ResultF = sum;
        });
    }
#endif
}
