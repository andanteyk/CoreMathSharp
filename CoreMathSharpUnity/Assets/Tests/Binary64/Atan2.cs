using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Atan2")]
    public void CoreAtan2()
    {
        MeasurePerformance("Atan2", () =>
        {
            Result = StrictMath.Atan2(Y, X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Atan2")]
    public void CoreAtan2()
    {
        MeasurePerformance("Atan2", () =>
        {
            double sum = 0.0;

            for (int i = 0; i < X.Length; i++)
            {
                sum += StrictMath.Atan2(Y[i], X[i]);
            }

            Result = sum;
        });
    }
}
