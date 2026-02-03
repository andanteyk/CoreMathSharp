using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Acosh")]
    public void CoreAcosh()
    {
        MeasurePerformance("Acosh", () =>
        {
            Result = StrictMath.Acosh(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Acosh")]
    public void CoreAcosh()
    {
        MeasurePerformance("Acosh", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.Acosh(x);
            }

            Result = sum;
        });
    }
}
