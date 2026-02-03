using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Cos")]
    public void CoreCos()
    {
        MeasurePerformance("Cos", () =>
        {
            Result = StrictMath.Cos(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Cos")]
    public void CoreCos()
    {
        MeasurePerformance("Cos", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Cos(x);
            }

            Result = sum;
        });
    }
}
