using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("TGammaF")]
    public void CoreTGammaF()
    {
        MeasurePerformance("TGammaF", () =>
        {
            ResultF = StrictMathF.TGamma(XF);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("TGammaF")]
    public void PInvokeTGammaF()
    {
        MeasurePerformance("TGammaF", () =>
        {
            ResultF = PInvoke.PInvoke.TGammaF(XF);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("TGammaF")]
    public void CoreTGammaF()
    {
        MeasurePerformance("TGammaF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.TGamma(x);
            }

            ResultF = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("TGammaF")]
    public void PInvokeTGammaF()
    {
        MeasurePerformance("TGammaF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.TGammaF(x);
            }

            ResultF = sum;
        });
    }
#endif
}
