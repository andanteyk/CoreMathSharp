using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Log1P")]
    public void CoreLog1P()
    {
        MeasurePerformance("Log1P", () =>
        {
            Result = StrictMath.Log1P(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Log1P")]
    public void CoreLog1P()
    {
        MeasurePerformance("Log1P", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Log1P(x);
            }

            Result = sum;
        });
    }
}
