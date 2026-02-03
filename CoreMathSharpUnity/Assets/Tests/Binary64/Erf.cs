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
}
