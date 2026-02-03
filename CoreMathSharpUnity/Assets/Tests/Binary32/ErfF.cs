using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("ErfF")]
    public void CoreErfF()
    {
        MeasurePerformance("ErfF", () =>
        {
            ResultF = StrictMathF.Erf(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("ErfF")]
    public void CoreErfF()
    {
        MeasurePerformance("ErfF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Erf(x);
            }

            ResultF = sum;
        });
    }
}
