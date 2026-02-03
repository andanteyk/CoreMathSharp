using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("AsinPiF")]
    public float MathAsinPiF()
    {
        return float.AsinPi(XF);
    }

    [Benchmark]
    [BenchmarkCategory("AsinPiF")]
    public float CoreAsinPiF()
    {
        return StrictMathF.AsinPi(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("AsinPiF")]
    public float MathAsinPiF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += float.AsinPi(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("AsinPiF")]
    public float CoreAsinPiF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.AsinPi(xf);
        }

        return sum;
    }
}
