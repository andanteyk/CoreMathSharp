using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("AtanPi")]
    public void CoreAtanPi()
    {
        MeasurePerformance("AtanPi", () =>
        {
            Result = StrictMath.AtanPi(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("AtanPi")]
    public void CoreAtanPi()
    {
        MeasurePerformance("AtanPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.AtanPi(x);
            }

            Result = sum;
        });
    }
}
