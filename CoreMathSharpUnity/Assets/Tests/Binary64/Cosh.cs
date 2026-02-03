using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Cosh")]
    public void CoreCosh()
    {
        MeasurePerformance("Cosh", () =>
        {
            Result = StrictMath.Cosh(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Cosh")]
    public void CoreCosh()
    {
        MeasurePerformance("Cosh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Cosh(x);
            }

            Result = sum;
        });
    }
}
