using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("FusedMultiplyAddF")]
    public void UnityFusedMultiplyAddF()
    {
        MeasurePerformance("FusedMultiplyAddF", () =>
        {
            ResultF = XF * YF + ZF;
        });
    }

    [Test, Performance]
    [Category("FusedMultiplyAddF")]
    public void MathematicsFusedMultiplyAddF()
    {
        MeasurePerformance("FusedMultiplyAddF", () =>
        {
            ResultF = math.mad(XF, YF, ZF);
        });
    }

    [Test, Performance]
    [Category("FusedMultiplyAddF")]
    public void BurstLowFusedMultiplyAddF()
    {
        MeasurePerformance("FusedMultiplyAddF", () =>
        {
            ResultF = BurstMathF.FusedMultiplyAddLow(XF, YF, ZF);
        });
    }

    [Test, Performance]
    [Category("FusedMultiplyAddF")]
    public void BurstMediumFusedMultiplyAddF()
    {
        MeasurePerformance("FusedMultiplyAddF", () =>
        {
            ResultF = BurstMathF.FusedMultiplyAddMedium(XF, YF, ZF);
        });
    }

    [Test, Performance]
    [Category("FusedMultiplyAddF")]
    public void BurstHighFusedMultiplyAddF()
    {
        MeasurePerformance("FusedMultiplyAddF", () =>
        {
            ResultF = BurstMathF.FusedMultiplyAddHigh(XF, YF, ZF);
        });
    }

    [Test, Performance]
    [Category("FusedMultiplyAddF")]
    public void CoreFusedMultiplyAddF()
    {
        MeasurePerformance("FusedMultiplyAddF", () =>
        {
            ResultF = StrictMathF.FusedMultiplyAdd(XF, YF, ZF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("FusedMultiplyAddF")]
    public void UnityFusedMultiplyAddF()
    {
        MeasurePerformance("FusedMultiplyAddF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += XF[i] * YF[i] + ZF[i];
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("FusedMultiplyAddF")]
    public void MathematicsFusedMultiplyAddF()
    {
        MeasurePerformance("FusedMultiplyAddF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += math.mad(XF[i], YF[i], ZF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("FusedMultiplyAddF")]
    public void BurstLowFusedMultiplyAddF()
    {
        MeasurePerformance("FusedMultiplyAddF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += BurstMathF.FusedMultiplyAddLow(XF[i], YF[i], ZF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("FusedMultiplyAddF")]
    public void BurstMediumFusedMultiplyAddF()
    {
        MeasurePerformance("FusedMultiplyAddF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += BurstMathF.FusedMultiplyAddMedium(XF[i], YF[i], ZF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("FusedMultiplyAddF")]
    public void BurstHighFusedMultiplyAddF()
    {
        MeasurePerformance("FusedMultiplyAddF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += BurstMathF.FusedMultiplyAddHigh(XF[i], YF[i], ZF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("FusedMultiplyAddF")]
    public void CoreFusedMultiplyAddF()
    {
        MeasurePerformance("FusedMultiplyAddF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += StrictMathF.FusedMultiplyAdd(XF[i], YF[i], ZF[i]);
            }

            ResultF = sum;
        });
    }
}
