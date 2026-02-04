using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Erf")]
    public void CoreErf()
    {
        MeasurePerformance("Erf", () =>
        {
            Result = StrictMath.Erf(X);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Erf")]
    public void PInvokeErf()
    {
        MeasurePerformance("Erf", () =>
        {
            Result = PInvoke.PInvoke.Erf(X);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Erf")]
    public void CoreErf()
    {
        MeasurePerformance("Erf", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Erf(x);
            }

            Result = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Erf")]
    public void PInvokeErf()
    {
        MeasurePerformance("Erf", () =>
        {
            double sum = 0.0;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.Erf(x);
            }

            Result = sum;
        });
    }
#endif
}
