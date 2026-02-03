using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Acos")]
    public void CoreAcos()
    {
        MeasurePerformance("Acos", () =>
        {
            Result = StrictMath.Acos(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Acos")]
    public void CoreAcos()
    {
        MeasurePerformance("Acos", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Acos(x);
            }

            Result = sum;
        });
    }
}
