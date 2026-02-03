using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("AcosPi")]
    public void CoreAcosPi()
    {
        MeasurePerformance("AcosPi", () =>
        {
            Result = StrictMath.AcosPi(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("AcosPi")]
    public void CoreAcosPi()
    {
        MeasurePerformance("AcosPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.AcosPi(x);
            }

            Result = sum;
        });
    }
}
