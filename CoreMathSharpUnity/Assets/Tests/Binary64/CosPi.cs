using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("CosPi")]
    public void CoreCosPi()
    {
        MeasurePerformance("CosPi", () =>
        {
            Result = StrictMath.CosPi(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("CosPi")]
    public void CoreCosPi()
    {
        MeasurePerformance("CosPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.CosPi(x);
            }

            Result = sum;
        });
    }
}
