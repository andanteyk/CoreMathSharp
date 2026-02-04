using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("ErfF")]
    public void CoreErfF()
    {
        MeasurePerformance("ErfF", () =>
        {
            ResultF = StrictMathF.Erf(XF);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("ErfF")]
    public void PInvokeErfF()
    {
        MeasurePerformance("ErfF", () =>
        {
            ResultF = PInvoke.PInvoke.ErfF(XF);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("ErfF")]
    public void CoreErfF()
    {
        MeasurePerformance("ErfF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Erf(x);
            }

            ResultF = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("ErfF")]
    public void PInvokeErfF()
    {
        MeasurePerformance("ErfF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.ErfF(x);
            }

            ResultF = sum;
        });
    }
#endif
}
