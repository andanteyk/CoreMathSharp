using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("SinCos")]
    public void CoreSinCos()
    {
        MeasurePerformance("SinCos", () =>
        {
            var (sin, cos) = StrictMath.SinCos(X);
            Result = sin + cos;
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("SinCos")]
    public void CoreSinCos()
    {
        MeasurePerformance("SinCos", () =>
        {
            double sinsum = 0.0;
            double cossum = 0.0;

            foreach (var x in X)
            {
                var (sin, cos) = StrictMath.SinCos(x);
                sinsum += sin;
                cossum += cos;
            }

            Result = sinsum + cossum;
        });
    }
}
