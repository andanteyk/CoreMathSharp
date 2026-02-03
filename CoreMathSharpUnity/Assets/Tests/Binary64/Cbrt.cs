using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Cbrt")]
    public void CoreCbrt()
    {
        MeasurePerformance("Cbrt", () =>
        {
            Result = StrictMath.Cbrt(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Cbrt")]
    public void CoreCbrt()
    {
        MeasurePerformance("Cbrt", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Cbrt(x);
            }

            Result = sum;
        });
    }
}
