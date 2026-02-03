using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark]
    [BenchmarkCategory("LGammaF")]
    public float CoreLGammaF()
    {
        return StrictMathF.LGamma(XF).value;
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark]
    [BenchmarkCategory("LGammaF")]
    public float CoreLGammaF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.LGamma(xf).value;
        }

        return sum;
    }
}
