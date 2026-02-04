using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("CompoundF")]
    public void UnityCompoundF()
    {
        MeasurePerformance("CompoundF", () =>
        {
            ResultF = Mathf.Pow(XF + 1f, YF);
        });
    }

    [Test, Performance]
    [Category("CompoundF")]
    public void MathematicsCompoundF()
    {
        MeasurePerformance("CompoundF", () =>
        {
            ResultF = math.pow(XF + 1f, YF);
        });
    }

    [Test, Performance]
    [Category("CompoundF")]
    public void BurstLowCompoundF()
    {
        MeasurePerformance("CompoundF", () =>
        {
            ResultF = BurstMathF.CompoundLow(XF, YF);
        });
    }

    [Test, Performance]
    [Category("CompoundF")]
    public void BurstMediumCompoundF()
    {
        MeasurePerformance("CompoundF", () =>
        {
            ResultF = BurstMathF.CompoundMedium(XF, YF);
        });
    }

    [Test, Performance]
    [Category("CompoundF")]
    public void BurstHighCompoundF()
    {
        MeasurePerformance("CompoundF", () =>
        {
            ResultF = BurstMathF.CompoundHigh(XF, YF);
        });
    }

    [Test, Performance]
    [Category("CompoundF")]
    public void CoreCompoundF()
    {
        MeasurePerformance("CompoundF", () =>
        {
            ResultF = StrictMathF.Compound(XF, YF);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("CompoundF")]
    public void PInvokeCompoundF()
    {
        MeasurePerformance("CompoundF", () =>
        {
            ResultF = PInvoke.PInvoke.CompoundF(XF, YF);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("CompoundF")]
    public void UnityCompoundF()
    {
        MeasurePerformance("CompoundF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += Mathf.Pow(XF[i] + 1f, YF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CompoundF")]
    public void MathematicsCompoundF()
    {
        MeasurePerformance("CompoundF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += math.pow(XF[i] + 1f, YF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CompoundF")]
    public void BurstLowCompoundF()
    {
        MeasurePerformance("CompoundF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += BurstMathF.CompoundLow(XF[i], YF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CompoundF")]
    public void BurstMediumCompoundF()
    {
        MeasurePerformance("CompoundF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += BurstMathF.CompoundMedium(XF[i], YF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CompoundF")]
    public void BurstHighCompoundF()
    {
        MeasurePerformance("CompoundF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += BurstMathF.CompoundHigh(XF[i], YF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CompoundF")]
    public void CoreCompoundF()
    {
        MeasurePerformance("CompoundF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += StrictMathF.Compound(XF[i], YF[i]);
            }

            ResultF = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("CompoundF")]
    public void PInvokeCompoundF()
    {
        MeasurePerformance("CompoundF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += PInvoke.PInvoke.CompoundF(XF[i], YF[i]);
            }

            ResultF = sum;
        });
    }
#endif
}
