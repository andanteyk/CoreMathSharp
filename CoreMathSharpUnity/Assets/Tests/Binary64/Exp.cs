using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Exp")]
    public void CoreExp()
    {
        MeasurePerformance("Exp", () =>
        {
            Result = StrictMath.Exp(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Exp")]
    public void CoreExp()
    {
        MeasurePerformance("Exp", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Exp(x);
            }

            Result = sum;
        });
    }
}
