using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Atan")]
    public void CoreAtan()
    {
        MeasurePerformance("Atan", () =>
        {
            Result = StrictMath.Atan(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Atan")]
    public void CoreAtan()
    {
        MeasurePerformance("Atan", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Atan(x);
            }

            Result = sum;
        });
    }
}
