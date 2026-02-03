using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Asin")]
    public void CoreAsin()
    {
        MeasurePerformance("Asin", () =>
        {
            Result = StrictMath.Asin(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Asin")]
    public void CoreAsin()
    {
        MeasurePerformance("Asin", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Asin(x);
            }

            Result = sum;
        });
    }
}
