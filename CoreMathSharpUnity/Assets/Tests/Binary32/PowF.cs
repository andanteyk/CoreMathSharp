using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("PowF")]
    public void UnityPowF()
    {
        MeasurePerformance("PowF", () =>
        {
            ResultF = Mathf.Pow(XF, YF);
        });
    }

    [Test, Performance]
    [Category("PowF")]
    public void MathematicsPowF()
    {
        MeasurePerformance("PowF", () =>
        {
            ResultF = math.pow(XF, YF);
        });
    }

    [Test, Performance]
    [Category("PowF")]
    public void BurstLowPowF()
    {
        MeasurePerformance("PowF", () =>
        {
            ResultF = BurstMathF.PowLow(XF, YF);
        });
    }

    [Test, Performance]
    [Category("PowF")]
    public void BurstMediumPowF()
    {
        MeasurePerformance("PowF", () =>
        {
            ResultF = BurstMathF.PowMedium(XF, YF);
        });
    }

    [Test, Performance]
    [Category("PowF")]
    public void BurstHighPowF()
    {
        MeasurePerformance("PowF", () =>
        {
            ResultF = BurstMathF.PowHigh(XF, YF);
        });
    }

    [Test, Performance]
    [Category("PowF")]
    public void CorePowF()
    {
        MeasurePerformance("PowF", () =>
        {
            ResultF = StrictMathF.Pow(XF, YF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("PowF")]
    public void UnityPowF()
    {
        MeasurePerformance("PowF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += Mathf.Pow(XF[i], YF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("PowF")]
    public void MathematicsPowF()
    {
        MeasurePerformance("PowF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += math.pow(XF[i], YF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("PowF")]
    public void BurstLowPowF()
    {
        MeasurePerformance("PowF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += BurstMathF.PowLow(XF[i], YF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("PowF")]
    public void BurstMediumPowF()
    {
        MeasurePerformance("PowF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += BurstMathF.PowMedium(XF[i], YF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("PowF")]
    public void BurstHighPowF()
    {
        MeasurePerformance("PowF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += BurstMathF.PowHigh(XF[i], YF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("PowF")]
    public void CorePowF()
    {
        MeasurePerformance("PowF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += StrictMathF.Pow(XF[i], YF[i]);
            }

            ResultF = sum;
        });
    }
}
