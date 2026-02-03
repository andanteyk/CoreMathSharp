using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("ErfcF")]
    public void CoreErfcF()
    {
        MeasurePerformance("ErfcF", () =>
        {
            ResultF = StrictMathF.Erfc(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("ErfcF")]
    public void CoreErfcF()
    {
        MeasurePerformance("ErfcF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Erfc(x);
            }

            ResultF = sum;
        });
    }
}
