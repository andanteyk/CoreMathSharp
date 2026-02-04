using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("AcosF")]
    public void UnityAcosF()
    {
        MeasurePerformance("AcosF", () =>
        {
            ResultF = Mathf.Acos(XF);
        });
    }

    [Test, Performance]
    [Category("AcosF")]
    public void MathematicsAcosF()
    {
        MeasurePerformance("AcosF", () =>
        {
            ResultF = math.acos(XF);
        });
    }

    [Test, Performance]
    [Category("AcosF")]
    public void BurstLowAcosF()
    {
        MeasurePerformance("AcosF", () =>
        {
            ResultF = BurstMathF.AcosLow(XF);
        });
    }

    [Test, Performance]
    [Category("AcosF")]
    public void BurstMediumAcosF()
    {
        MeasurePerformance("AcosF", () =>
        {
            ResultF = BurstMathF.AcosMedium(XF);
        });
    }

    [Test, Performance]
    [Category("AcosF")]
    public void BurstHighAcosF()
    {
        MeasurePerformance("AcosF", () =>
        {
            ResultF = BurstMathF.AcosHigh(XF);
        });
    }

    [Test, Performance]
    [Category("AcosF")]
    public void CoreAcosF()
    {
        MeasurePerformance("AcosF", () =>
        {
            ResultF = StrictMathF.Acos(XF);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("AcosF")]
    public void PInvokeAcosF()
    {
        MeasurePerformance("AcosF", () =>
        {
            ResultF = PInvoke.PInvoke.AcosF(XF);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("AcosF")]
    public void UnityAcosF()
    {
        MeasurePerformance("AcosF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += Mathf.Acos(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AcosF")]
    public void MathematicsAcosF()
    {
        MeasurePerformance("AcosF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.acos(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AcosF")]
    public void BurstLowAcosF()
    {
        MeasurePerformance("AcosF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AcosLow(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AcosF")]
    public void BurstMediumAcosF()
    {
        MeasurePerformance("AcosF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AcosMedium(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AcosF")]
    public void BurstHighAcosF()
    {
        MeasurePerformance("AcosF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += BurstMathF.AcosHigh(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("AcosF")]
    public void CoreAcosF()
    {
        MeasurePerformance("AcosF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Acos(x);
            }

            ResultF = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("AcosF")]
    public void PInvokeAcosF()
    {
        MeasurePerformance("AcosF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.AcosF(x);
            }

            ResultF = sum;
        });
    }
#endif
}
