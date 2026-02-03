using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Sin")]
    public void CoreSin()
    {
        MeasurePerformance("Sin", () =>
        {
            Result = StrictMath.Sin(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Sin")]
    public void CoreSin()
    {
        MeasurePerformance("Sin", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Sin(x);
            }

            Result = sum;
        });
    }
}
