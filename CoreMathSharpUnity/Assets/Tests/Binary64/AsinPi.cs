using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("AsinPi")]
    public void CoreAsinPi()
    {
        MeasurePerformance("AsinPi", () =>
        {
            Result = StrictMath.AsinPi(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("AsinPi")]
    public void CoreAsinPi()
    {
        MeasurePerformance("AsinPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.AsinPi(x);
            }

            Result = sum;
        });
    }
}
