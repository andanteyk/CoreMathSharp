using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("TGamma")]
    public void CoreTGamma()
    {
        MeasurePerformance("TGamma", () =>
        {
            Result = StrictMath.TGamma(X);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("TGamma")]
    public void CoreTGamma()
    {
        MeasurePerformance("TGamma", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += StrictMath.TGamma(x);
            }

            Result = sum;
        });
    }
}
