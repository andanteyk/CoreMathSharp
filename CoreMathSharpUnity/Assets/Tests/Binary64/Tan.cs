using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Tan")]
    public void CoreTan()
    {
        MeasurePerformance("Tan", () =>
        {
            Result = StrictMath.Tan(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Tan")]
    public void CoreTan()
    {
        MeasurePerformance("Tan", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Tan(x);
            }

            Result = sum;
        });
    }
}
