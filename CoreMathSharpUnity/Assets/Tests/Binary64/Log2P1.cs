using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Log2P1")]
    public void CoreLog2P1()
    {
        MeasurePerformance("Log2P1", () =>
        {
            Result = StrictMath.Log2P1(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Log2P1")]
    public void CoreLog2P1()
    {
        MeasurePerformance("Log2P1", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Log2P1(x);
            }

            Result = sum;
        });
    }
}
