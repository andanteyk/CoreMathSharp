using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("TGamma")]
    public void CoreTGamma()
    {
        MeasurePerformance("TGamma", () =>
        {
            Result = StrictMath.TGamma(X);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("TGamma")]
    public void PInvokeTGamma()
    {
        MeasurePerformance("TGamma", () =>
        {
            Result = PInvoke.PInvoke.TGamma(X);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("TGamma")]
    public void CoreTGamma()
    {
        MeasurePerformance("TGamma", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.TGamma(x);
            }

            Result = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("TGamma")]
    public void PInvokeTGamma()
    {
        MeasurePerformance("TGamma", () =>
        {
            double sum = 0.0;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.TGamma(x);
            }

            Result = sum;
        });
    }
#endif
}
