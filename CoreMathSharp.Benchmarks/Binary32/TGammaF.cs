using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark]
    [BenchmarkCategory("TGammaF")]
    public float CoreTGammaF()
    {
        return StrictMathF.TGamma(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark]
    [BenchmarkCategory("TGammaF")]
    public float CoreTGammaF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.TGamma(xf);
        }

        return sum;
    }
}
