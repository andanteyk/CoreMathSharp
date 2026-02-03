using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("SinhF")]
    public void CoreSinhF()
    {
        MeasurePerformance("SinhF", () =>
        {
            ResultF = StrictMathF.Sinh(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("SinhF")]
    public void CoreSinhF()
    {
        MeasurePerformance("SinhF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Sinh(x);
            }

            ResultF = sum;
        });
    }
}
