using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("SinPi")]
    public void CoreSinPi()
    {
        MeasurePerformance("SinPi", () =>
        {
            Result = StrictMath.SinPi(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("SinPi")]
    public void CoreSinPi()
    {
        MeasurePerformance("SinPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.SinPi(x);
            }

            Result = sum;
        });
    }
}
