using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Exp10")]
    public void CoreExp10()
    {
        MeasurePerformance("Exp10", () =>
        {
            Result = StrictMath.Exp10(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Exp10")]
    public void CoreExp10()
    {
        MeasurePerformance("Exp10", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Exp10(x);
            }

            Result = sum;
        });
    }
}
