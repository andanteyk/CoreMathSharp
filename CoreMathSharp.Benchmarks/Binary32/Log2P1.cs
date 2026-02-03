using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Log2P1F")]
    public float MathLog2P1F()
    {
        return float.Log2P1(XF);
    }

    [Benchmark]
    [BenchmarkCategory("Log2P1F")]
    public float CoreLog2P1F()
    {
        return StrictMathF.Log2P1(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Log2P1F")]
    public float MathLog2P1F()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += float.Log2P1(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Log2P1F")]
    public float CoreLog2P1F()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Log2P1(xf);
        }

        return sum;
    }
}
