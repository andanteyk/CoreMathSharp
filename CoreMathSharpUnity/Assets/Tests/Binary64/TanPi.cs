using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("TanPi")]
    public void CoreTanPi()
    {
        MeasurePerformance("TanPi", () =>
        {
            Result = StrictMath.TanPi(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("TanPi")]
    public void CoreTanPi()
    {
        MeasurePerformance("TanPi", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.TanPi(x);
            }

            Result = sum;
        });
    }
}
