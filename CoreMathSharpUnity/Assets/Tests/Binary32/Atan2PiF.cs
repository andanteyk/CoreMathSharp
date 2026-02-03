using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Atan2PiF")]
    public void UnityAtan2PiF()
    {
        MeasurePerformance("Atan2PiF", () =>
        {
            ResultF = Mathf.Atan2(YF, XF) / Mathf.PI;
        });
    }

    [Test, Performance]
    [Category("Atan2PiF")]
    public void MathematicsAtan2PiF()
    {
        MeasurePerformance("Atan2PiF", () =>
        {
            ResultF = math.atan2(YF, XF) / math.PI;
        });
    }

    [Test, Performance]
    [Category("Atan2PiF")]
    public void BurstLowAtan2PiF()
    {
        MeasurePerformance("Atan2PiF", () =>
        {
            ResultF = BurstMathF.Atan2PiLow(YF, XF);
        });
    }

    [Test, Performance]
    [Category("Atan2PiF")]
    public void BurstMediumAtan2PiF()
    {
        MeasurePerformance("Atan2PiF", () =>
        {
            ResultF = BurstMathF.Atan2PiMedium(YF, XF);
        });
    }

    [Test, Performance]
    [Category("Atan2PiF")]
    public void BurstHighAtan2PiF()
    {
        MeasurePerformance("Atan2PiF", () =>
        {
            ResultF = BurstMathF.Atan2PiHigh(YF, XF);
        });
    }

    [Test, Performance]
    [Category("Atan2PiF")]
    public void CoreAtan2PiF()
    {
        MeasurePerformance("Atan2PiF", () =>
        {
            ResultF = StrictMathF.Atan2Pi(YF, XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Atan2PiF")]
    public void UnityAtan2PiF()
    {
        MeasurePerformance("Atan2PiF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += Mathf.Atan2(YF[i], XF[i]) / Mathf.PI;
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Atan2PiF")]
    public void MathematicsAtan2PiF()
    {
        MeasurePerformance("Atan2PiF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += math.atan2(YF[i], XF[i]) / math.PI;
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Atan2PiF")]
    public void BurstLowAtan2PiF()
    {
        MeasurePerformance("Atan2PiF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += BurstMathF.Atan2PiLow(YF[i], XF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Atan2PiF")]
    public void BurstMediumAtan2PiF()
    {
        MeasurePerformance("Atan2PiF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += BurstMathF.Atan2PiMedium(YF[i], XF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Atan2PiF")]
    public void BurstHighAtan2PiF()
    {
        MeasurePerformance("Atan2PiF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += BurstMathF.Atan2PiHigh(YF[i], XF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Atan2PiF")]
    public void CoreAtan2PiF()
    {
        MeasurePerformance("Atan2PiF", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += StrictMathF.Atan2Pi(YF[i], XF[i]);
            }

            ResultF = sum;
        });
    }
}
