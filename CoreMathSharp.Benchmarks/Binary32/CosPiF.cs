using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("CosPiF")]
    public float MathCosPiF()
    {
        return float.CosPi(XF);
    }

    [Benchmark]
    [BenchmarkCategory("CosPiF")]
    public float CoreCosPiF()
    {
        return StrictMathF.CosPi(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("CosPiF")]
    public float MathCosPiF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += float.CosPi(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("CosPiF")]
    public float CoreCosPiF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.CosPi(xf);
        }

        return sum;
    }
}
