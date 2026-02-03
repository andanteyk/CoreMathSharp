using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Atanh")]
    public void CoreAtanh()
    {
        MeasurePerformance("Atanh", () =>
        {
            Result = StrictMath.Atanh(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Atanh")]
    public void CoreAtanh()
    {
        MeasurePerformance("Atanh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Atanh(x);
            }

            Result = sum;
        });
    }
}
