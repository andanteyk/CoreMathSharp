using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Log10P1")]
    public void CoreLog10P1()
    {
        MeasurePerformance("Log10P1", () =>
        {
            Result = StrictMath.Log10P1(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Log10P1")]
    public void CoreLog10P1()
    {
        MeasurePerformance("Log10P1", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Log10P1(x);
            }

            Result = sum;
        });
    }
}
