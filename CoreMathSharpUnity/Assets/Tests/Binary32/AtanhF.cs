using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("AtanhF")]
    public void CoreAtanhF()
    {
        MeasurePerformance("AtanhF", () =>
        {
            ResultF = StrictMathF.Atanh(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("AtanhF")]
    public void CoreAtanhF()
    {
        MeasurePerformance("AtanhF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Atanh(x);
            }

            ResultF = sum;
        });
    }
}
