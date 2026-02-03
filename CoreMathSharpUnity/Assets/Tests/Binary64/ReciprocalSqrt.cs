using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("ReciprocalSqrt")]
    public void CoreReciprocalSqrt()
    {
        MeasurePerformance("ReciprocalSqrt", () =>
        {
            Result = StrictMath.ReciprocalSqrt(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("ReciprocalSqrt")]
    public void CoreReciprocalSqrt()
    {
        MeasurePerformance("ReciprocalSqrt", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.ReciprocalSqrt(x);
            }

            Result = sum;
        });
    }
}
