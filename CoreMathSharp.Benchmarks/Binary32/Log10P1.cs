using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Log10P1F")]
    public float MathLog10P1F()
    {
        return float.Log10P1(XF);
    }

    [Benchmark]
    [BenchmarkCategory("Log10P1F")]
    public float CoreLog10P1F()
    {
        return StrictMathF.Log10P1(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Log10P1F")]
    public float MathLog10P1F()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += float.Log10P1(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Log10P1F")]
    public float CoreLog10P1F()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Log10P1(xf);
        }

        return sum;
    }
}
