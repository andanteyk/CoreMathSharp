using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Sqrt")]
    public void CoreSqrt()
    {
        MeasurePerformance("Sqrt", () =>
        {
            Result = StrictMath.Sqrt(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Sqrt")]
    public void CoreSqrt()
    {
        MeasurePerformance("Sqrt", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Sqrt(x);
            }

            Result = sum;
        });
    }
}
