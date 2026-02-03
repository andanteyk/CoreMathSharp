using CoreMathSharp;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("CosF")]
    public void MathematicsCoshF()
    {
        MeasurePerformance("CoshF", () =>
        {
            ResultF = math.cosh(XF);
        });
    }

    [Test, Performance]
    [Category("CoshF")]
    public void CoreCoshF()
    {
        MeasurePerformance("CoshF", () =>
        {
            ResultF = StrictMathF.Cosh(XF);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("CoshF")]
    public void MathematicsCoshF()
    {
        MeasurePerformance("CoshF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += math.cosh(x);
            }

            ResultF = sum;
        });
    }

    [Test, Performance]
    [Category("CoshF")]
    public void CoreCoshF()
    {
        MeasurePerformance("CoshF", () =>
        {
            float sum = 0.0f;

            foreach (var x in XF)
            {
                sum += StrictMathF.Cosh(x);
            }

            ResultF = sum;
        });
    }
}
