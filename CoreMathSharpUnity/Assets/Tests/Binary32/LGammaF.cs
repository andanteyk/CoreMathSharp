using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("LGammaF")]
    public void CoreLGammaF()
    {
        MeasurePerformance("LGammaF", () =>
        {
            ResultF = StrictMathF.LGamma(XF).value;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("LGammaF")]
    public void PInvokeLGammaF()
    {
        MeasurePerformance("LGammaF", () =>
        {
            ResultF = PInvoke.PInvoke.LGammaF(XF);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("LGammaF")]
    public void CoreLGammaF()
    {
        MeasurePerformance("LGammaF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.LGamma(x).value;
            }

            ResultF = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("LGammaF")]
    public void PInvokeLGammaF()
    {
        MeasurePerformance("LGammaF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.LGammaF(x);
            }

            ResultF = sum;
        });
    }
#endif
}
