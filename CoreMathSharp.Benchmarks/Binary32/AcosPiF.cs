using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("AcosPiF")]
    public float MathAcosPiF()
    {
        return float.AcosPi(XF);
    }

    [Benchmark]
    [BenchmarkCategory("AcosPiF")]
    public float CoreAcosPiF()
    {
        return StrictMathF.AcosPi(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("AcosPiF")]
    public float MathAcosPiF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += float.AcosPi(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("AcosPiF")]
    public float CoreAcosPiF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.AcosPi(xf);
        }

        return sum;
    }
}
