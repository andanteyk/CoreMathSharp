using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("AtanPiF")]
    public float MathAtanPiF()
    {
        return float.AtanPi(XF);
    }

    [Benchmark]
    [BenchmarkCategory("AtanPiF")]
    public float CoreAtanPiF()
    {
        return StrictMathF.AtanPi(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("AtanPiF")]
    public float MathAtanPiF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += float.AtanPi(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("AtanPiF")]
    public float CoreAtanPiF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.AtanPi(xf);
        }

        return sum;
    }
}
