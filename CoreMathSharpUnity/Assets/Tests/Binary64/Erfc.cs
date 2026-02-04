using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Erfc")]
    public void CoreErfc()
    {
        MeasurePerformance("Erfc", () =>
        {
            Result = StrictMath.Erfc(X);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Erfc")]
    public void PInvokeErfc()
    {
        MeasurePerformance("Erfc", () =>
        {
            Result = PInvoke.PInvoke.Erfc(X);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Erfc")]
    public void CoreErfc()
    {
        MeasurePerformance("Erfc", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Erfc(x);
            }

            Result = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Erfc")]
    public void PInvokeErfc()
    {
        MeasurePerformance("Erfc", () =>
        {
            double sum = 0.0;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.Erfc(x);
            }

            Result = sum;
        });
    }
#endif
}
