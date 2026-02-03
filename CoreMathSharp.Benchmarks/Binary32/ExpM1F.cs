using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("ExpM1F")]
    public float MathExpM1F()
    {
        return float.ExpM1(XF);
    }

    [Benchmark]
    [BenchmarkCategory("ExpM1F")]
    public float CoreExpM1F()
    {
        return StrictMathF.ExpM1(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("ExpM1F")]
    public float MathExpM1F()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += float.ExpM1(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("ExpM1F")]
    public float CoreExpM1F()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.ExpM1(xf);
        }

        return sum;
    }
}
