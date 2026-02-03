using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("ExpM1")]
    public void CoreExpM1()
    {
        MeasurePerformance("ExpM1", () =>
        {
            Result = StrictMath.ExpM1(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("ExpM1")]
    public void CoreExpM1()
    {
        MeasurePerformance("ExpM1", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.ExpM1(x);
            }

            Result = sum;
        });
    }
}
