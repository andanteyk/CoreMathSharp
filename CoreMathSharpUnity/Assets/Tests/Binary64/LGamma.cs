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
}
