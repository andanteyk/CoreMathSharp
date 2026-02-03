using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Log2")]
    public void CoreLog2()
    {
        MeasurePerformance("Log2", () =>
        {
            Result = StrictMath.Log2(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Log2")]
    public void CoreLog2()
    {
        MeasurePerformance("Log2", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Log2(x);
            }

            Result = sum;
        });
    }
}
