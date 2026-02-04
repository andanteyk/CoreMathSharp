using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("LGamma")]
    public void CoreLGamma()
    {
        MeasurePerformance("LGamma", () =>
        {
            Result = StrictMath.LGamma(X).value;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("LGamma")]
    public void PInvokeLGamma()
    {
        MeasurePerformance("LGamma", () =>
        {
            Result = PInvoke.PInvoke.LGamma(X);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("LGamma")]
    public void CoreLGamma()
    {
        MeasurePerformance("LGamma", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.LGamma(x).value;
            }

            Result = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("LGamma")]
    public void PInvokeLGamma()
    {
        MeasurePerformance("LGamma", () =>
        {
            double sum = 0.0;

            foreach (var x in XF)
            {
                sum += PInvoke.PInvoke.LGamma(x);
            }

            Result = sum;
        });
    }
#endif
}
