using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Atan2F")]
    public void UnityAtan2F()
    {
        MeasurePerformance("Atan2F", () =>
        {
            ResultF = Mathf.Atan2(YF, XF);
        });
    }

    [Test, Performance]
    [Category("Atan2F")]
    public void MathematicsAtan2F()
    {
        MeasurePerformance("Atan2F", () =>
        {
            ResultF = math.atan2(YF, XF);
        });
    }

    [Test, Performance]
    [Category("Atan2F")]
    public void BurstLowAtan2F()
    {
        MeasurePerformance("Atan2F", () =>
        {
            ResultF = BurstMathF.Atan2Low(YF, XF);
        });
    }

    [Test, Performance]
    [Category("Atan2F")]
    public void BurstMediumAtan2F()
    {
        MeasurePerformance("Atan2F", () =>
        {
            ResultF = BurstMathF.Atan2Medium(YF, XF);
        });
    }

    [Test, Performance]
    [Category("Atan2F")]
    public void BurstHighAtan2F()
    {
        MeasurePerformance("Atan2F", () =>
        {
            ResultF = BurstMathF.Atan2High(YF, XF);
        });
    }

    [Test, Performance]
    [Category("Atan2F")]
    public void CoreAtan2F()
    {
        MeasurePerformance("Atan2F", () =>
        {
            ResultF = StrictMathF.Atan2(YF, XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Atan2F")]
    public void UnityAtan2F()
    {
        MeasurePerformance("Atan2F", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += Mathf.Atan2(YF[i], XF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Atan2F")]
    public void MathematicsAtan2F()
    {
        MeasurePerformance("Atan2F", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += math.atan2(YF[i], XF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Atan2F")]
    public void BurstLowAtan2F()
    {
        MeasurePerformance("Atan2F", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += BurstMathF.Atan2Low(YF[i], XF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Atan2F")]
    public void BurstMediumAtan2F()
    {
        MeasurePerformance("Atan2F", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += BurstMathF.Atan2Medium(YF[i], XF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Atan2F")]
    public void BurstHighAtan2F()
    {
        MeasurePerformance("Atan2F", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += BurstMathF.Atan2High(YF[i], XF[i]);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("Atan2F")]
    public void CoreAtan2F()
    {
        MeasurePerformance("Atan2F", () =>
        {
            float sum = 0.0f;

            for (int i = 0; i < XF.Length; i++)
            {
                sum += StrictMathF.Atan2(YF[i], XF[i]);
            }

            ResultF = sum;
        });
    }
}
