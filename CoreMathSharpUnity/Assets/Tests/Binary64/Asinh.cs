using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Asinh")]
    public void CoreAsinh()
    {
        MeasurePerformance("Asinh", () =>
        {
            Result = StrictMath.Asinh(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Asinh")]
    public void CoreAsinh()
    {
        MeasurePerformance("Asinh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Asinh(x);
            }

            Result = sum;
        });
    }
}
