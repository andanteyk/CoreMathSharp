using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Exp10M1")]
    public void CoreExp10M1()
    {
        MeasurePerformance("Exp10M1", () =>
        {
            Result = StrictMath.Exp10M1(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Exp10M1")]
    public void CoreExp10M1()
    {
        MeasurePerformance("Exp10M1", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Exp10M1(x);
            }

            Result = sum;
        });
    }
}
