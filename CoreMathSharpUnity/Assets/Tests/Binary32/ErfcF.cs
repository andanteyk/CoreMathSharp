using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("ErfcF")]
    public void CoreErfcF()
    {
        MeasurePerformance("ErfcF", () =>
        {
            ResultF = StrictMathF.Erfc(XF);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("ErfcF")]
    public void PInvokeErfcF()
    {
        MeasurePerformance("ErfcF", () =>
        {
            ResultF = PInvoke.PInvoke.ErfcF(XF);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("ErfcF")]
    public void CoreErfcF()
    {
        MeasurePerformance("ErfcF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Erfc(x);
            }

            ResultF = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("ErfcF")]
    public void PInvokeErfcF()
    {
        MeasurePerformance("ErfcF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.ErfcF(x);
            }

            ResultF = sum;
        });
    }
#endif
}
