using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("LogF")]
    public void UnityLogF()
    {
        MeasurePerformance("LogF", () =>
        {
            ResultF = Mathf.Log(XF);
        });
    }

    [Test, Performance]
    [Category("LogF")]
    public void MathematicsLogF()
    {
        MeasurePerformance("LogF", () =>
        {
            ResultF = math.log(XF);
        });
    }

    [Test, Performance]
    [Category("LogF")]
    public void BurstLowLogF()
    {
        MeasurePerformance("LogF", () =>
        {
            ResultF = BurstMathF.LogLow(XF);
        });
    }

    [Test, Performance]
    [Category("LogF")]
    public void BurstMediumLogF()
    {
        MeasurePerformance("LogF", () =>
        {
            ResultF = BurstMathF.LogMedium(XF);
        });
    }

    [Test, Performance]
    [Category("LogF")]
    public void BurstHighLogF()
    {
        MeasurePerformance("LogF", () =>
        {
            ResultF = BurstMathF.LogHigh(XF);
        });
    }

    [Test, Performance]
    [Category("LogF")]
    public void CoreLogF()
    {
        MeasurePerformance("LogF", () =>
        {
            ResultF = StrictMathF.Log(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("LogF")]
    public void UnityLogF()
    {
        MeasurePerformance("LogF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Log(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("LogF")]
    public void MathematicsLogF()
    {
        MeasurePerformance("LogF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.log(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("LogF")]
    public void BurstLowLogF()
    {
        MeasurePerformance("LogF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.LogLow(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("LogF")]
    public void BurstMediumLogF()
    {
        MeasurePerformance("LogF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.LogMedium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("LogF")]
    public void BurstHighLogF()
    {
        MeasurePerformance("LogF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.LogHigh(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("LogF")]
    public void CoreLogF()
    {
        MeasurePerformance("LogF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Log(x);
            }

            ResultF = sum;
        });
    }
}
