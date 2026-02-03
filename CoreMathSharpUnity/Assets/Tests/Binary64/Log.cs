using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Log")]
    public void CoreLog()
    {
        MeasurePerformance("Log", () =>
        {
            Result = StrictMath.Log(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Log")]
    public void CoreLog()
    {
        MeasurePerformance("Log", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Log(x);
            }

            Result = sum;
        });
    }
}
