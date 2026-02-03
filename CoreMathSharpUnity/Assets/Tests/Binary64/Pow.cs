using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Pow")]
    public void CorePow()
    {
        MeasurePerformance("Pow", () =>
        {
            Result = StrictMath.Pow(X, Y);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Pow")]
    public void CorePow()
    {
        MeasurePerformance("Pow", () =>
        {
            double sum = 0.0;

            for (int i = 0; i < X.Length; i++)
            {
                sum += StrictMath.Pow(X[i], Y[i]);
            }

            Result = sum;
        });
    }
}
