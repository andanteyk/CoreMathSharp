using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("TanhF")]
    public void CoreTanhF()
    {
        MeasurePerformance("TanhF", () =>
        {
            ResultF = StrictMathF.Tanh(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("TanhF")]
    public void CoreTanhF()
    {
        MeasurePerformance("TanhF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Tanh(x);
            }

            ResultF = sum;
        });
    }
}
