using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Exp2")]
    public void CoreExp2()
    {
        MeasurePerformance("Exp2", () =>
        {
            Result = StrictMath.Exp2(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Exp2")]
    public void CoreExp2()
    {
        MeasurePerformance("Exp2", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Exp2(x);
            }

            Result = sum;
        });
    }
}
