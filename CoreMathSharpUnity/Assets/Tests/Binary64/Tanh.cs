using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Tanh")]
    public void CoreTanh()
    {
        MeasurePerformance("Tanh", () =>
        {
            Result = StrictMath.Tanh(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Tanh")]
    public void CoreTanh()
    {
        MeasurePerformance("Tanh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Tanh(x);
            }

            Result = sum;
        });
    }
}
