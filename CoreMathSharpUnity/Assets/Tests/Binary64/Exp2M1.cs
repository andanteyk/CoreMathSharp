using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Exp2M1")]
    public void CoreExp2M1()
    {
        MeasurePerformance("Exp2M1", () =>
        {
            Result = StrictMath.Exp2M1(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Exp2M1")]
    public void CoreExp2M1()
    {
        MeasurePerformance("Exp2M1", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Exp2M1(x);
            }

            Result = sum;
        });
    }
}
