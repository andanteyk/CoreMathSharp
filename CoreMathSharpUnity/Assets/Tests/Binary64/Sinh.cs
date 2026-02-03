using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Sinh")]
    public void CoreSinh()
    {
        MeasurePerformance("Sinh", () =>
        {
            Result = StrictMath.Sinh(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Sinh")]
    public void CoreSinh()
    {
        MeasurePerformance("Sinh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Sinh(x);
            }

            Result = sum;
        });
    }
}
